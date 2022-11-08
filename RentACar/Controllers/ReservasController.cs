using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RentACar_Modelos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace RentACar.Controllers
{
    public class ReservasController : Controller
    {
        string urlBase = "https://localhost:44317";

        // GET: Reservas
        public async Task<ActionResult> Index()
        {
            this.ViewData["TextoBotonResultados"] = "Finalizadas";

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

        public ActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Crear(Reserva reserva)
        {
            using (var clienteHttp = new HttpClient())
            {
                try
                {
                    clienteHttp.BaseAddress = new Uri(urlBase);
                    HttpResponseMessage respuesta = await clienteHttp.PostAsync("api/reservas", new StringContent(
                        new JavaScriptSerializer().Serialize(reserva), Encoding.UTF8, "application/json"));

                    // si algo sale mal con la peticion
                    if (respuesta.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        JObject contenidoRespuesta = JObject.Parse(await respuesta.Content.ReadAsStringAsync());
                        string mensajeApi = contenidoRespuesta.GetValue("Message").ToString();

                        this.ViewData["MensajeError"] = mensajeApi;
                        return this.View();
                    }
                    else if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        this.ViewData["MensajeExito"] = await respuesta.Content.ReadAsStringAsync();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return this.View();
        }

        public async Task<ActionResult> BitacoraReservas()
        {
            this.ViewData["TextoBotonResultados"] = "Todas";

            using (var clientHttp = new HttpClient())
            {
                clientHttp.BaseAddress = new Uri(urlBase);
                var respuesta = await clientHttp.GetAsync("api/reservasFinalizadas");

                if (respuesta.IsSuccessStatusCode)
                {
                    string resultado = await respuesta.Content.ReadAsStringAsync();
                    List<Reserva> listaReservas = JsonConvert.DeserializeObject<List<Reserva>>(resultado);
                    return this.View("Index", listaReservas);
                }
            }

            return this.View("Index");
        }
    }
}