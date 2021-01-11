using Lib.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Lib.Service
{
    public class Requesters
    {
        public static List<Users> ListUsers { get; set; }
        private static ILogger<Requesters> _logger;
        static HttpClient client = new HttpClient();

        public Requesters(ILogger<Requesters> logger)
        {
            _logger = logger;
        }

        public static async Task<HttpStatusCode> GetStatusFromUrl(string url)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                return response.StatusCode;

            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API não encontrada!!!");

                return HttpStatusCode.NotFound;
            }
        }

        public static async Task<List<Users>> GetUsersAsync(string url)
        {
            try
            {
                string users = null;

                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    users = await response.Content.ReadAsStringAsync();
                    return ListUsers = JsonConvert.DeserializeObject<List<Users>>(users).ToList();
                }
                else
                {
                    _logger.LogInformation("JSON inválido");

                    return null;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Erro no JSON!!!");

                return null;
            }
        }
    }
}
