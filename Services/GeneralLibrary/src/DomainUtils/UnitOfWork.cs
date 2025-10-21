using Domain.Interfaces;
using DomainErrors;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Domain.DBContext
{
    public class UnitOfWork<T> : IUnitOfWork where T : DbContext
    {
        public readonly T _ctx;
        public UnitOfWork(T ctx) { _ctx = ctx; }

        public IAppDbTransaction BeginTransaction(IsolationLevel? isolationLevel = IsolationLevel.ReadCommitted)
        {
            var efTransaction = _ctx.Database.BeginTransaction(isolationLevel ?? IsolationLevel.ReadCommitted);
            return new AppDbTransaction(efTransaction);
        }

        public async Task<Either<GeneralFailure, int>> CommitAllChanges(CancellationToken cancellationToken)
        {
            try
            {
                return await _ctx.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException)
            {
                return GeneralFailures.ProblemAddingEntityIntoDbContext("Problem Saving Data");
            }
            catch (Exception ex)
            {
                return GeneralFailures.ExceptionThrown("GenericRepository-AddAsync", "Problem Saving Data", ex?.InnerException?.Message ?? string.Empty);
            }
        }

        public void Dispose() { _ctx?.Dispose(); GC.SuppressFinalize(this); }
    }
}