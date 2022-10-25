namespace RentACar_Modelos
{
    public class Coche
    {
        public int Id { get; set; }
        public string Matricula { get; set; }
        public string Color { get; set; }
        public decimal PrecioAlquiler { get; set; }
        public int IdGaraje { get; set; }
        public string Garaje { get; set; }
        public int IdMarca { get; set; }
        public string Marca { get; set; }

    }
}
