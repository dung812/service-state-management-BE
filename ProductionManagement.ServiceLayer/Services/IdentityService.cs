using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ProductionManagement.DataContract.Identity;
using ProductionManagement.Exceptions;
using ProductionManagement.ServiceLayer.Interfaces;
using ProductionManagement.Models;
using ProductionManagement.ServiceLayer.Constants;
using ProductionManagement.DataContract.Constant;
using Microsoft.AspNetCore.Http;

namespace ProductionManagement.ServiceLayer.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

		public IdentityService(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<LoginResponseContract?> AuthorizeAsync(LoginContract loginModel)
        {

            var user = await _userManager.FindByNameAsync(loginModel.Username) ?? throw new NotFoundException("Username or password is invalid");
            if (user.IsDisabled)
            {
                throw new RestrictedPermissionException("User is disabled");
            }
            var isPasswordMatch = await _userManager.CheckPasswordAsync(user, loginModel.Password);
            if (!isPasswordMatch)
            {
                throw new NotFoundException("Username or password is invalid");
            }

            var roles = await _userManager.GetRolesAsync(user);

			var claims = new[]
		   {
				new Claim(ClaimTypes.Name,user.UserName),
				new Claim(ClaimTypes.GivenName, user.Name),
				new Claim(CustomClaimTypes.ConcurrencyStamp, user.ConcurrencyStamp),
				new Claim(ClaimTypes.Role, string.Join(Separator.CommaSeparator, roles)),
			};

			var token = CreateToken(claims);

			var loginResponse = new LoginResponseContract()
            {
                Name = user.Name,
                Email= user.Email,
                IsFirstLogin = false,
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token)
            };
            return loginResponse;
        }

        private JwtSecurityToken CreateToken(IEnumerable<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out var tokenValidityInMinutes);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.UtcNow.AddMinutes(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
	}
}
