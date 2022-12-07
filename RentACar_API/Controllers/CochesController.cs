using RentACar_Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading.Tasks;

namespace RentACar_API.Controllers
{
    [RoutePrefix("api")]
    public class CochesController : ApiController
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["webapi_conn"].ConnectionString);

        /// <summary>
        /// Trae todos los coches de la base de datos
        /// </summary>
        /// <returns>Objeto IEnumerable con los coches</returns>
        [HttpGet]
        [Route("coches")]
        public IEnumerable<Coche> ConsultarCoches()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("pr_ConsultarCoches", conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            List<Coche> coches = new List<Coche>();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Coche coche = new Coche();
                    coche.Id = Convert.ToInt32(dt.Rows[i]["Id"]);
                    coche.Matricula = dt.Rows[i]["Matricula"].ToString();
                    coche.Color = dt.Rows[i]["Color"].ToString();
                    coche.PrecioAlquiler = Convert.ToDecimal(dt.Rows[i]["PrecioAlquiler"]);
                    coche.Garaje = dt.Rows[i]["Garaje"].ToString();
                    coche.Marca = dt.Rows[i]["Marca"].ToString();
                    coche.EsActivo = Convert.ToInt32(dt.Rows[i]["EsActivo"]);
                    coche.Disponible = Convert.ToInt32(dt.Rows[i]["Disponible"]);
                    coches.Add(coche);
                }
            }
            return coches;
        }

        /// <summary>
        /// Trae todos los garajes de la base de datos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("garajes")]
        public IHttpActionResult ConsultarGarajes()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("pr_ConsultarGarajes", conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            List<Garaje> listaGarajes = new List<Garaje>();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Garaje garaje = new Garaje();
                    garaje.IdGaraje = Convert.ToInt32(dt.Rows[i]["Id"]);
                    garaje.Direccion = dt.Rows[i]["Direccion"].ToString();
                    listaGarajes.Add(garaje);
                }
            }

            return this.Ok(listaGarajes);
        }

        /// <summary>
        /// Trae todas las marcas de la base de datos
        /// </summary>
        /// <returns>HttpResult</returns>
        [HttpGet]
        [Route("marcas")]
        public IHttpActionResult ConsultarMarcas()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("pr_ConsultarMarcas", conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            List<Marca> listaMarcas = new List<Marca>();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Marca marca = new Marca();
                    marca.IdMarca = Convert.ToInt32(dt.Rows[i]["Id"]);
                    marca.Nombre = dt.Rows[i]["Nombre"].ToString();
                    listaMarcas.Add(marca);
                }
            }

            return this.Ok(listaMarcas);
        }

        /// <summary>
        /// Trae todos los colores de coche de la base de datos
        /// </summary>
        /// <returns>HttpResult</returns>
        [HttpGet]
        [Route("coloresCoche")]
        public IHttpActionResult ConsultarColoresCoche()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("pr_ConsultarColoresCoche", conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            List<ColorCoche> listaColores = new List<ColorCoche>();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ColorCoche color = new ColorCoche();
                    color.IdColor = Convert.ToInt32(dt.Rows[i]["Id"]);
                    color.Color = dt.Rows[i]["Color"].ToString();
                    listaColores.Add(color);
                }
            }

            return this.Ok(listaColores);
        }

        /// <summary>
        /// Graba un nuevo objeto Coche en la base de datos
        /// </summary>
        /// <param name="coche">Objeto con la informacion del coche</param>
        /// <returns>HttpResult</returns>
        [HttpPost]
        [Route("coches")]
        public async Task<IHttpActionResult> CrearCoche(Coche coche)
        {
            if (coche is null)
            {
                return BadRequest();
            }

            SqlCommand cmd = new SqlCommand("pr_CrearCoche", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@matricula", coche.Matricula);
            cmd.Parameters.AddWithValue("@precio_alquiler", coche.PrecioAlquiler);
            cmd.Parameters.AddWithValue("@id_garaje", coche.IdGaraje);
            cmd.Parameters.AddWithValue("@id_marca", coche.IdMarca);
            cmd.Parameters.AddWithValue("@id_color", coche.IdColor);

            try
            {
                conn.Open();
                await cmd.ExecuteNonQueryAsync();
                conn.Close();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

            return Ok("Coche creado con exito");
        }


        [HttpGet]
        [Route("cochesDisponibles")]
        public IHttpActionResult ConsultarCochesDisponibles()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("pr_ListaCochesDisponibles", conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            List<Coche> coches = new List<Coche>();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Coche coche = new Coche();
                    coche.Id = Convert.ToInt32(dt.Rows[i]["Id"]);
                    coche.Matricula = dt.Rows[i]["Matricula"].ToString();
                    coche.Color = dt.Rows[i]["Color"].ToString();
                    coche.PrecioAlquiler = Convert.ToDecimal(dt.Rows[i]["PrecioAlquiler"]);
                    coche.Garaje = dt.Rows[i]["Garaje"].ToString();
                    coche.Marca = dt.Rows[i]["Marca"].ToString();
                    coches.Add(coche);
                }
            }
            return Ok(coches);
        }

        ////[HttpGet]
        ////[Route("api/coches/{idFiltro}")]
        ////public async Task<IHttpActionResult> ConsultarPorFiltro(int idFiltro)
        ////{

        ////}
    }
}
