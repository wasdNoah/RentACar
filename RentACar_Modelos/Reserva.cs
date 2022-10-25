namespace RentACar_Modelos
{
    public class Reserva
    {
        public int Id { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public int DiasAlquiler { get; set; }
        public int IdCliente { get; set; }
        public int IdCoche { get; set; }
        public decimal PrecioTotal { get; set; }
    }
}
