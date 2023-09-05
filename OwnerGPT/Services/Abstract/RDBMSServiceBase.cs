using Microsoft.EntityFrameworkCore;
using OwnerGPT.Models.Interfaces;
using OwnerGPT.Utilities;
using System.Linq.Expressions;
using OwnerGPT.Services.Abstract.Interfaces;
using OwnerGPT.Repositores.RDBMS.Abstracts.Interfaces;
using OwnerGPT.Utilities.Extenstions;
using OwnerGPT.Models.DTO;

namespace OwnerGPT.Services.Abstract
{
    public class RDBMSServiceBase<T> : IRDBMSCRUDService<T> where T : class
    {
        public RDBMSServiceBase(IRDBMSUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        protected internal IRDBMSUnitOfWork UnitOfWork { get; set; }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> expression) =>
            UnitOfWork.Repository<T>().DBSet.Where(expression);

        public virtual IQueryable<T> OrderBy<TValue>(Expression<Func<T, TValue>> orderByExpression) =>
            UnitOfWork.Repository<T>().DBSet.OrderBy(orderByExpression);

        public virtual async Task<IEnumerable<T>> GetAll() =>
            await UnitOfWork.Repository<T>().DBSet.ToListAsync();

        public virtual async Task<PaginateDTO<T>> Paginate(int currentPage, Expression<Func<T, bool>>? expression)
        {
            var items = UnitOfWork.Repository<T>().DBSet.ConditionalWhere(expression != null, expression!).Skip(currentPage * 10);
            var itemsCount = await UnitOfWork.Repository<T>().DBSet.ConditionalCount(expression!);

            return new PaginateDTO<T>()
            {
                Items = items,
                Page = currentPage,
                Total = itemsCount,
                Pages = (int)Math.Ceiling((double)itemsCount / 10),
            };
        }

        public virtual async Task<T> FindById(int id)
        {
            var entity = await UnitOfWork.Repository<T>().DBSet.FindAsync(id);

            if (entity == null)
                throw new Exception("Entity Not Found");

            return entity;
        }

        public virtual async Task<T> Delete(int id)
        {
            var targetEntitiy = await UnitOfWork.Repository<T>().DBSet.FindAsync(id);

            if (targetEntitiy != null)
            {
                UnitOfWork.Repository<T>().DBSet.Remove(targetEntitiy);

                return targetEntitiy;
            }

            throw new Exception(nameof(IEntity));
        }

        public async Task<T> Update(IEntity entityToUpdate)
        {
            T entity = await UnitOfWork.Repository<T>().DBSet.FirstOrDefaultAsync(entity => ((IEntity) entity).Id == ((IEntity)entityToUpdate).Id);

            if (entity != null)
            {
                var dtoProperties = ReflectionUtil.GetInterfacedObjectProperties(entityToUpdate.GetType());

                foreach (var property in dtoProperties)
                {
                    var dtoPropertyValue = ReflectionUtil.GetValueOf(entityToUpdate, property.Name);

                    if (dtoPropertyValue != null)
                    {
                        if (ReflectionUtil.ContainsProperty(entity, property.Name))
                            ReflectionUtil.SetValueOf(entity, property.Name, dtoPropertyValue);
                    }
                }

                UnitOfWork.Repository<T>().DBSet.Update(entity);

                return entity;
            }

            throw new Exception(nameof(T));
        }

        public async Task<T> Update(T entityToUpdate)
        {
            if (await UnitOfWork.Repository<IEntity>().DBSet.AnyAsync(entity => entity.Id == ((IEntity)entityToUpdate).Id))
            {
                UnitOfWork.Repository<T>().DBSet.Update(entityToUpdate);

                return entityToUpdate;
            }
            throw new Exception(nameof(T));
        }

        public async Task<T> Create(T entity)
        {
            await UnitOfWork.Repository<T>().DBSet.AddAsync(entity);

            await UnitOfWork.CompletedAsync();

            return entity;
        }

    }
}
