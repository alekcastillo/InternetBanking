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
    /// <summary>
    /// Esta clase conecta con el controlador de Activos en el API REST
    /// </summary>
    public class ActivoManager
    {
        string UrlBase = "http://localhost:49220/api/Activos/";

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
        /// Este metodo obtiene un activo proveniente del API
        /// </summary>
        /// <param name="token"></param>
        /// <param name="codigo"></param>
        /// <returns>Objeto Activo</returns>
        public async Task<Activo> ObtenerActivo(string token, string codigo)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<Activo>(response);
        }

        /// <summary>
        /// Este metodo obtiene la lista de activos del API
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Lista IEnumerable de objetos Activo</returns>
        public async Task<IEnumerable<Activo>> ObtenerActivos(string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(UrlBase);

            return JsonConvert.DeserializeObject<IEnumerable<Activo>>(response);
        }

        public async Task<Activo> Ingresar(Activo activo, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PostAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(activo), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Activo>(await response.Content.ReadAsStringAsync());
        }

        public async Task<Activo> Actualizar(Activo activo, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PutAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(activo), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Activo>(await response.Content.ReadAsStringAsync());
        }

        public async Task<Activo> Eliminar(string codigo, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.DeleteAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<Activo>(await response.Content.ReadAsStringAsync());
        }
    }
}