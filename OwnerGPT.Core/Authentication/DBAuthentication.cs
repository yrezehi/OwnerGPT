using OwnerGPT.Core.Services.Abstract;
using OwnerGPT.Databases.Repositores.RDBMS.Abstracts.Interfaces;
using OwnerGPT.Models;

namespace OwnerGPT.Core.Authentication
{
    public class DBAuthentication : RDBMSServiceBase<Account>
    {
        public DBAuthentication(IRDBMSUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
