using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AppWebInternetBanking.Models;

namespace AppWebInternetBanking.Controllers
{
    public class SolicitudPrestamoManager
    {
        string UrlBase = "http://localhost:49220/api/SolicitudPrestamo/";

        HttpClient GetClient(string token)
        {
            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", token);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            return httpClient;
        }

        public async Task<SolicitudPrestamo> ObtenerSolicitudPrestamo(string token, string codigo)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<SolicitudPrestamo>(response);
        }

        public async Task<IEnumerable<SolicitudPrestamo>> ObtenerSolicitudPrestamos(string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(UrlBase);

            return JsonConvert.DeserializeObject<IEnumerable<SolicitudPrestamo>>(response);
        }

        public async Task<SolicitudPrestamo> Ingresar(SolicitudPrestamo solicitudPrestamo, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PostAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(solicitudPrestamo),
                Encoding.UTF8,
                "application/json"));

            return JsonConvert.DeserializeObject<SolicitudPrestamo>(await
                response.Content.ReadAsStringAsync());
        }

        public async Task<SolicitudPrestamo> Actualizar(SolicitudPrestamo solicitudPrestamo, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PutAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(solicitudPrestamo),
                Encoding.UTF8,
                "application/json"));

            return JsonConvert.DeserializeObject<SolicitudPrestamo>(await response.
                Content.ReadAsStringAsync());
        }

        public async Task<SolicitudPrestamo> Eliminar(string id, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.DeleteAsync(string.Concat(UrlBase, id));

            return JsonConvert.DeserializeObject<SolicitudPrestamo>(await
                response.Content.ReadAsStringAsync());
        }
    }
}