using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PAW_W2.Models
{
    public class Evaluacion
    {
        public int Id { get; set; }
        public int TareaId { get; set; }
        public string UsuarioId { get; set; }
        public int Puntuacion { get; set; }
        public string Comentario { get; set; }
        public DateTime Fecha { get; set; }

    }
}