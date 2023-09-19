using OwnerGPT.Core.Services.Abstract;
using OwnerGPT.Core.Services.Abstract.Interfaces;

namespace OwnerGPT.Core.Services.Compositions
{
    public interface ICompositionBaseService<T> where T : class
    {
        RDBMSServiceBase<T> RDBMSServiceBase { get; set; }
        PGVServiceBase<T> PGVServiceBase { get; set; }
    }
}
