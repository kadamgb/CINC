using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace NQR.CINC.Entities.EntityFramework
{
    public partial class CINCEntities
    {
        const string _dbContextKey = "_dbcontext";
        int _transactionReferenceCount = 0;
        TransactionScope _transaction;

        /// <summary>
        /// The current open database connection
        /// </summary>
        public static CINCEntities Current
        {
            get
            {
                //Try to get the database connection (dbcontext) from the AppContext dictionary
                var dbContext = AppContext.GetItem(_dbContextKey) as CINCEntities;

                //If not present in the dictionary
                if (dbContext == null)
                {
                    //create a new connection
                    dbContext = new CINCEntities();

                    //add the connection to the AppContext dictionary so that the same connection can be reused by different repositories for the entire http request
                    AppContext.AddItem(_dbContextKey, dbContext);
                }

                return dbContext;
            }
        }

        public static void DisposeCurrent()
        {
            //get the dbContext from AppContext
            var dbContext = AppContext.GetItem(_dbContextKey) as CINCEntities;

            if (dbContext != null)
            {
                //check if a transaction is currently active
                if (dbContext.IsInTransaction)
                {
                    //Dispose off the transaction
                    dbContext._transaction.Dispose();
                }

                //TODO : to be uncommented once source code is integrated in project
                //dispose the connection
                dbContext.Dispose();
            }
        }

        public bool IsInTransaction
        {
            get
            {
                return _transactionReferenceCount > 0;
            }

        }

        /// <summary>
        /// Start a new distributed transaction
        /// </summary>
        /// <returns></returns>
        public TransactionScope BeginTransaction()
        {
            if (!IsInTransaction)
            {
                createTransactionScope();
            }

            _transactionReferenceCount++;

            return _transaction;
        }

        private void createTransactionScope()
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = IsolationLevel.ReadCommitted;
            _transaction = new TransactionScope(TransactionScopeOption.Required, options);
        }

        /// <summary>
        /// Rolls back the distributed transaction
        /// </summary>
        public void DisposeTransaction()
        {
            if (_transactionReferenceCount == 1)
            {
                _transaction.Dispose();
            }

            _transactionReferenceCount--;
        }


        /// <summary>
        /// Commits the distributed transaction
        /// </summary>
        public void CommitTransaction()
        {
            if (_transactionReferenceCount == 1)
            {
                _transaction.Complete();
            }
        }
    }
}
