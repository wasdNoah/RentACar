using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RentACar_Modelos
{
    public class Coche
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio.")]
        [MaxLength(7, ErrorMessage = "La matricula debe contener 7 caracteres.")]
        [MinLength(7, ErrorMessage = "La matricula debe contener 7 caracteres.")]
        public string Matricula { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio.")]
        [Range(1, Int32.MaxValue, ErrorMessage = "El precio de alquiler debe ser mayor a cero.")]
        public decimal PrecioAlquiler { get; set; }
        public int? IdGaraje { get; set; }
        public string Garaje { get; set; }
        public int? IdMarca { get; set; }
        public string Marca { get; set; }
        public int? IdColor { get; set; }
        public string Color { get; set; }
        public int? Disponible { get; set; }
        public int? EsActivo { get; set; }
    }
}
