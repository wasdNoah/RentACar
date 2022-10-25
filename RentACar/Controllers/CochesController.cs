using Newtonsoft.Json;
using RentACar_Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RentACar.Controllers
{
    public class CochesController : Controller
    {
        string urlBase = "https://localhost:44317";

        public async Task<ActionResult> Index()
        {
            using (var cliente = new HttpClient())
            {
                cliente.BaseAddress = new Uri(urlBase);
                cliente.DefaultRequestHeaders.Accept.Clear();
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage respuesta = await cliente.GetAsync("api/coches");

                if (respuesta.IsSuccessStatusCode)
                {
                    string resultado = respuesta.Content.ReadAsStringAsync().Result;
                    List<Coche> listaCoches = JsonConvert.DeserializeObject<List<Coche>>(resultado);

                    return this.View(listaCoches);
                }
            }

            return View();
        }
    }
}