using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PAW_W2.Models
{
    public class Tarea
    {
        public int Id { get; set; }

        public string AutorId { get; set; }

        public string Titulo { get; set; }

        public string Descripcion { get; set; }

        public string Lenguajes { get; set; }

        public string UrlRepo { get; set; }

        public string Categoria { get; set; }

        public DateTime Fecha { get; set; }
    }
}