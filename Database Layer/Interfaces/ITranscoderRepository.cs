using Interfaces;
using Speaker.leison.Entities;
using System.Threading.Tasks;

namespace DatabaseOperations.Interfaces
{
    public interface ITranscoderRepository : BaseInterface<Transcoder>
    {
        Transcoder GetTranscoderInfoByCHanellId(int id);
        int GetChanellIdBycardandport(int card, int port);
    }
}
