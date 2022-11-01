﻿using Newtonsoft.Json;
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

        /// <summary>
        /// Consulta los detalles para crear un nuevo coche
        /// </summary>
        /// <returns>Vista para crear un nuevo coche</returns>
        [HttpGet]
        public async Task<ActionResult> Crear()
        {
            using (var clienteHttp = new HttpClient())
            {
                clienteHttp.BaseAddress = new Uri(urlBase);
                ////clienteHttp.DefaultRequestHeaders.Accept.Clear();
                ////clienteHttp.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage respuestaMarcas = await clienteHttp.GetAsync("api/marcas");
                HttpResponseMessage respuestaColores = await clienteHttp.GetAsync("api/coloresCoche");
                HttpResponseMessage respuestaGarajes = await clienteHttp.GetAsync("api/garajes");

                if (respuestaMarcas.IsSuccessStatusCode
                    && respuestaColores.IsSuccessStatusCode
                    && respuestaGarajes.IsSuccessStatusCode)
                {
                    string resultadoMarcas = respuestaMarcas.Content.ReadAsStringAsync().Result;
                    List<Marca> listaMarcas = JsonConvert.DeserializeObject<List<Marca>>(resultadoMarcas);

                    string resultadoColores = respuestaColores.Content.ReadAsStringAsync().Result;
                    List<ColorCoche> listaColores = JsonConvert.DeserializeObject<List<ColorCoche>>(resultadoColores);

                    string resultadoGarajes = respuestaGarajes.Content.ReadAsStringAsync().Result;
                    List<Garaje> listaGarajes = JsonConvert.DeserializeObject<List<Garaje>>(resultadoGarajes);

                    this.ViewData["listaMarcas"] = listaMarcas;
                    this.ViewData["listaColores"] = listaColores;
                    this.ViewData["listaGarajes"] = listaGarajes;
                }
            }

            return this.View();

            ////this.ViewData["MensajeError"] = "Error al recopilar informacion para crear el coche.";
            ////return this.RedirectToAction("Index");
        }

        /// <summary>
        /// Graba los datos de un nuevo coche en la base de datos
        /// </summary>
        /// <param name="coche">Objeto con los datos del nuevo coche</param>
        /// <returns>Redireccion a la vista Index</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Crear(Coche coche)
        {
            if (!ModelState.IsValid)
            {
                return this.View(coche);
            }

            using (var clienteHttp = new HttpClient())
            {
                clienteHttp.BaseAddress = new Uri(urlBase);
                HttpResponseMessage respuesta = await clienteHttp.PostAsync("api/coches", new StringContent(
                    new JavaScriptSerializer().Serialize(coche), Encoding.UTF8, "application/json"));

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