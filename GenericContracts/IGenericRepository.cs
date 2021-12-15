namespace RESTJwt.GenericContracts
{
    public interface IGenericRepository<T>
    {
        public T FirstOrDefault(Func<T, bool> criteria);
        public IEnumerable<T> GetAll();
        public T Delete(T entity);
        public T Create(T entity);
        public T Update(T entity);
    }
}
