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
    public class CertificadosDepositosManager
    {
        string UrlBase = "http://localhost:49220/api/CertificadosDepositos/";

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
        /// Este metodo obtiene un Certificado proveniente del API
        /// </summary>
        /// <param name="token"></param>
        /// <param name="codigo"></param>
        /// <returns>Objeto Certificado</returns>
        public async Task<CertificadosDepositos> ObtenerCertificado(string token, string codigo)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<CertificadosDepositos>(response);
        }

        /// <summary>
        /// Este metodo obtiene la lista de Certificado del API
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Lista IEnumerable de objetos Certificado</returns>
        public async Task<IEnumerable<CertificadosDepositos>> ObtenerCertificados(string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(UrlBase);

            return JsonConvert.DeserializeObject<IEnumerable<CertificadosDepositos>>(response);
        }

        public async Task<CertificadosDepositos> Ingresar(CertificadosDepositos certificadosDepositos, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PostAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(certificadosDepositos), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<CertificadosDepositos>(await response.Content.ReadAsStringAsync());
        }

        public async Task<CertificadosDepositos> Actualizar(CertificadosDepositos certificadosDepositos, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PutAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(certificadosDepositos), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<CertificadosDepositos>(await response.Content.ReadAsStringAsync());
        }

        public async Task<CertificadosDepositos> Eliminar(string codigo, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.DeleteAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<CertificadosDepositos>(await response.Content.ReadAsStringAsync());
        }
    }
}