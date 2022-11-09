using System;
using System.ComponentModel.DataAnnotations;

namespace RentACar_Modelos
{
    public class Reserva
    {
        public int Id { get; set; }
        [Display(Name = "Fecha de Inicio")]
        public DateTime FechaInicio { get; set; }
        [Display(Name = "Fecha De Finalizacion")]
        public DateTime FechaFin { get; set; }
        [Display(Name = "ID de Cliente")]
        public int IdCliente { get; set; }
        [Display(Name = "Matricula de Coche")]
        public string Matricula { get; set; }
        public string NombreCliente { get; set; }
        public int DiasAlquiler { get; set; }
        public decimal PrecioAlquilerCoche { get; set; }
        public decimal PrecioTotal { get; set; }
        public int IdVendedor { get; set; }
        public string NombreVendedor { get; set; }
    }
}
