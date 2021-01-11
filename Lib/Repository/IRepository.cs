using Lib.Models;

namespace Lib.Repository
{
    public interface IRepository
    {
        public void BaixarDados(Users user);

        public void SalvarDados(Users users);
    }
}
