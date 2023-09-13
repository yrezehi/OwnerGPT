using OwnerGPT.Core.Services.Abstract;
using OwnerGPT.DB.Repositores.RDBMS.Abstracts.Interfaces;
using OwnerGPT.Models;
using OwnerGPT.Models.Entities.Agents;

namespace OwnerGPT.Core.Services
{
    public class AcountService : RDBMSServiceBase<Account>
    {
        public AcountService(IRDBMSUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
