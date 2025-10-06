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
    public class TareaDao
    {
        private static string path => HostingEnvironment.MapPath("~/App_Data/tareas.json");

        private static void Validar()
        {
            if (!File.Exists(path))
            {
                File.WriteAllText(path, "[]");
            }
        }

        public static List<Tarea> Leer()
        {
            Validar();
            return JsonConvert.DeserializeObject<List<Tarea>>(File.ReadAllText(path));
        }
        public static void Escribir(List<Tarea> tareas)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(tareas, Formatting.Indented));
        }
    }
}