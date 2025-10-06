using Newtonsoft.Json;
using PAW_W2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace PAW_W2.Helpers
{
    public class EvaluacionDao
    {
        private static string path => HostingEnvironment.MapPath("~/App_Data/evaluaciones.json");

        private static void Validar()
        {
            if (!File.Exists(path))
            {
                File.WriteAllText(path, "[]");
            }
        }

        public static List<Evaluacion> Leer()
        {
            Validar();
            return JsonConvert.DeserializeObject<List<Evaluacion>>(File.ReadAllText(path));
        }
        public static void Escribir(List<Evaluacion> evaluaciones)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(evaluaciones, Formatting.Indented));
        }
    }
}