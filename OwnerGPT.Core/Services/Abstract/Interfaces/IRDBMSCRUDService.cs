using OwnerGPT.Models.Entities.DTO;
using System.Linq.Expressions;

namespace OwnerGPT.Core.Services.Abstract.Interfaces
{
    public interface IRDBMSCRUDService<T> where T : class
    {
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);

        Task<IEnumerable<T>> GetAll();

        Task<PaginateDTO<T>> Paginate(int currentPage, Expression<Func<T, bool>>? expression);

        Task<T> FindById(int id);

        Task<T?> NullableFindById(int id);

        Task<T> Delete(int id);

        Task<T> Update(T entityToUpdate);

        Task<T> Create(T entityDTO);
    }
}
