using Newtonsoft.Json;
using RentACar_Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RentACar.Controllers
{
    public class ReservasController : Controller
    {
        string urlBase = "https://localhost:44317";

        // GET: Reservas
        public async Task<ActionResult> Index()
        {
            using (var httpCliente = new HttpClient())
            {
                httpCliente.BaseAddress = new Uri(urlBase);
                HttpResponseMessage respuesta = await httpCliente.GetAsync("api/reservas");

                if (respuesta.IsSuccessStatusCode)
                {
                    string resultado = respuesta.Content.ReadAsStringAsync().Result;
                    List<Reserva> listaReservas = JsonConvert.DeserializeObject<List<Reserva>>(resultado);

                    return this.View(listaReservas);
                }
            }
            return View();
        }
    }
}