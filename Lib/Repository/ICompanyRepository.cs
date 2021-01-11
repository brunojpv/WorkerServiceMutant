using Lib.Models;
using System;

namespace Lib.Repository
{
    public interface ICompanyRepository : IDisposable
    {
        public int GetId();

        public Company ConsultarCompanhia(string name);

        public void InserirCompanhia(Company adress);
    }
}
