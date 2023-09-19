using OwnerGPT.Core.Services.Abstract;
using OwnerGPT.Core.Services.Compositions;
using OwnerGPT.DB.Repositores.RDBMS.Abstracts.Interfaces;
using OwnerGPT.DocumentEmbedding.Encoder;
using OwnerGPT.Models.Entities;
using OwnerGPT.Models.Entities.Agents;

namespace OwnerGPT.Core.Services
{
    public class AccountService : CompositionBaseService<Account>
    {

        public AccountService(RDBMSServiceBase<Account> RDBMSServiceBase, PGVServiceBase<Account> PGVServiceBase) : base(RDBMSServiceBase, PGVServiceBase) { }
    }
}