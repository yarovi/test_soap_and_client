using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace WS.Unit06.User.Data.DAO
{
    public class GenericDAO<T> where T: class
    {
        private DataContext Context { get; set; }

        public GenericDAO(DataContext context)
        {
            this.Context = context;
        }

        protected DbSet<T> DbSet
        {
            get { return Context.Set<T>(); }
        }

        public virtual bool Add(T t)
        {
            DbSet.Add(t);
            return Context.SaveChanges() != 0;            
        }

        public virtual bool Remove(T t)
        {
            DbSet.Remove(t);
            return Context.SaveChanges() != 0;
        }

        public virtual bool Update(T t)
        {
            DbSet.Attach(t);
            Context.Entry(t).State = EntityState.Modified;
            return Context.SaveChanges() != 0;
        }

        public virtual T Find(int id)
        {
            return DbSet.Find(id);            
        }

        public virtual IEnumerable<T> All()
        {
            return DbSet.ToList();
        }

        public virtual int Count()
        {
            return DbSet.Count();
        }
    }
}
