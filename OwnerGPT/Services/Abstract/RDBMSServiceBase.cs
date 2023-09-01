using Microsoft.EntityFrameworkCore;
using OwnerGPT.Models.DTO.Interfaces;
using OwnerGPT.Models.DTO;
using OwnerGPT.Models.Interfaces;
using OwnerGPT.Utilities;
using System.Linq.Expressions;
using OwnerGPT.Services.Abstract.Interfaces;
using OwnerGPT.Repositores.RDBMS.Abstracts.Interfaces;
using OwnerGPT.Utilities.Extenstions;

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

        public async Task<T> Update(T entityToUpdate, IDTO entityDTO)
        {
            if (await UnitOfWork.Repository<IEntity>().DBSet.AnyAsync(entity => entity.Id == ((IEntity)entityToUpdate).Id))
            {
                var dtoProperties = ReflectionUtil.GetInterfacedObjectProperties(typeof(IDTO));

                foreach (var property in dtoProperties)
                {
                    var dtoPropertyValue = ReflectionUtil.GetValueOf(entityDTO, property.Name);

                    if (dtoPropertyValue != null)
                    {
                        if (ReflectionUtil.ContainsProperty(entityToUpdate, property.Name))
                            ReflectionUtil.SetValueOf(entityDTO, property.Name, dtoPropertyValue);
                    }
                }

                UnitOfWork.Repository<T>().DBSet.Update(entityToUpdate);

                return entityToUpdate;
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

        public async Task<T> Insert(IDTO entityDTO)
        {
            T entity = ReflectionUtil.MapEntity<T>(entityDTO);

            await UnitOfWork.Repository<T>().DBSet.AddAsync(entity);

            await UnitOfWork.CompletedAsync();

            return entity;
        }

    }
}
}
