using AppWebInternetBanking.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AppWebInternetBanking.Controllers
{
    public class SeguroManager
    {
        string UrlBase = "http://localhost:49220/api/Seguro/";

        /// <summary>
        /// Metodo que inicializa el objeto HttpClient
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Objeto HttpClient con los headers inicializados</returns>
        HttpClient GetClient(string token)
        {
            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", token);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            return httpClient;
        }

        /// <summary>
        /// Este metodo obtiene un seguro proveniente del API
        /// </summary>
        /// <param name="token"></param>
        /// <param name="codigo"></param>
        /// <returns>Objeto Seguro</returns>
        public async Task<Seguro> ObtenerSeguro(string token, string codigo)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<Seguro>(response);
        }

        /// <summary>
        /// Este metodo obtiene la lista de seguro del API
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Lista IEnumerable de objetos Seguro</returns>
        public async Task<IEnumerable<Seguro>> ObtenerSeguro(string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(UrlBase);

            return JsonConvert.DeserializeObject<IEnumerable<Seguro>>(response);
        }

        public async Task<Seguro> Ingresar(Seguro seguro, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PostAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(seguro), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Seguro>(await response.Content.ReadAsStringAsync());
        }

        public async Task<Seguro> Actualizar(Seguro seguro, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PutAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(seguro), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Seguro>(await response.Content.ReadAsStringAsync());
        }

        public async Task<Seguro> Eliminar(string codigo, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.DeleteAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<Seguro>(await response.Content.ReadAsStringAsync());
        }
    }
}