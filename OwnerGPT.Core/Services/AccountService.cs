using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using OwnerGPT.Core.Authentication;
using OwnerGPT.Core.Extensions;
using OwnerGPT.Core.Services.Abstract;
using OwnerGPT.Core.Services.Compositions;
using OwnerGPT.Models;
using OwnerGPT.Models.Abstracts.DTO;
using System.Security.Claims;

namespace OwnerGPT.Core.Services
{
    public class AccountService : CompositionBaseService<Account>
    {
        // Interface it with generic authentiucation as provider...
        private readonly ADAuthentication ADAuthentication;
        private readonly IHttpContextAccessor HttpContextAccessor;

        public AccountService(IHttpContextAccessor httpContextAccessor, ADAuthentication adAuthentication, RDBMSServiceBase<Account> RDBMSServiceBase, PGVServiceBase<VectorEmbedding> PGVServiceBase)
            : base(RDBMSServiceBase, PGVServiceBase) =>
            (ADAuthentication, HttpContextAccessor) = (adAuthentication, httpContextAccessor);

        public async Task<Account> SignIn(CredentialsDTO credentials)
        {
            if (!ADAuthentication.IsAuthenticated(credentials))
                throw new ArgumentException("Invalid authentication attempt!");

            return await this.RDBMSServiceBase.FindByProperty(entity => entity.Email!, credentials.Identifier)
                .Then(CookieAuthenticationSignIn).Then(account => account.Result);
        }

        private async Task<Account> CookieAuthenticationSignIn(Account account)
        {
            await HttpContextAccessor.HttpContext.SignInAsync(this.GenerateClaimsPrincipal(account));

            return account;
        }

        private ClaimsPrincipal GenerateClaimsPrincipal(Account account) =>
            new ClaimsPrincipal(this.GenerateClaimsIdentity(account));
     

        private ClaimsIdentity GenerateClaimsIdentity(Account account) =>
            new ClaimsIdentity(this.GenerateClaims(account), CookieAuthenticationDefaults.AuthenticationScheme);
        

        private List<Claim> GenerateClaims(Account account) => 
            new() {
                new Claim(ClaimTypes.Email, account.Email!),
            };

    }
}