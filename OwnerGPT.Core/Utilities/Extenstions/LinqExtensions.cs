using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using OwnerGPT.Models.Abstracts.Interfaces;

namespace OwnerGPT.Core.Utilities.Extenstions
{
    public static class LinqExtensions
    {
        public static IQueryable<IEntity> ConditionalWhere<IEntity>(this DbSet<IEntity> source, bool condition, Expression<Func<IEntity, bool>> predicate) where IEntity : class =>
            condition ? source.Where(predicate) : source;

        public static async Task<int> ConditionalCount<IEntity>(this DbSet<IEntity> source, Expression<Func<IEntity, bool>> predicate) where IEntity : class =>
            predicate != null ? await source.Where(predicate).AsQueryable().CountAsync() : await source.CountAsync();

        public static IQueryable<IEntity> PaginateQuerable<IEntity>(this IQueryable<IEntity> source, int page, int size) where IEntity : class =>
            source.Skip(page).Take(size);

        // includes first level of properties
        public static IQueryable<IEntity> IncludeSurface<IEntity>(this IQueryable<IEntity> source, bool includeSurface = false) where IEntity : class
        {
            if (includeSurface)
            {
                foreach (var property in ReflectionUtil.GetObjectProperties(typeof(IEntity)))
                {
                    var publicAccessor = property.GetGetMethod();

                    if (publicAccessor != null && publicAccessor.IsVirtual)
                    {
                        source = source.Include(property.Name);
                    }
                }
            }
            return source;
        }
    }
}
