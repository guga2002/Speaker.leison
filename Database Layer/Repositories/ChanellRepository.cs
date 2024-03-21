
using DatabaseOperations.Interfaces;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Speaker.leison.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Repositories
{
    public class ChanellRepository : BaseRepository,IChanellRepository
    {
        private readonly System.Data.Entity.DbSet<Chanell> chanells;
        public ChanellRepository() : base()
        {
            chanells = this.database.Set<Chanell>();
        }

        public Chanell GetByID(int id)
        {
           return chanells.Where(io => io.Id == id).FirstOrDefault();
        }
        public void Add(Chanell item)
        {
            if (!chanells.Any(io => io.Name==item.Name&&io.PortIn250==item.PortIn250))
            {
               chanells.Add(item);
               database.SaveChanges();
            }
            // aseti  info ukve aris bazashi
        }

        public Chanell GetChanellByPort(int port)
        {
            if (chanells.Any(io => io.PortIn250 == port))
            {
                var res = chanells.Where(io => io.PortIn250 == port).FirstOrDefault();
                if (res == null)
                {
                    return new Chanell();
                }
                return res;
            }
             Console.Out.WriteLineAsync(  "Arxi ara damatebuli");
            return null;
        }

        public void Remove(int id)
        {
            var res =  chanells.Where(io => io.PortIn250 == id).FirstOrDefault();
            if (res != null)
            {
                chanells.Remove(res);
                 database.SaveChanges();
            }
        }

        public void View(int id)
        {
            var res = chanells.Where(io => io.PortIn250 == id).FirstOrDefault();

            if (res != null)
            {
                MessageBox.Show(res.Name.ToString());

            }
            else
            {
                MessageBox.Show("bazashi info ar aris");
            }
        }
    }
}
