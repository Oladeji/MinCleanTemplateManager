using Domain.Interfaces;
using DomainBase;
using DomainErrors;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using System.Linq.Expressions;

namespace Domain.DBContext
{
    public class GenericRepository<T, R> : IGenericRepository<T> where T : BaseEntity where R : DbContext
    {
        private readonly R _ctx;
        private readonly ILogger<GenericRepository<T, R>> _logger;
        public GenericRepository(R ctx, ILogger<GenericRepository<T, R>> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }

        async Task<Either<GeneralFailure, int>> IGenericRepository<T>.AddAsync(T entity, CancellationToken cancellationToken)
        {
            try
            {
                var x = await _ctx.AddAsync<T>(entity, cancellationToken);
                return await _ctx.SaveChangesAsync(cancellationToken);
            }

            catch (MySqlException ex)
            {

                return GeneralFailures.ProblemAddingEntityIntoDbContext(entity.GuidId.ToString() + " " + ex.Message + " " + ex.InnerException?.ToString());
            }
            catch (DbUpdateException ex)
            {

                _logger.LogError($"{ex}, Problem adding entity with Guid {entity.GuidId}");
                if (ex.InnerException.Message.Contains("DUPLICATE", StringComparison.CurrentCultureIgnoreCase))
                {
                    return GeneralFailures.DuplicateEntity($"Data with Key {entity.GuidId} Already exist in database" );
                }
                return GeneralFailures.ProblemAddingEntityIntoDbContext(entity.GuidId.ToString() + " " + ex.Message + " " + ex.InnerException);
            }


            catch (Exception ex)
            {
                _logger.LogError($"{ex}, Problem adding entity with Guid {entity.GuidId}");

                return GeneralFailures.ProblemAddingEntityIntoDbContext(entity.GuidId.ToString() + " " + ex.Message + " " + ex.InnerException);
            }
        }

