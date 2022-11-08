using Newtonsoft.Json;
using RentACar.Models;
using RentACar_Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace RentACar.Controllers
{
    public class CochesController : Controller
    {
        string urlBase = "https://localhost:44317";

        ////private readonly HttpClient _httpClient = new HttpClient()
        ////{
        ////    BaseAddress = new Uri("https://localhost:44317")
        ////};

        /// <summary>
        /// Consume endpoint para obtener todos los clientes y los envia a la vista
        /// </summary>
        /// <returns>Una vista</returns>
        public async Task<ActionResult> Index()
        {
            using (var clienteHttp = new HttpClient())
            {
                clienteHttp.BaseAddress = new Uri(urlBase);
                ////clienteHttp.DefaultRequestHeaders.Accept.Clear();
                ////clienteHttp.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage respuesta = await clienteHttp.GetAsync("api/coches");

                if (respuesta.IsSuccessStatusCode)
                {
                    string resultado = respuesta.Content.ReadAsStringAsync().Result;
                    List<Coche> listaCoches = JsonConvert.DeserializeObject<List<Coche>>(resultado);

                    return this.View(listaCoches);
                }
            }

            return this.View();
        }

        private async Task<CocheViewModel> llenarModelo(CocheViewModel modelo = null)
        {
            var marcas = await GetSelectMarcas();
            var colores = await GetSelectColores();
            var garajes = await GetSelectGarajes();

            if (modelo == null)
            {
                modelo = new CocheViewModel()
                {
                    SelectMarcas = marcas.ToList(),
                    SelectColores = colores.ToList(),
                    SelectGarajes = garajes.ToList()
                };

                return modelo;
            }

            modelo.SelectMarcas = marcas.ToList();
            modelo.SelectColores = colores.ToList();
            modelo.SelectGarajes = garajes.ToList();

            return modelo;
        }

        /// <summary>
        /// Consulta los detalles para crear un nuevo coche
        /// </summary>
        /// <returns>Vista para crear un nuevo coche</returns>
        [HttpGet]
        public async Task<ActionResult> Crear()
        {
            return this.View(await llenarModelo());
        }

        /// <summary>
        /// Graba los datos de un nuevo coche en la base de datos
        /// </summary>
        /// <param name="coche">Objeto con los datos del nuevo coche</param>
        /// <returns>Redireccion a la vista Index</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Crear(CocheViewModel modeloCoche)
        {
            if (!ModelState.IsValid)
            {
                return this.View(await llenarModelo(modeloCoche));
            }

            using (var clienteHttp = new HttpClient())
            {
                clienteHttp.BaseAddress = new Uri(urlBase);
                HttpResponseMessage respuesta = await clienteHttp.PostAsync("api/coches", new StringContent(
                    new JavaScriptSerializer().Serialize(modeloCoche.Coche), Encoding.UTF8, "application/json"));

                if (respuesta.IsSuccessStatusCode)
                {
                    ModelState.Clear();
                    return this.RedirectToAction("Index");
                }
                else
                {
                    this.ViewData["MensajeError"] = "Error al crear el coche";
                }
            }
            return this.RedirectToAction("Index");
        }

        public async Task<IEnumerable<Marca>> GetSelectMarcas()
        {
            using (var clienteHttp = new HttpClient())
            {
                clienteHttp.BaseAddress = new Uri(urlBase);
                HttpResponseMessage respuesta = await clienteHttp.GetAsync("api/marcas");

                if (respuesta.IsSuccessStatusCode)
                {
                    string resultado = respuesta.Content.ReadAsStringAsync().Result;
                    List<Marca> listaMarcas = JsonConvert.DeserializeObject<List<Marca>>(resultado);
                    return listaMarcas;
                }
            }

            return null;
        }

        public async Task<IEnumerable<ColorCoche>> GetSelectColores()
        {
            using (var clienteHttp = new HttpClient())
            {
                clienteHttp.BaseAddress = new Uri(urlBase);
                HttpResponseMessage respuesta = await clienteHttp.GetAsync("api/coloresCoche");

                if (respuesta.IsSuccessStatusCode)
                {
                    string resultado = respuesta.Content.ReadAsStringAsync().Result;
                    List<ColorCoche> listaColores = JsonConvert.DeserializeObject<List<ColorCoche>>(resultado);
                    return listaColores;
                }
            }

            return null;
        }

        public async Task<IEnumerable<Garaje>> GetSelectGarajes()
        {
            using (var clienteHttp = new HttpClient())
            {
                clienteHttp.BaseAddress = new Uri(urlBase);
                HttpResponseMessage respuesta = await clienteHttp.GetAsync("api/garajes");

                if (respuesta.IsSuccessStatusCode)
                {
                    string resultado = respuesta.Content.ReadAsStringAsync().Result;
                    List<Garaje> listaGarajes = JsonConvert.DeserializeObject<List<Garaje>>(resultado);
                    return listaGarajes;
                }
            }

            return null;
        }

        ////[HttpGet]
        ////public async Task<ActionResult> ConsultarPorFiltro(int idFiltro)
        ////{
        ////    using (var clienteHttp = new HttpClient())
        ////    {
        ////        clienteHttp.BaseAddress = new Uri(urlBase);
        ////        HttpResponseMessage respuesta = await clienteHttp.GetAsync($"api/coches/{idFiltro}");

        ////        if (respuesta.IsSuccessStatusCode)
        ////        {

        ////        }
        ////    }
        ////}
    }
}