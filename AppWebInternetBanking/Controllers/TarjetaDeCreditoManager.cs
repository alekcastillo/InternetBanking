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
    public class TarjetaDeCreditoManager
    {
        string UrlBase = "http://localhost:49220/api/TarjetasDeCredito/";

        HttpClient GetClient(string token)
        {
            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", token);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            return httpClient;
        }

        public async Task<TarjetaDeCredito> ObtenerTarjetaDeCredito(string token, string codigo)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<TarjetaDeCredito>(response);
        }

        public async Task<IEnumerable<TarjetaDeCredito>> ObtenerTarjetaDeCreditos(string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(UrlBase);

            return JsonConvert.DeserializeObject<IEnumerable<TarjetaDeCredito>>(response);
        }

        public async Task<TarjetaDeCredito> Ingresar(TarjetaDeCredito TarjetaDeCredito, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PostAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(TarjetaDeCredito),
                Encoding.UTF8,
                "application/json"));

            return JsonConvert.DeserializeObject<TarjetaDeCredito>(await
                response.Content.ReadAsStringAsync());
        }

        public async Task<TarjetaDeCredito> Actualizar(TarjetaDeCredito TarjetaDeCredito, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PutAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(TarjetaDeCredito),
                Encoding.UTF8,
                "application/json"));

            return JsonConvert.DeserializeObject<TarjetaDeCredito>(await response.
                Content.ReadAsStringAsync());
        }

        public async Task<TarjetaDeCredito> Eliminar(string id, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.DeleteAsync(string.Concat(UrlBase, id));

            return JsonConvert.DeserializeObject<TarjetaDeCredito>(await
                response.Content.ReadAsStringAsync());
        }
    }
}