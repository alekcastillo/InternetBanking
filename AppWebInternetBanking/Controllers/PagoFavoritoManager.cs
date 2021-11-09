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
    public class PagoFavoritoManager
    {
        string UrlBase = "http://localhost:49220/api/PagosFavoritos/";

        HttpClient GetClient(string token)
        {
            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", token);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            return httpClient;
        }

        public async Task<PagoFavorito> ObtenerPagoFavorito(string token, string codigo)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<PagoFavorito>(response);
        }

        public async Task<IEnumerable<PagoFavorito>> ObtenerPagoFavoritos(string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(UrlBase);

            return JsonConvert.DeserializeObject<IEnumerable<PagoFavorito>>(response);
        }

        public async Task<PagoFavorito> Ingresar(PagoFavorito PagoFavorito, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PostAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(PagoFavorito),
                Encoding.UTF8,
                "application/json"));

            return JsonConvert.DeserializeObject<PagoFavorito>(await
                response.Content.ReadAsStringAsync());
        }

        public async Task<PagoFavorito> Actualizar(PagoFavorito PagoFavorito, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PutAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(PagoFavorito),
                Encoding.UTF8,
                "application/json"));

            return JsonConvert.DeserializeObject<PagoFavorito>(await response.
                Content.ReadAsStringAsync());
        }

        public async Task<PagoFavorito> Eliminar(string id, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.DeleteAsync(string.Concat(UrlBase, id));

            return JsonConvert.DeserializeObject<PagoFavorito>(await
                response.Content.ReadAsStringAsync());
        }
    }
}