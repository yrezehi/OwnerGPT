using OwnerGPT.Core.Services.Abstract;
using OwnerGPT.Databases.Repositores.RDBMS.Abstracts.Interfaces;
using OwnerGPT.Models;
using OwnerGPT.Models.Abstracts.DTO;

namespace OwnerGPT.Core.Authentication
{
    public class DBAuthentication : RDBMSServiceBase<Account>
    {
        public DBAuthentication(IRDBMSUnitOfWork unitOfWork) : base(unitOfWork) { }

        public bool IsAuthenticated(CredentialsDTO credentialsDTO) =>
            this.Any(user => (user.Password == credentialsDTO.Password) && (user.Email == credentialsDTO.Identifier));
    }
}
