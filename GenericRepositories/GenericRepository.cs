namespace RESTJwt.GenericRepositories
{
    using RESTJwt.Data;
    using RESTJwt.GenericContracts;
    using Microsoft.EntityFrameworkCore;
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private ApplicationDbContext _dbContext;
        private DbSet<T> entities;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
            this.entities = this._dbContext.Set<T>();
        }

        public virtual T Create(T entity)
        {
            this.entities.Add(entity);
            this._dbContext.SaveChanges();

            return entity;
        }

        public virtual T Delete(T entity)
        {
            this.entities.Remove(entity);
            this._dbContext.SaveChanges();

            return entity;
        }

        public virtual T Update(T entity)
        {
            this.entities.Update(entity);
            this._dbContext.SaveChanges();

            return entity;
        }

        public virtual T FirstOrDefault(Func<T, bool> criteria)
        {
            return this.entities.FirstOrDefault(criteria);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return this.entities.AsEnumerable();
        }
    }
}
