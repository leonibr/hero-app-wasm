using HeroApp.Domain;
using HeroApp.Infra;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HeroApp.App.Services
{
    public interface IAuthUserService
    {
        Task<bool> UserExists(string userName);
        Task<IdentityResult> CreateMinimunUser(string userName, string password);
        Task<string> GenerateToken(string userName);
        Task<AppUser> GetUser(string email);
        Task<AppUser> GetUserById(string userId);

    }
    public class AuthUserService : IAuthUserService
    {
        private readonly IHeroContext heroContext;
        private readonly IWebHostEnvironment env;
        private readonly SigningConfigurations signingConfigurations;
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<AppRole> roleManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly TokenConfigurations tokenConfigurations;

        public AuthUserService(IHeroContext heroContext,
                    IWebHostEnvironment env,
                    SigningConfigurations signingConfigurations,
                    UserManager<AppUser> userManager,
                    RoleManager<AppRole> roleManager,
                    SignInManager<AppUser> signInManager,
                    TokenConfigurations tokenConfigurations)
        {
            this.heroContext = heroContext;
            this.env = env;
            this.signingConfigurations = signingConfigurations;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.tokenConfigurations = tokenConfigurations;
        }

        public async Task<IdentityResult> CreateMinimunUser(string userName, string password)
        {
            var newUser = new AppUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = userName,
                Email = userName,
            };
            var identityResult =  await userManager.CreateAsync(newUser, password);

            if (identityResult.Succeeded)
            {
                await userManager.AddToRoleAsync(newUser, AppProfileConstants.USER);
            }
                return identityResult;
            
        }

        
        public async Task<string> GenerateToken(string userName)
        {
            var userIdentity = await userManager.FindByNameAsync(userName);
            var claims = new List<Claim>() {
                          new Claim(JwtRegisteredClaimNames.NameId, userIdentity.Id),
                          new Claim(JwtRegisteredClaimNames.UniqueName, userName)
                        };


            var roles = await userManager.GetRolesAsync(userIdentity);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            ClaimsIdentity identity = new ClaimsIdentity(
                claims
            );

            DateTime dataCriacao = DateTime.Now;
            DateTime dataExpiracao = dataCriacao +
                TimeSpan.FromSeconds(tokenConfigurations.Seconds);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
            {
                Issuer = tokenConfigurations.Issuer,
                Audience = tokenConfigurations.Audience,
                SigningCredentials = signingConfigurations.SigningCredentials,
                Subject = identity,
                NotBefore = dataCriacao,
                Expires = dataExpiracao

            });
            return handler.WriteToken(securityToken);
        }

        public async Task<AppUser> GetUser(string email)
        {
            return await userManager.FindByNameAsync(email);
        }

        public async Task<AppUser> GetUserById(string userId)
        {
            return await userManager.FindByIdAsync(userId);
        }

        public async Task<bool> UserExists(string userName)
        {
            var user = await userManager.FindByNameAsync(userName);
            return user != null;
        }


    }
}
