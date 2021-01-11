using Lib.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace Lib.Repository
{
    public class Repository : IRepository
    {
        private readonly ILogger<UsersRepository> _logger;
        private IUsersRepository _usersRepository;
        private IAdressRepository _adressRepository;
        private ICompanyRepository _companyRepository;
        private IGeoRepository _geoRepository;

        public Repository(ILogger<UsersRepository> logger,
                          IUsersRepository usersRepository,
                          IAdressRepository adressRepository,
                          ICompanyRepository companyRepository,
                          IGeoRepository geoRepository)
        {
            _usersRepository = usersRepository;
            _adressRepository = adressRepository;
            _companyRepository = companyRepository;
            _geoRepository = geoRepository;
            _logger = logger;            
        }

        public void BaixarDados(Users user)
        {
            try
            {
                string fileName = "Json\\Users.txt";
                var json = JsonConvert.SerializeObject(user);
                string jsonString = JValue.Parse(json).ToString(Formatting.Indented);
                File.AppendAllText(fileName, jsonString);

                Console.WriteLine(jsonString);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao baixar dados!!!");
            }
        }

        public void SalvarDados(Users users)
        {
            _usersRepository.InserirUsuario(users);
            _adressRepository.InserirEndereco(users.Address);
            _companyRepository.InserirCompanhia(users.Company);
            _geoRepository.InserirGeo(users.Address.Geo);
        }
    }
}
