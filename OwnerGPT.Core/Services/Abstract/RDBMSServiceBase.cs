using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using OwnerGPT.DB.Repositores.RDBMS.Abstracts.Interfaces;
using OwnerGPT.Models.Entities.DTO;
using OwnerGPT.Models.Entities.Interfaces;
using OwnerGPT.Core.Services.Abstract.Interfaces;
using OwnerGPT.Core.Utilities;
using OwnerGPT.Core.Utilities.Extenstions;
using OwnerGPT.Models.Entities.Agents;

namespace OwnerGPT.Core.Services.Abstract
{
    public class RDBMSServiceBase<T> : IRDBMSCRUDService<T> where T : class
    {
        public RDBMSServiceBase(IRDBMSUnitOfWork unitOfWork)
        { 
            DBSet = unitOfWork.Repository<T>().DBSet;
            UnitOfWork = unitOfWork;
        }

        protected internal IRDBMSUnitOfWork UnitOfWork { get; set; }
        protected DbSet<T> DBSet { get; set; }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> expression) =>
            DBSet.Where(expression);

        public virtual IQueryable<T> OrderBy<TValue>(Expression<Func<T, TValue>> orderByExpression) =>
            DBSet.OrderBy(orderByExpression);

        public virtual async Task<IEnumerable<T>> GetAll() =>
            await DBSet.ToListAsync();

        public virtual async Task<PaginateDTO<T>> Paginate(int currentPage, Expression<Func<T, bool>>? expression)
        {
            var items = DBSet.ConditionalWhere(expression != null, expression!).Skip(currentPage * 10);
            var itemsCount = await DBSet.ConditionalCount(expression!);

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
            var entity = await DBSet.FindAsync(id);

            if (entity == null)
                throw new Exception("Entity Not Found");

            return entity;
        }

        public virtual async Task<T> Delete(int id)
        {
            var targetEntitiy = await DBSet.FindAsync(id);

            if (targetEntitiy != null)
            {
                DBSet.Remove(targetEntitiy);

                return targetEntitiy;
            }

            throw new Exception(nameof(IEntity));
        }

        public async Task<T> Update(IEntity entityToUpdate)
        {
            T entity = await DBSet.FirstOrDefaultAsync(entity => ((IEntity)entity).Id == entityToUpdate.Id);

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

                DBSet.Update(entity);

                return entity;
            }

            throw new Exception(nameof(T));
        }

        public async Task<T> Update(T entityToUpdate)
        {
            if (await DBSet.AnyAsync(entity => ((IEntity)entity).Id == ((IEntity)entityToUpdate).Id))
            {
                DBSet.Update(entityToUpdate);

                return entityToUpdate;
            }
            throw new Exception(nameof(T));
        }

        public async Task<T> Create(T entity)
        {
            await DBSet.AddAsync(entity);

            await UnitOfWork.CompletedAsync();

            return entity;
        }

    }
}
