using OwnerGPT.Models.Abstracts.DTO;

namespace OwnerGPT.Core.Authentication.Abstracts
{
    public interface IAuthentication {
        bool IsAuthenticated(CredentialsDTO credentials);
    }
}
