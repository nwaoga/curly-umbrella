using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FootieWorld.data.ef.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public FootieDbEntities  Context { get; set; }


        public UnitOfWork (DbContext dbContext )
        {
            this.Context = (FootieDbEntities)dbContext;
        }

        public void SaveChanges()
        {
            //audit coming soon 

            this.Context.SaveChanges();
        }

        public void Dispose()
        {
            this.Context.Dispose();
        }

    }
}
