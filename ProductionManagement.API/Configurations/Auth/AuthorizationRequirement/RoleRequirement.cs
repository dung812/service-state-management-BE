using Microsoft.AspNetCore.Authorization;

namespace ProductionManagement.API.Configurations.Auth.AuthorizationRequirement
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public string Roles { get; }

        public RoleRequirement(string roleNames)
        {
            Roles = roleNames;
        }
    }
}
