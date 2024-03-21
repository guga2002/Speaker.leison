
using DatabaseOperations.Interfaces;
using Microsoft.EntityFrameworkCore;
using Speaker.leison.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Repositories
{
    public class InfoRepository : BaseRepository,IInfoRepository
    {
        private readonly System.Data.Entity.DbSet<Info> Infos;
        public InfoRepository() : base()
        {
            Infos = database.Set<Info>();
        }

        public void Add(Info item)
        {
            if (!Infos.Any(io => io.CHanellId == item.CHanellId))
            {
                Infos.Add(item);
               database.SaveChanges();
            }
        }

        public Info GeTInfoByCHanellID(int id)
        {
            if (Infos.Any(io => io.CHanellId == id))
            {
                var res =  Infos.Where(io => io.CHanellId == id).FirstOrDefault();
                return res;
            }
            Console.Out.WriteLineAsync(  "info ara gansazgvruli");
            return null;
        }

        public void Remove(int id)
        {
            var res = Infos.Where(io => io.CHanellId == id).FirstOrDefault();
            if (res != null)
            {
                Infos.Remove(res);
                database.SaveChanges();
            }
        }

        public void View(int id)
        {
            var res =  Infos.Where(io => io.CHanellId == id).FirstOrDefault();

            if (res != null)
            {
                MessageBox.Show(res.ToString());
            }
        }
    }
}
