using Lib.Models;
using System;

namespace Lib.Repository
{
    public interface IAdressRepository : IDisposable
    {
        public int GetId();

        public Address ConsultarEndereco(string zipcode);

        public void InserirEndereco(Address adress);
    }
}
