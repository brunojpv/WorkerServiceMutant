using Lib.Models;
using System;

namespace Lib.Repository
{
    public interface IUsersRepository
    {
        public Users ConsultarUsuario(string userName);

        public void InserirUsuario(Users user);
    }
}
