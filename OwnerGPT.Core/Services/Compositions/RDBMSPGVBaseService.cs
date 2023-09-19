using OwnerGPT.Core.Services.Abstract;
using OwnerGPT.Core.Services.Abstract.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnerGPT.Core.Services.Compositions
{
    public class RDBMSPGVBaseService<T> where T : class
    {
        public readonly RDBMSServiceBase<T> RDBMSBaseService;
        public readonly PGVServiceBase<T> PGVServiceBaseSerivce;

        public RDBMSPGVBaseService()
        {

        }
    }
}
