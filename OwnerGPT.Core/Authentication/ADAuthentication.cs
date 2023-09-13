using OwnerGPT.Core.Authentication.Abstracts;

namespace OwnerGPT.Core.Authentication
{
    public class ADAuthentication : IAuthentication
    {
        public ADAuthentication() { }

        public bool Authenticate() { return false; }
    }
}
