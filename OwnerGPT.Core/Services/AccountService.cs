using OwnerGPT.Core.Services.Abstract;
using OwnerGPT.DB.Repositores.RDBMS.Abstracts.Interfaces;
using OwnerGPT.Models.Entities;
using OwnerGPT.Models.Entities.Agents;

namespace OwnerGPT.Core.Services
{
    public class AccountService : RDBMSServiceBase<Account>
    {
        public AccountService(IRDBMSUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
