using Dapper;
using Lib.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Data;

namespace Lib.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ILogger<CompanyRepository> _logger;
        private readonly IDbConnection _conn;

        public CompanyRepository(ILogger<CompanyRepository> logger, IDbConnection conn)
        {
            _logger = logger;
            _conn = conn;

            if (_conn.State == ConnectionState.Closed)
                _conn.Open();
        }

        public int GetId()
        {
            try
            {
                int query = _conn.QuerySingle<Company>("SELECT ISNULL(MAX(ID), 0) + 1 AS Id FROM Company").Id;

                return query;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar o Id da companhia!!!");

                return -1;
            }
        }

        public Company ConsultarCompanhia(string name)
        {
            try
            {
                var query = _conn.QuerySingle<Company>(@"SELECT * FROM Company WHERE Name = @Name", new { Name = name });

                if (query.Id > 0)
                    return query;
                else
                    return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao inserir a companhia!!!");

                return null;
            }
        }

        public void InserirCompanhia(Company company)
        {
            try
            {
                var query = "Insert into Company(Name, CatchPhrase, Bs) values(@Name, @CatchPhrase, @Bs)";

                var result = _conn.Execute(query, new { Name = company.Name, CatchPhrase = company.CatchPhrase, Bs = company.Bs });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao salvar o endereço!!!");
            }
        }

        public void Dispose()
        {
            if (_conn.State == ConnectionState.Open)
                _conn.Close();

            GC.SuppressFinalize(this);
        }
    }
}
