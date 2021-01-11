using Dapper;
using Lib.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Data;

namespace Lib.Repository
{
    public class AdressRepository : IAdressRepository
    {
        private readonly ILogger<AdressRepository> _logger;
        private readonly IDbConnection _conn;
        private IGeoRepository _geoRepository;

        public AdressRepository(ILogger<AdressRepository> logger, IDbConnection conn, IGeoRepository geoRepository)
        {
            _geoRepository = geoRepository;
            _logger = logger;
            _conn = conn;

            if (_conn.State == ConnectionState.Closed)
                _conn.Open();
        }

        public int GetId()
        {
            try
            {
                int query = _conn.QuerySingle<Address>("SELECT ISNULL(MAX(ID), 0) + 1 AS Id FROM Adress").Id;

                return query;                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar o Id do endereço!!!");

                return -1;
            }
        }

        public Address ConsultarEndereco(string zipcode)
        {
            try
            {
                var query = _conn.QuerySingle<Address>(@"SELECT * FROM Adress WHERE Zipcode = @Zipcode", new { Zipcode = zipcode });

                if (query.Id > 0)
                    return query;
                else
                    return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao inserir o endereço!!!");

                return null;
            }
        }

        public void InserirEndereco(Address adress)
        {
            try
            {
                int idGeo = _geoRepository.GetId();

                var query = "Insert into Adress(IdGeo, Street, Suite, City, Zipcode) values(@IdGeo, @Street, @Suite, @City, @Zipcode)";

                var result = _conn.Execute(query, new { IdGeo = idGeo, Street = adress.Street, Suite = adress.Suite, City = adress.City, Zipcode = adress.Zipcode });
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
