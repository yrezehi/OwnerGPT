using System.DirectoryServices.Protocols;
using System.Runtime.InteropServices;

namespace OwnerGPT.Core.Authentication
{
    public class ADAuthentication
    {
        private readonly string LDAP_HOST;
        private readonly string LDAP_PORT;
        
        private readonly string LDAP_DOMAIN;
        private readonly string LDAP_SUB_DOMAIN;

        private readonly string LDAP_USER;
        private readonly string LDAP_PASSWORD;

        private readonly LdapConnection Connection;

        public ADAuthentication()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                throw new PlatformNotSupportedException("Only windows is supported at the moment for active directory");
            }
        }
    }
}
