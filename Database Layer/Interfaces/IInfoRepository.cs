
using Interfaces;
using Speaker.leison.Entities;
using System.Threading.Tasks;

namespace DatabaseOperations.Interfaces
{
    public interface IInfoRepository:BaseInterface<Info>
    {
        Info GeTInfoByCHanellID(int id);
    }
}
