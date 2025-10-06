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
    public class UsuarioDao
    {
        private static string path => HostingEnvironment.MapPath("~/App_Data/usuarios.json");

        private static void Validar()
        {
            if (!File.Exists(path))
            {
                File.WriteAllText(path, "[]");
            }
        }

        public static List<Usuario> Leer()
        {
            Validar();
            return JsonConvert.DeserializeObject<List<Usuario>>(File.ReadAllText(path));
        }

        public static Usuario Login(string user, string pass)
        {
            return Leer().FirstOrDefault(u => u.User == user && u.Pass == pass);
        }
    }
}