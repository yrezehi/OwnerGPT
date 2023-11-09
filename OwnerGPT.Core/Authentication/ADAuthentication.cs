using OwnerGPT.Core.Authentication.Abstracts;
using OwnerGPT.Models.Abstracts.DTO;
using System.DirectoryServices.AccountManagement;
using System.Runtime.InteropServices;

namespace OwnerGPT.Core.Authentication
{
    public class ADAuthentication : IAuthentication
    {
        private readonly PrincipalContext LDAPContext;
        private readonly string LDAP_DOMAIN;

        public ADAuthentication()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                throw new PlatformNotSupportedException("Only windows is supported at the moment for active directory");
            }

            //LDAP_DOMAIN = ConfigurationUtil.GetValue<string>("LDAP_DOMAIN");

            //LDAPContext = new PrincipalContext(ContextType.Domain, LDAP_DOMAIN);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        public bool IsAuthenticated(CredentialsDTO credentials) =>
            true; // LDAPContext.ValidateCredentials(email, password);

    }
}
