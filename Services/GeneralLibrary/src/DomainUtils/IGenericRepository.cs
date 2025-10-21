using DomainBase;
using DomainErrors;
using LanguageExt;


namespace Domain.Interfaces
{

    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<Either<GeneralFailure, int>> AddRangeAsync(List<T> entity, CancellationToken cancellationToken);
        Task<Either<GeneralFailure, int>> UpdateAsync(T entity, CancellationToken cancellationToken);
        Task<Either<GeneralFailure, int>> DeleteAsync(T entity, CancellationToken cancellationToken);
        Task<Either<GeneralFailure, List<T>>> GetAllAsync(System.Linq.Expressions.Expression<Func<T, bool>>? expression = null, List<string>? includes = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, int? numbersOfItemsToTake = null, CancellationToken cancellationToken = default);  
        Task<Either<GeneralFailure, T>> GetMatch(System.Linq.Expressions.Expression<Func<T, bool>> expression, List<string>? includes = null, CancellationToken cancellationToken = default);
        Task<Either<GeneralFailure, T>> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = default);
        Task<Either<GeneralFailure, int>> DeleteByGuidAsync(Guid guid, CancellationToken cancellationToken = default);
        Task<Either<GeneralFailure, int>> DeleteByQueryAsync(System.Linq.Expressions.Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
        Task<Either<GeneralFailure, int>> ExecuteQueryAsync(string query, Dictionary<string, object>? parameters, CancellationToken cancellationToken);
        Task<Either<GeneralFailure, int>> GetMaxValueAsync(System.Linq.Expressions.Expression<Func<T, int>> selector, CancellationToken cancellationToken = default);
        Task<Either<GeneralFailure, int>> AddAsync(T entity, CancellationToken cancellationToken);
        Task<Either<GeneralFailure, int>> RemoveRangeByExpressionAsync(System.Linq.Expressions.Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
    }
}