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
    [RoutePrefix("api/clientes")]
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
                    if (Convert.ToInt32(dt.Rows[i]["esActivo"]) == 1)
                    {
                        cliente.Id = Convert.ToInt32(dt.Rows[i]["Id"]);
                        cliente.Nombre = dt.Rows[i]["Nombre"].ToString();
                        clientes.Add(cliente);
                    }
                }
            }

            return clientes;
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult ConsultarCliente(int id)
        {
            SqlCommand cmd = new SqlCommand("pr_ConsultarCliente", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);

            SqlDataAdapter adaptador = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adaptador.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                return this.NotFound();
            }

            DataRow row = dt.Rows[0];

            Cliente cliente = new Cliente()
            {
                Id = Convert.ToInt32(row["Id"]),
                Nombre = row["Nombre"].ToString()
            };

            return this.Ok(cliente);
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
                return this.BadRequest();
            }
            return this.Ok();
        }

        /// <summary>
        /// Actualiza los datos de un cliente
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns>HttpActionResult</returns>
        [HttpPut]
        public IHttpActionResult ActualizarCliente(Cliente clienteActualizar)
        {
            if (clienteActualizar != null)
            {
                SqlCommand cmd = new SqlCommand("pr_ActualizarCliente", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", clienteActualizar.Id);
                cmd.Parameters.AddWithValue("@nombre", clienteActualizar.Nombre);
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
                return this.BadRequest();
            }

            return this.Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IHttpActionResult> EliminarCliente(int id)
        {
            SqlCommand cmd = new SqlCommand("pr_CambiarEstadoCliente", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@tipoAccion", 1);

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

            return this.Ok();
        }
    }
}
