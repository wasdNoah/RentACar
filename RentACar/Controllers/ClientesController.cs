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
using System.Web.Script.Serialization;
using System.Text;

namespace RentACar.Controllers
{
    public class ClientesController : Controller
    {
        string urlBase = "https://localhost:44317";

        [HttpGet]
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

        [HttpGet]
        public ActionResult Crear()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Crear(Cliente cliente)
        {
            using (var clienteHttp = new HttpClient())
            {
                clienteHttp.BaseAddress = new Uri(urlBase);
                HttpResponseMessage respuesta = await clienteHttp.PostAsync("api/clientes", new StringContent(
                    new JavaScriptSerializer().Serialize(cliente), Encoding.UTF8, "application/json"));

                if (respuesta.IsSuccessStatusCode)
                {
                    ModelState.Clear();
                    return this.View();
                }
                else
                {
                    this.ViewData["MensajeError"] = "Algo salió mal al guardar el nuevo cliente";
                }
            }

            return this.View();
        }

        /// <summary>
        /// Hace una petición al api para obtener la información de un cliente y cargarla a la vista
        /// </summary>
        /// <param name="clienteId">Id del cliente a editar </param>
        /// <returns>Vista para editar los datos del cliente</returns>
        [HttpGet]
        public async Task<ActionResult> Editar(int? clienteId)
        {
            using (var clienteHttp = new HttpClient())
            {
                clienteHttp.BaseAddress = new Uri(urlBase);
                clienteHttp.DefaultRequestHeaders.Accept.Clear();
                clienteHttp.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage respuesta = await clienteHttp.GetAsync($"api/clientes/{clienteId}");

                if (respuesta.IsSuccessStatusCode)
                {
                    string resultado = respuesta.Content.ReadAsStringAsync().Result;
                    Cliente clienteEncontrado = JsonConvert.DeserializeObject<Cliente>(resultado);

                    this.Session["IdCliente"] = clienteEncontrado.Id;
                    return this.View(clienteEncontrado);
                }
                else
                {
                    this.ViewData["MensajeError"] = "Cliente no encontrado";
                }

                return this.View();
            }
        }

        /// <summary>
        /// Graba los datos de un cliente editado.
        /// </summary>
        /// <param name="cliente">Cliente con los datos actualizados</param>
        /// <returns>Una redirección a la vista con todos los usuarios</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar([Bind(Exclude = "Id")]Cliente cliente)
        {
            if (this.Session["IdCliente"] == null)
            {
                this.ViewData["MensajeError"] = "No se encontró el ID del cliente a editar. Por favor, volver a seleccionar.";
                return this.View(cliente);
            }

            cliente.Id = (int)this.Session["IdCliente"];
            using (var clienteHttp = new HttpClient())
            {
                clienteHttp.BaseAddress = new Uri(urlBase);
                HttpResponseMessage respuesta = await clienteHttp.PutAsync($"api/clientes", new StringContent(
                    new JavaScriptSerializer().Serialize(cliente), Encoding.UTF8, "application/json"));

                if (respuesta.IsSuccessStatusCode)
                {
                    this.ModelState.Clear();
                    return this.RedirectToAction("Index");
                }
            }

            this.ViewData["MensajeError"] = "No se pudo encontrar el cliente";
            return this.View();
        }

        /// <summary>
        /// Cambia el estado de un cliente a no activo. Ya no lo muestra en la vista.
        /// </summary>
        /// <param name="Id">Id del cliente a eliminar</param>
        /// <returns>Redirección a la vista con todos clientes</returns>
        public async Task<ActionResult> Eliminar(int? Id)
        {
            using (var clienteHttp = new HttpClient())
            {
                clienteHttp.BaseAddress = new Uri(urlBase);
                HttpResponseMessage respuesta = await clienteHttp.DeleteAsync($"api/clientes/{Id}");

                if (respuesta.IsSuccessStatusCode)
                {
                    return this.RedirectToAction("Index");
                }
            }

            this.ViewData["MensajeError"] = $"No se pudo eliminar el cliente con id: {Id}";
            return this.RedirectToAction("Index");
        }
    }
}