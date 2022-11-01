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
    [Route("api/reservas")]
    public class ReservasController : ApiController
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["webapi_conn"].ConnectionString);

        [HttpGet]
        public IEnumerable<Reserva> ConsultarReservas()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("pr_ConsultarReservas", conn);
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
                    reserva.DiasAlquiler = Convert.ToInt32(dt.Rows[i]["DiasAlquiler"]);
                    reserva.NombreCliente = dt.Rows[i]["NombreCliente"].ToString();
                    reserva.IdCliente = Convert.ToInt32(dt.Rows[i]["IdCliente"]);
                    reserva.Matricula = dt.Rows[i]["Matricula"].ToString();
                    reserva.IdCoche = Convert.ToInt32(dt.Rows[i]["IdCoche"]);
                    reserva.PrecioAlquilerCoche = Convert.ToDecimal(dt.Rows[i]["PrecioAlquilerCoche"]);
                    reserva.PrecioTotal = Convert.ToDecimal(dt.Rows[i]["PrecioTotal"]);
                    reservas.Add(reserva);
                }
            }
            return reservas;
        }
    }
}
