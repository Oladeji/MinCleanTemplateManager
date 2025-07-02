using DomainErrors;
using LanguageExt;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace Domain.Interfaces
{


    public interface IAppDbTransaction : IDisposable
    {
        void Commit();
        void Rollback();
        // Optionally, expose the underlying IDbTransaction if needed
        IDbTransaction InnerTransaction { get; }
    }
    public interface IUnitOfWork : IDisposable
    {
        Task<Either<GeneralFailure, int>> CommitAllChanges(CancellationToken cancellationToken);
        IAppDbTransaction BeginTransaction(IsolationLevel? isolationLevel = IsolationLevel.ReadCommitted);
        //IAppDbTransaction BeginTransaction();
    }
    public class AppDbTransaction(IDbContextTransaction efTransaction) : IAppDbTransaction
    {
        private readonly IDbContextTransaction _efTransaction= efTransaction;


        public void Commit() => _efTransaction.Commit();
        public void Rollback() => _efTransaction.Rollback();
        public void Dispose() => _efTransaction.Dispose();
        public IDbTransaction InnerTransaction => ((RelationalTransaction)_efTransaction).GetDbTransaction();

        // use case of the InnerTransaction transaction with EF Core and ADO.NET 

        //        using (var transaction = context.Database.BeginTransaction())
        //{
        //    var dbTransaction = transaction.GetDbTransaction(); // This is IDbTransaction

        //    // Use dbTransaction with ADO.NET
        //    var command = context.Database.GetDbConnection().CreateCommand();
        //    command.Transaction = dbTransaction;
        //    command.CommandText = "UPDATE ...";
        //    command.ExecuteNonQuery();

        //    // You can also use EF Core operations here
        //    context.SaveChanges();

        //    transaction.Commit();
        //}
    }
}