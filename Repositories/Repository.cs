using DirectorAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace DirectorAPI.Repositories
{
    public class Repository<T> where T:class
    {
        private Sistem21PrimariaContext context;
        public Repository(Sistem21PrimariaContext Context)
        {
            this.context=Context;
        }

        public DbSet<T> Get() //Obtiene todos los elementos.
        {
            return context.Set<T>();
        }

        public T? Get(object v) //Busca uno en especifico.
        {
            return context.Find<T>(v);
        }

        public void Insert(T entidad) //Inserta uno.
        {
            context.Add(entidad);
            context.SaveChanges();
        }

        public void Update(T entidad) //Actualiza uno.
        {
            context.Update(entidad);
            context.SaveChanges();
        }

        public void Delete(T entidad) //Elimina uno.
        {
            context.Remove(entidad);
            context.SaveChanges();
        }

    }
}