        async Task<Either<GeneralFailure, int>> IGenericRepository<T>.UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            try
            {
                _ctx.Update(entity);
                return await _ctx.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex}, Problem updating entity with Guid {entity.GuidId}");
                return GeneralFailures.ProblemUpdatingEntityInRepository(entity.GuidId.ToString() + " " + ex.Message + " " + ex.InnerException);
            }
        }

        async Task<Either<GeneralFailure, int>> IGenericRepository<T>.DeleteAsync(T entity, CancellationToken cancellationToken)
        {
            try
            {
                if (entity != null)
                {
                    _ctx.Remove(entity);
                    return await _ctx.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    _logger.LogWarning("Entity to delete is null");
                    return GeneralFailures.DataNotFoundInRepository(entity?.GuidId.ToString()?? "");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex}, Problem deleting entity with Guid {entity.GuidId}");
                return GeneralFailures.ProblemDeletingEntityFromRepository(entity.GuidId.ToString() + " " + ex.Message + " " + ex.InnerException);
            }
        }
        public async Task<Either<GeneralFailure, int>> DeleteByGuidAsync(Guid guid, CancellationToken cancellationToken = default)
        {

            try
            {
                var entity = await _ctx.Set<T>().AsNoTracking().FirstOrDefaultAsync(s => (s.GuidId.Equals(guid)), cancellationToken);
                if (entity != null)
                {

                    _ctx.Remove<T>(entity);
                    return await _ctx.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    _logger.LogWarning("Entity to delete is null");
                    return GeneralFailures.DataNotFoundInRepository(guid.ToString());


                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex}, Problem deleting entity with Guid {guid}");
                return GeneralFailures.ProblemDeletingEntityFromRepository(guid.ToString() + " " + ex.Message + " " + ex.InnerException);
            }
        }
        public async Task<Either<GeneralFailure, int>> DeleteByQueryAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Executing DeleteByQueryAsync with expression: {Expression}", expression);

                var entity = await _ctx.Set<T>().AsNoTracking().FirstOrDefaultAsync(expression, cancellationToken);
                if (entity != null)
                {
                    _logger.LogInformation("Entity found: {Entity}", entity);
                    _ctx.Remove<T>(entity);
                    var result = await _ctx.SaveChangesAsync(cancellationToken);
                    _logger.LogInformation("Entity deleted successfully. Result: {Result}", result);
                    return Prelude.Right<GeneralFailure, int>(result);
                }

                else
                {
                    _logger.LogWarning("No entity found matching the expression: {Expression}", expression);

                    return Prelude.Left<GeneralFailure, int>(GeneralFailures.DataNotFoundInRepository(expression.ToString()));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem deleting entity with expression: {Expression}", expression);
                return Prelude.Left<GeneralFailure, int>(GeneralFailures.ProblemDeletingEntityFromRepository(ex.Message));
            }
        }




        public async Task<Either<GeneralFailure, int>> RemoveRangeByExpressionAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)

        {
            try
            {

                var entities = await _ctx.Set<T>().Where(expression).ToListAsync(cancellationToken);

                if (entities.Any())
                {
                    _ctx.Set<T>().RemoveRange(entities);
                    return await _ctx.SaveChangesAsync(cancellationToken);
                }
                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex}, Problem deleting entity with expression");
                return GeneralFailures.ProblemDeletingEntityFromRepository(ex.Message);
            }

        }

        public async Task<Either<GeneralFailure, T>> GetByGuidAsync(Guid guid, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _ctx.Set<T>().AsNoTracking().SingleOrDefaultAsync(s => (s.GuidId.Equals(guid)), cancellationToken);
                return entity != null ? entity : GeneralFailures.DataNotFoundInRepository(guid.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex}, Problem retrieving entity with Guid {guid}");
                return GeneralFailures.ErrorRetrievingSingleDataFromRepository(guid.ToString() + ex.Message);
            }
        }


        public async Task<Either<GeneralFailure, T>> GetMatch(Expression<Func<T, bool>> expression, List<string>? includes = null, CancellationToken cancellationToken = default)
        {
            try
            {
                IQueryable<T> query = _ctx.Set<T>();
                if (includes is not null)
                {
                    foreach (var includeProperty in includes)
                    {
                        query = query.Include(includeProperty);
                    }
                }
                var entity = await query.AsNoTracking().FirstOrDefaultAsync(expression, cancellationToken);
                return entity != null ? entity : GeneralFailures.DataNotFoundInRepository("NOT FOUND");
            }
            catch (MySqlException ex)
            {
                _logger.LogError($"{ex}, Problem retrieving entity with expression");
                return GeneralFailures.ExceptionThrown("GenericRepo-Add", ex.Message, ex.StackTrace?? string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex}, Problem retrieving entity with expression");
                return GeneralFailures.ErrorRetrievingListDataFromRepository(ex.ToString());
            }
        }
        async Task<Either<GeneralFailure, List<T>>> IGenericRepository<T>.GetAllAsync(Expression<Func<T, bool>>? expression, List<string>? includes, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy, int? numbersOfItemsToTake, CancellationToken cancellationToken)
        {
            try
            {
                IQueryable<T> query = _ctx.Set<T>();
                if (expression is not null)
                {
                    query = query.Where(expression);
                }
                if (includes is not  null)
                {
                    foreach (var includeProperty in includes)
                    {
                        query = query.Include(includeProperty);
                    }
                }
                if (orderBy != null)
                {
                    query = orderBy(query);
                }


                var result = numbersOfItemsToTake != null ? await query.AsNoTracking().Take(numbersOfItemsToTake.Value).ToListAsync(cancellationToken) : await query.AsNoTracking().ToListAsync(cancellationToken);
                return result;
            }
            catch (MySqlException ex)
            {
                _logger.LogError($"{ex}, Problem retrieving list data from repository");
                return GeneralFailures.ExceptionThrown("GenericRepo-GetAllAsync", ex.Message, ex.StackTrace ??  string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex}, Problem retrieving list data from repository");
                return GeneralFailures.ErrorRetrievingListDataFromRepository(ex.ToString());
            }
        }

        //async Task<Either<GeneralFailure, List<T>>> IGenericRepository<T>.GetAllAsync(Expression<Func<T, bool>> expression, List<string> includes, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        IQueryable<T> query = _ctx.Set<T>();
        //        if (expression != null)
        //        {
        //            query = query.Where(expression);
        //        }
        //        if (includes != null)
        //        {
        //            foreach (var includeProperty in includes)
        //            {
        //                query = query.Include(includeProperty);
        //            }
        //        }
        //        if (orderBy != null)
        //        {
        //            query = orderBy(query);
        //        }
        //        var result = await query.AsNoTracking().ToListAsync(cancellationToken);
        //        return result;
        //    }
        //    catch (MySqlException ex)
        //    {
        //        _logger.LogError($"{ex}, Problem retrieving list data from repository");
        //        return GeneralFailures.ExceptionThrown("GenericRepo-GetAllAsync", ex.Message, ex.StackTrace);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"{ex}, Problem retrieving list data from repository");
        //        return GeneralFailures.ErrorRetrievingListDataFromRepository(ex.ToString());
        //    }
        //}


        public async Task<Either<GeneralFailure, int>> GetMaxValueAsync(Expression<Func<T, int>> selector, CancellationToken cancellationToken = default)
        {
            try
            {
                var maxValue = await _ctx.Set<T>().MaxAsync(selector, cancellationToken);
                return maxValue;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem retrieving max value");
                return GeneralFailures.ExceptionThrown("GenericRepository-ExecuteQuery", "Problem Executing Query", ex?.InnerException?.Message ?? string.Empty);
            }
        }

        public async Task<Either<GeneralFailure, int>> ExecuteQueryAsync(string query, Dictionary<string, object>? paramsDict, CancellationToken cancellationToken)
        {



            try
            {
                if (paramsDict == null)
                {
                    var result = await _ctx.Database.ExecuteSqlRawAsync(query, cancellationToken);
                    return result;
                }
                else
                {
                    var parameters = paramsDict.Select(kvp => new MySqlParameter(kvp.Key, kvp.Value)).ToArray();
                    await _ctx.Database.ExecuteSqlRawAsync(query, parameters, cancellationToken);
                    return await _ctx.SaveChangesAsync(cancellationToken);
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex}, Problem executing query");
                return GeneralFailures.ExceptionThrown("GenericRepository-ExecuteQuery", "Problem Executing Query", ex?.InnerException?.Message ?? string.Empty);
            }
        }

        public async Task<Either<GeneralFailure, int>> AddRangeAsync(List<T> entity, CancellationToken cancellationToken)
        {
            try
            {
                await _ctx.AddRangeAsync(entity, cancellationToken);
                return await _ctx.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"{ex}, Problem adding List to Repository ");
                return GeneralFailures.ProblemAddingEntityIntoDbContext(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex}, Problem adding List to Repository ");
                return GeneralFailures.ExceptionThrown("GenericRepository-AddAsync", "Problem Adding Entity with Guid", ex?.InnerException?.Message ?? string.Empty);
            }
        }


    }
}
