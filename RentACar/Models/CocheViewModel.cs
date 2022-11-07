using RentACar_Modelos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentACar.Models
{
    public class CocheViewModel
    {
        public CocheViewModel()
        {
            this.SelectMarcas = new List<Marca>();
            this.SelectGarajes = new List<Garaje>();
            this.SelectColores = new List<ColorCoche>();
        }

        [Display(Name = "Seleccionar Marca")]
        public List<Marca> SelectMarcas { get; set; }
        [Display(Name = "Seleccionar Color")]
        public List<ColorCoche> SelectColores { get; set; }
        [Display(Name = "Seleccionar Garaje")]
        public List<Garaje> SelectGarajes { get; set; }
        public Coche Coche { get; set; }
    }
}