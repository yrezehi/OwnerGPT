using OwnerGPT.Core.Services.Abstract;
using OwnerGPT.Core.Services.Abstract.Interfaces;

namespace OwnerGPT.Core.Services.Compositions
{
    public class RDBMSPGVBaseService<T> where T : class
    {
        public readonly RDBMSServiceBase<T> RDBMSBaseService;
        public readonly PGVServiceBase<T> PGVServiceBaseSerivce;

        public RDBMSPGVBaseService(RDBMSServiceBase<T> RDBMSBaseService, PGVServiceBase<T> PGVServiceBase)
        {

        }
    }
}
