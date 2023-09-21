using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using OwnerGPT.Core.Authentication;
using OwnerGPT.Core.Services.Abstract;
using OwnerGPT.Core.Services.Compositions;
using OwnerGPT.DB.Repositores.RDBMS.Abstracts.Interfaces;
using OwnerGPT.Models.Entities;
using OwnerGPT.Models.Entities.Agents;
using OwnerGPT.Models.Entities.DTO;
using System.Security.Claims;

namespace OwnerGPT.Core.Services
{
    public class AccountService : CompositionBaseService<Account>
    {

        private readonly ADAuthentication ADAuthentication;
        private readonly IHttpContextAccessor HttpContextAccessor;

        public AccountService(IHttpContextAccessor httpContextAccessor, ADAuthentication adAuthentication, RDBMSServiceBase<Account> RDBMSServiceBase, PGVServiceBase<VectorEmbedding> PGVServiceBase)
            : base(RDBMSServiceBase, PGVServiceBase) 
        {
            ADAuthentication = adAuthentication;
            HttpContextAccessor = httpContextAccessor;
        }

        public async Task<Account> SignIn(CredentialsDTO credentials)
        {
            if (!ADAuthentication.Authenticate(credentials.Identifier, credentials.Password))
                throw new Exception("Invalid authentication attempt!");

            var account = await this.RDBMSServiceBase.FindByProperty<string>(entity => entity.Email!, credentials.Identifier);
            
            CookieAuthenticationSignIn(account);

            return account;
        }

        private async void CookieAuthenticationSignIn(Account account)
        {
            await HttpContextAccessor.HttpContext.SignInAsync(this.GenerateClaimsPrincipal(account));
        }

        private ClaimsPrincipal GenerateClaimsPrincipal(Account account)
        {
            return new ClaimsPrincipal(this.GenerateClaimsIdentity(account));
        }

        private ClaimsIdentity GenerateClaimsIdentity(Account account)
        {
            return new ClaimsIdentity(this.GenerateClaims(account), CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private List<Claim> GenerateClaims(Account account)
        {
            return new List<Claim>()
            {
                new Claim(ClaimTypes.Email, account.Email!),
            };
        }

    }
}