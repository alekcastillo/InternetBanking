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
    public class PrestamoManager
    {
        string UrlBase = "http://localhost:49220/api/Prestamo/";

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
        /// Este metodo obtiene un prestamo proveniente del API
        /// </summary>
        /// <param name="token"></param>
        /// <param name="codigo"></param>
        /// <returns>Objeto Prestamo </returns>
        public async Task<Prestamo> ObtenerPrestamo(string token, string codigo)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<Prestamo>(response);
        }

        /// <summary>
        /// Este metodo obtiene la lista de prestamo del API
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Lista IEnumerable de objetos Prestamo</returns>
        public async Task<IEnumerable<Prestamo>> ObtenerPrestamo(string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(UrlBase);

            return JsonConvert.DeserializeObject<IEnumerable<Prestamo>>(response);
        }

        public async Task<Prestamo> Ingresar(Prestamo prestamo, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PostAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(prestamo), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Prestamo>(await response.Content.ReadAsStringAsync());
        }

        public async Task<Prestamo> Actualizar(Prestamo prestamo, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PutAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(prestamo), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Prestamo>(await response.Content.ReadAsStringAsync());
        }

        public async Task<Prestamo> Eliminar(string codigo, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.DeleteAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<Prestamo>(await response.Content.ReadAsStringAsync());
        }
    }
}