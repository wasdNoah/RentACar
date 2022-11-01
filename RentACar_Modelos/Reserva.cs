using System;

namespace RentACar_Modelos
{
    public class Reserva
    {
        public int Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int DiasAlquiler { get; set; }
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; }
        public int IdCoche { get; set; }
        public string Matricula { get; set; }
        public decimal PrecioAlquilerCoche { get; set; }
        public decimal PrecioTotal { get; set; }
    }
}
