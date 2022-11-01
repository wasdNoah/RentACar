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
        public IEnumerable<Marca> SelectMarcas { get; set; }
        public IEnumerable<ColorCoche> SelectColores { get; set; }
        public IEnumerable<Garaje> SelectGarajes { get; set; }
        public Coche Coche { get; set; }
    }
}