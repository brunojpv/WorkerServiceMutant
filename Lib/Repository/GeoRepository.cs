using Dapper;
using Lib.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Data;

namespace Lib.Repository
{
    public class GeoRepository : IGeoRepository
    {
        private readonly ILogger<GeoRepository> _logger;
        private readonly IDbConnection _conn;

        public GeoRepository(ILogger<GeoRepository> logger, IDbConnection conn)
        {
            _logger = logger;
            _conn = conn;

            if (_conn.State == ConnectionState.Closed)
                _conn.Open();
        }

        public Geo ConsultarGeo(int id)
        {
            throw new System.NotImplementedException();
        }

        public int GetId()
        {
            try
            {
                int query = _conn.QuerySingle<Geo>("SELECT ISNULL(MAX(ID), 0) + 1 AS Id FROM Geo").Id;

                return query;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar o Id da geolocalização!!!");

                return -1;
            }
        }

        public void InserirGeo(Geo geo)
        {
            try
            {
                var query = "Insert into Geo(Lat, Lng) values(@Lat, @Lng)";

                var result = _conn.Execute(query, new { Lat = geo.Lat, Lng = geo.Lng });            
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao salvar o endereço!!!");
            }
        }
    }
}
