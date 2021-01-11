using Lib.Models;

namespace Lib.Repository
{
    public interface IGeoRepository
    {
        public int GetId();

        public Geo ConsultarGeo(int id);

        public void InserirGeo(Geo geo);
    }
}