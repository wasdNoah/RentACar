using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RentACar_Modelos;

namespace RentACar.Controllers
{
    public class ClientesController : Controller
    {
        string urlBase = "https://localhost:44317";

        public async Task<ActionResult> Index()
        {
            using(var cliente = new HttpClient())
            {
                cliente.BaseAddress = new Uri(urlBase);
                cliente.DefaultRequestHeaders.Accept.Clear();
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage respuesta = await cliente.GetAsync("api/clientes");

                if (respuesta.IsSuccessStatusCode)
                {
                    string resultado = respuesta.Content.ReadAsStringAsync().Result;
                    List<Cliente> listaClientes = JsonConvert.DeserializeObject<List<Cliente>>(resultado);

                    return this.View(listaClientes);
                }
                else
                {
                    Console.WriteLine("Error al llamar a la API");
                }
            }

            return this.View();
        }
    }
}