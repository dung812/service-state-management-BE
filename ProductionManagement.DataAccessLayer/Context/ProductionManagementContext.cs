using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Logging;
using ProductionManagement.DataAccessLayer.AuditEntry;
using ProductionManagement.DataAccessLayer.CurrentUser;
using ProductionManagement.Models;
using ProductionManagement.Models.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ProductionManagement.DataAccessLayer.Context
{
    public class ProductionManagementContext : IdentityDbContext<User>
    {
        private readonly ICurrentUserModel _currentUserModel;
		private string _connectionString;

		public string ConnectionString
		{
			get =>
				string.IsNullOrEmpty(_connectionString)
					? GetConnectionString("local_ProductionManagement")
					: _connectionString;
			set => _connectionString = value;
		}

		public ProductionManagementContext() : base()
        {
        }
		private string GetConnectionString(string name)
		{
			var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
			var configurationBuilder = new ConfigurationBuilder();
			var path = Path.Combine(Directory.GetCurrentDirectory(), $"appsettings.{envName}.json");
			configurationBuilder.AddJsonFile(path, false);
			var root = configurationBuilder.Build();
			_connectionString = root.GetSection("ConnectionStrings").GetSection(name).Value;
			return _connectionString;
		}

		public ProductionManagementContext(DbContextOptions<ProductionManagementContext> options,
            ICurrentUserModel currentUserModel) : base(options)
        {
            _currentUserModel = currentUserModel;
			ChangeTracker.StateChanged += UpdateTimestampsAndUser;
			ChangeTracker.Tracked += UpdateTimestampsAndUser;
        }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
			if (optionsBuilder.IsConfigured && ConnectionString != null) return;

			base.OnConfiguring(optionsBuilder);
			if (ConnectionString != null)
				optionsBuilder.UseSqlServer(ConnectionString)
					.LogTo(Console.WriteLine, LogLevel.Information);
		}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.SetTableName(entityType.GetTableName());
            }
        }

		private void UpdateTimestampsAndUser(object sender, EntityEntryEventArgs e)
        {
			if (e.Entry.Entity is IEntity<Guid> entity && e.Entry.Entity is not Audit)
			{
                var entityEntry = e.Entry;
                var auditEntry = new AuditEntry.AuditEntry(entityEntry)
                {
                    TableName = entityEntry.Entity.GetType().Name,
                    UserName = _currentUserModel.Username
				};

                foreach (var property in entityEntry.Properties)
                {
                    var propertyName = property.Metadata.Name;

                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }

					switch (e.Entry.State)
					{

						case EntityState.Added:
							auditEntry.AuditType = AuditType.Create;
							auditEntry.NewValues[propertyName] = property.CurrentValue;

							entity.CreatedDate = DateTime.UtcNow;
							entity.CreatedBy = _currentUserModel.Username;
							break;
						case EntityState.Modified:
							auditEntry.AuditType = AuditType.Delete;
							auditEntry.OldValues[propertyName] = property.OriginalValue;

							entity.ModifiedDate = DateTime.UtcNow;
							entity.ModifiedBy = _currentUserModel.Username;
							break;
						case EntityState.Deleted:
							auditEntry.AuditType = AuditType.Update;

							if (property.OriginalValue != null && property.CurrentValue != null &&
								!property.OriginalValue.Equals(property.CurrentValue))
							{
								auditEntry.ChangedColumns.Add(propertyName);
								auditEntry.OldValues[propertyName] = property.OriginalValue;
								auditEntry.NewValues[propertyName] = property.CurrentValue;
							}
							else if ((property.OriginalValue == null && property.CurrentValue != null) ||
									 (property.OriginalValue != null && property.CurrentValue == null))
							{
								auditEntry.ChangedColumns.Add(propertyName);
								auditEntry.OldValues[propertyName] = property.OriginalValue;
								auditEntry.NewValues[propertyName] = property.CurrentValue;
							}

							entity.DeletedDate = DateTime.UtcNow;
							entity.DeletedBy = _currentUserModel.Username;
							break;
					}
				}
                Set<Audit>().Add(auditEntry.ToAudit());
			}
		}
    }
}