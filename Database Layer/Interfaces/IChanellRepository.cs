
using Speaker.leison.Entities;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IChanellRepository:BaseInterface<Chanell>
    {
       Chanell GetChanellByPort(int port);
        Chanell GetByID(int id);
    }
}
