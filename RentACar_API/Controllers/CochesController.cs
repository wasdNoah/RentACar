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

namespace RentACar_API.Controllers
{
    [Route("api/coches")]
    public class CochesController : ApiController
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["webapi_conn"].ConnectionString);

        [HttpGet]
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
                    coches.Add(coche);
                }
            }
            return coches;
        }

        [HttpPost]
        public IHttpActionResult CrearCoche(Coche coche)
        {
            if (coche != null)
            {
                SqlCommand cmd = new SqlCommand("pr_CrearCoche", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@matricula", coche.Matricula);
                cmd.Parameters.AddWithValue("@color", coche.Color);
                cmd.Parameters.AddWithValue("@precio_alquiler", coche.PrecioAlquiler);
                cmd.Parameters.AddWithValue("@id_garaje", coche.IdGaraje);
                cmd.Parameters.AddWithValue("@id_marca", coche.IdMarca);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
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
            }
            else
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
