using Dapper;
using Lib.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Data;

namespace Lib.Repository
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ILogger<UsersRepository> _logger;
        private readonly IDbConnection _conn;
        private IAdressRepository _adressRepository;
        private ICompanyRepository _companyRepository;

        public UsersRepository(ILogger<UsersRepository> logger,
                               IDbConnection conn,
                               IAdressRepository adressRepository,
                               ICompanyRepository companyRepository)
        {
            _adressRepository = adressRepository;
            _companyRepository = companyRepository;
            _logger = logger;
            _conn = conn;

            if (_conn.State == ConnectionState.Closed)
                _conn.Open();
        }

        public Users ConsultarUsuario(string userName)
        {
            try
            {
                var query = _conn.QuerySingleOrDefault<Users>(@"SELECT * FROM Users WHERE UserName = @UserName", new { UserName = userName });

                if (query != null)
                    return query;
                else
                    return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar o usuários!!!");

                return null;
            }
        }        

        public void InserirUsuario(Users user)
        {
            try
            {
                int idAdress  = _adressRepository.GetId();
                int idCompany = _companyRepository.GetId(); 
                var consulta  = ConsultarUsuario(user.Username);

                if (consulta == null)
                {
                    var query = "Insert into Users(Id, IdAdress, IdCompany, Name, UserName, Email, Phone, Website) values(@Id, @IdAdress, @IdCompany, @Name, @Username, @Email, @Phone, @Website)";

                    var result = _conn.Execute(query, new { Id = user.Id, IdAdress = idAdress, IdCompany = idCompany, Name = user.Name, Username = user.Username, Email = user.Email, Phone = user.Phone, Website = user.Website });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro salvar dados!!!");
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
