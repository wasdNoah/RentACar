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
    [Route("api/clientes")]
    public class ClientesController : ApiController
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["webapi_conn"].ConnectionString);

        [HttpGet]
        public IEnumerable<Cliente> ConsultarClientes()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("pr_ConsultarClientes", conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            List<Cliente> clientes = new List<Cliente>();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Cliente cliente = new Cliente();
                    cliente.Id = Convert.ToInt32(dt.Rows[i]["Id"]);
                    cliente.Nombre = dt.Rows[i]["Nombre"].ToString();
                    clientes.Add(cliente);
                }
            }

            return clientes;
        }

        [HttpPost]
        public IHttpActionResult CrearCliente(Cliente cliente)
        {
            if (cliente != null)
            {
                SqlCommand cmd = new SqlCommand("pr_CrearCliente", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nombre", cliente.Nombre);
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
