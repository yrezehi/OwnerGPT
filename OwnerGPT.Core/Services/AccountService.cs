﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using OwnerGPT.Core.Authentication;
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
            if (!ADAuthentication.Authenticate(credentials.Identifier, credentials.Password))
                throw new ArgumentException("Invalid authentication attempt!");

            var account = await this.RDBMSServiceBase.FindByProperty(entity => entity.Email!, credentials.Identifier);
        
            return account;
        }

        private async Task CookieAuthenticationSignIn(Account account) =>
            await HttpContextAccessor.HttpContext.SignInAsync(this.GenerateClaimsPrincipal(account));

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