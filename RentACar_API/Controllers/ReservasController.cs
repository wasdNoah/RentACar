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
    [Route("api/reservas")]
    public class ReservasController : ApiController
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["webapi_conn"].ConnectionString);

        [HttpGet]
        public IEnumerable<Reserva> ConsultarReservas()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("reservas.pr_ConsultarReservas", conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            List<Reserva> reservas = new List<Reserva>();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Reserva reserva = new Reserva();
                    reserva.Id = Convert.ToInt32(dt.Rows[i]["Id"]);
                    reserva.FechaInicio = DateTime.Parse(dt.Rows[i]["FechaInicio"].ToString());
                    reserva.FechaFin = DateTime.Parse(dt.Rows[i]["FechaFin"].ToString());
                    reserva.IdCliente = Convert.ToInt32(dt.Rows[i]["IdCliente"]);
                    reserva.NombreCliente = dt.Rows[i]["NombreCliente"].ToString();
                    reserva.Matricula = dt.Rows[i]["Matricula"].ToString();
                    reserva.PrecioAlquilerCoche = Convert.ToDecimal(dt.Rows[i]["PrecioAlquilerCoche"]);
                    reserva.PrecioTotal = Convert.ToDecimal(dt.Rows[i]["PrecioTotal"]);
                    reserva.DiasAlquiler = Convert.ToInt32(dt.Rows[i]["DiasAlquiler"]);
                    reserva.IdVendedor = Convert.ToInt32(dt.Rows[i]["IdVendedor"]);
                    reserva.NombreVendedor = dt.Rows[i]["NombreVendedor"].ToString();
                    reservas.Add(reserva);
                }
            }
            return reservas;
        }

        [HttpPost]
        public async Task<IHttpActionResult> CrearReserva(Reserva reserva)
        {
            if (reserva == null)
            {
                return BadRequest("No existe reserva en la peticion.");
            }

            // verificar que el coche se encuentre disponible
            using (SqlCommand cmd = new SqlCommand("reservas.pr_ConcheSeEncuentraEnReservaActiva", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@matricula", reserva.Matricula);
                cmd.Parameters.AddWithValue("@fecha_inicio_nueva_reserva", reserva.FechaFin);

                IDbDataParameter resultadoSP = cmd.CreateParameter();
                resultadoSP.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add(resultadoSP);

                try
                {
                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    conn.Close();

                    if (Convert.ToInt32(resultadoSP.Value) == 1)
                    {
                        return BadRequest("El coche no se encuentra disponible.");
                    }
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

            // verificar si el cliente cuenta con alguna reserva activa
            using (SqlCommand cmd = new SqlCommand("reservas.pr_ClienteCuentaConReservaActiva", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_cliente", reserva.IdCliente);
                cmd.Parameters.AddWithValue("@fecha_inicio", reserva.FechaInicio);

                IDbDataParameter resultadoSP = cmd.CreateParameter();
                resultadoSP.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add(resultadoSP);

                try
                {
                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    if (Convert.ToInt32(resultadoSP.Value) == 1)
                    {
                        return BadRequest("Imposible agregar reserva. El cliente ya cuenta con otra reserva para la fecha establecida.");
                    }
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

            // el coche se encuentra disponible y el cliente no cuenta con reservas activas
            // el flujo continua con la creacion de la reserva
            using (SqlCommand cmd = new SqlCommand("reservas.pr_CrearReserva", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_cliente", reserva.IdCliente);
                cmd.Parameters.AddWithValue("@matricula", reserva.Matricula);
                cmd.Parameters.AddWithValue("@fecha_inicio", reserva.FechaInicio);
                cmd.Parameters.AddWithValue("@fecha_fin", reserva.FechaFin);
                cmd.Parameters.AddWithValue("@id_vendedor", reserva.IdVendedor);

                try
                {
                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    conn.Close();
                }
                catch (Exception)
                {
                    return this.InternalServerError();
                }
                finally
                {
                    conn.Close();
                }
            }

            return Ok("Reserva creada exitosamente.");
        }

        [HttpGet]
        [Route("api/reservasFinalizadas")]
        public IHttpActionResult ConsultarReservasFinalizadas()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("reservas.pr_ConsultarReservasFinalizadas", conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            List<Reserva> reservas = new List<Reserva>();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Reserva reserva = new Reserva();
                    reserva.Id = Convert.ToInt32(dt.Rows[i]["Id"]);
                    reserva.FechaInicio = DateTime.Parse(dt.Rows[i]["FechaInicio"].ToString());
                    reserva.FechaFin = DateTime.Parse(dt.Rows[i]["FechaFin"].ToString());
                    reserva.IdCliente = Convert.ToInt32(dt.Rows[i]["IdCliente"]);
                    reserva.NombreCliente = dt.Rows[i]["NombreCliente"].ToString();
                    reserva.Matricula = dt.Rows[i]["Matricula"].ToString();
                    reserva.PrecioAlquilerCoche = Convert.ToDecimal(dt.Rows[i]["PrecioAlquilerCoche"]);
                    reserva.PrecioTotal = Convert.ToDecimal(dt.Rows[i]["PrecioTotal"]);
                    reserva.DiasAlquiler = Convert.ToInt32(dt.Rows[i]["DiasAlquiler"]);
                    reserva.IdVendedor = Convert.ToInt32(dt.Rows[i]["IdVendedor"]);
                    reserva.NombreVendedor = dt.Rows[i]["NombreVendedor"].ToString();
                    reservas.Add(reserva);
                }
            }

            return this.Ok(reservas);
        }

        [HttpGet]
        [Route("api/reservas/{id}")]
        public IHttpActionResult ConsultarReserva(int? id)
        {
            // Verifica si existe el ID y si es valido
            if (id is null || id <= 0)
            {
                return BadRequest("Debe proveer el ID, y éste no debe ser igual o menor a cero.");
            }

            SqlCommand cmd = new SqlCommand("reservas.pr_ConsultarReservaPorId", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id_reserva", id);

            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = cmd;
            adapter.Fill(dt);

            // verifica que exista una reserva con el id proveido
            if (dt.Rows.Count == 0)
            {
                return BadRequest($"No existe la reserva con ID {id}");
            }

            DataRow primerElemento = dt.Rows[0];
            Reserva reserva = new Reserva();

            reserva.Id = Convert.ToInt32(primerElemento["Id"]);
            reserva.Matricula = primerElemento["MatriculaCoche"].ToString();
            reserva.IdCliente = Convert.ToInt32(primerElemento["IdCliente"]);
            reserva.FechaInicio = DateTime.Parse(primerElemento["FechaInicio"].ToString());
            reserva.FechaFin = DateTime.Parse(primerElemento["FechaFin"].ToString());
            reserva.EsActiva = Convert.ToInt32(primerElemento["EsActiva"]);

            return this.Ok(reserva);
        }
    }
}
