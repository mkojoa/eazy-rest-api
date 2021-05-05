using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace eazy.rest.extension.EfCore.Helper
{
    public static class DataExtensions
    {
        public static Task ReplaceOneAsync<TModel>(this DbSet<TModel> entities,
            Expression<Func<TModel, bool>> predicate, TModel dto)
            where TModel : class
        {
            var records = entities
                .Where(predicate)
                .SingleAsync();
            if (records != null)
                entities.UpdateRange(dto);

            return Task.CompletedTask;
        }

        public static Task DeleteOneAsync<TModel>(this DbSet<TModel> entities,
            object dbSet,
            Expression<Func<TModel, bool>> predicate)
            where TModel : class
        {
            var records = entities
                .Where(predicate)
                .ToList();
            if (records.Count > 0)
                entities.RemoveRange(records);

            return Task.CompletedTask;
        }
    }
}