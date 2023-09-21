using Microsoft.AspNetCore.Authentication.Cookies;
using OwnerGPT.Core.Authentication;
using OwnerGPT.Core.Services.Abstract;
using OwnerGPT.Core.Services.Compositions;
using OwnerGPT.DB.Repositores.RDBMS.Abstracts.Interfaces;
using OwnerGPT.DocumentEmbedding.Encoder;
using OwnerGPT.Models.Entities;
using OwnerGPT.Models.Entities.Agents;
using OwnerGPT.Models.Entities.DTO;
using System.Security.Claims;
using System.Security.Principal;

namespace OwnerGPT.Core.Services
{
    public class AccountService : CompositionBaseService<Account>
    {

        private readonly ADAuthentication ADAuthentication;

        public AccountService(ADAuthentication adAuthentication, RDBMSServiceBase<Account> RDBMSServiceBase, PGVServiceBase<VectorEmbedding> PGVServiceBase)
            : base(RDBMSServiceBase, PGVServiceBase) 
        {
            ADAuthentication = adAuthentication;
        }

        public void SignIn(CredentialsDTO credentials)
        {
            if (!ADAuthentication.Authenticate(credentials.Identifier, credentials.Password))
                throw new Exception("Invalid authentication attempt!");

            CookieAuthenticationSignIn();
        }

        private void CookieAuthenticationSignIn()
        {

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