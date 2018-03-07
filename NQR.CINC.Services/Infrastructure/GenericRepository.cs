using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace NQR.CINC.Services.Infrastructure
{
    public class GenericDBRepository<TEntity> : BaseRepository, IRepository<TEntity> where TEntity : class
    {
        internal DbContext _dbContext;
        internal DbSet<TEntity> _dbSet;

        //The constructor accepts a database context instance and initializes the entity set variable
        public GenericDBRepository(DbContext context)
        {
            this._dbContext = context;
            this._dbSet = context.Set<TEntity>();
        }

        /// <summary>
        /// The Get method uses lambda expressions to allow the calling code to specify a filter condition and a column to order the results by, and a string parameter lets the caller 
        /// provide a comma-delimited list of navigation properties for eager loading
        /// </summary>
        /// <param name="filter">A lambda expression based on the TEntity type, and this expression will return a Boolean value.</param>
        /// <param name="orderBy">A lambda expression that will return an ordered version of a IQueryable object</param>
        /// <param name="includeProperties">A set of properties to be returned</param>
        /// <returns>An IQueryable collection of Entity objects</returns>
        public virtual IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;

            //apply the filter if one if specified
            if (filter != null)
            {
                query = query.Where(filter);
            }

            //apply eager loading expression after parsing the comma delimited list of properties
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            //apply the orderby expression if one is specified
            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }

        /// <summary>
        /// Get a single record
        /// </summary>
        /// <param name="id">ID of the record to retrieve</param>
        /// <returns>A single entity instance</returns>
        public virtual TEntity GetByID(object id)
        {
            return _dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        /// <summary>
        /// Delete the object
        /// </summary>
        /// <param name="id">Id of the record to be deleted</param>
        public virtual void Delete(object id)
        {
            TEntity entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
        }

        /// <summary>
        /// Delete the object
        /// </summary>
        /// <param name="entityToDelete">Reference of the entity to be deleted</param>
        public virtual void Delete(TEntity entityToDelete)
        {
            if (_dbContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _dbContext.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual IEnumerable<TEntity> GetFromSQL(string strSQL)
        {
            return _dbContext.Database.SqlQuery<TEntity>(strSQL);
        }

        public TEntity Find(params object[] keyValues)
        {
            return _dbSet.Find(keyValues);
        }

        public IQueryable<TEntity> SelectQuery(string query, params object[] parameters)
        {
            return _dbSet.SqlQuery(query, parameters).AsQueryable();
        }

        public int ExecuteStoreProcedure(string strSQL, params object[] parameters)
        {
            return _dbContext.Database.ExecuteSqlCommand(strSQL, parameters);
        }
    }
}
