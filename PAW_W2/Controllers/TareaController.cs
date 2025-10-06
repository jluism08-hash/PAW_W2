using PAW_W2.Helpers;
using PAW_W2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace PAW_W2.Controllers
{
    public class TareaController : Controller
    {
        // GET: Tarea
        public ActionResult Index()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "Login");
            var items = TareaDao.Leer().OrderByDescending(t => t.Fecha).ToList();
            var rs = EvaluacionDao.Leer();
            ViewBag.Prom = rs.GroupBy(x => x.TareaId)
                             .ToDictionary(g => g.Key, g => g.Average(y => y.Puntuacion));
            return View(items);
        }

        [HttpGet]
        public ActionResult Create()
        {
            if(Session["UserId"] == null) 
                return RedirectToAction("Login", "Login");
            return View(new Tarea { Fecha = DateTime.Now});
        }

        [HttpPost]
        public JsonResult CreateAjax(Tarea tarea)
        {
            if (Session["UserId"] == null) 
                return Json(new { ok = false, msg = "Debe iniciar sesión" });

            if (String.IsNullOrWhiteSpace(tarea.Titulo) || String.IsNullOrWhiteSpace(tarea.Descripcion))
                return Json(new { ok = false, msg = "Datos requeridos" });

            var list = TareaDao.Leer();
            tarea.Id = (list.Count == 0 ? 1 : list.Max(t => t.Id) + 1);
            tarea.AutorId = (String)Session["UserId"] ?? "anon";
            tarea.Fecha = DateTime.Now;

            list.Add(tarea);
            TareaDao.Escribir(list);

            return Json(new { ok = true, id = tarea.Id });
        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            var tarea = TareaDao.Leer().FirstOrDefault(t => t.Id == id);
            if (tarea == null) 
                return HttpNotFound();

            var rs = EvaluacionDao.Leer().Where(r => r.TareaId == id).ToList();
            ViewBag.Promedio = rs.Count == 0 ? 0 : rs.Average(r => r.Puntuacion);
            ViewBag.Cant = rs.Count;
            ViewBag.Comentarios = rs.OrderByDescending(r => r.Fecha).ToList();


            return View(tarea);
        }

        [HttpPost]
        public JsonResult RateAjax(int tareaId, int puntuacion, string comentario)
        {
            try
            {
                if (Session["UserId"] == null) return Json(new { ok = false, msg = "Debe iniciar sesión" });
                if (puntuacion < 1 || puntuacion > 5) return Json(new { ok = false, msg = "Puntuación inválida" });

                var list = EvaluacionDao.Leer();
                var item = new Evaluacion
                {
                    Id = (list.Count == 0 ? 1 : list.Max(x => x.Id) + 1),
                    TareaId = tareaId,
                    UsuarioId = (string)Session["UserId"],
                    Puntuacion = puntuacion,
                    Comentario = comentario ?? "",
                    Fecha = DateTime.Now
                };
                list.Add(item);
                EvaluacionDao.Escribir(list);

                var rs = list.Where(x => x.TareaId == tareaId).ToList();
                var prom = rs.Count == 0 ? 0 : rs.Average(x => x.Puntuacion);
                return Json(new { ok = true, promedio = prom, cant = rs.Count });
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, msg = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["UserId"] == null) 
                return RedirectToAction("Login", "Login");
            var list = TareaDao.Leer();
            var t = list.FirstOrDefault(x => x.Id == id);
            if (t == null) 
                return HttpNotFound();
            if ((string)Session["UserId"] != t.AutorId) 
                return new HttpStatusCodeResult(403);
            return View(t);
        }

        [HttpPost]
        public ActionResult Edit(Tarea m)
        {
            if (Session["UserId"] == null) 
                return RedirectToAction("Login", "Login");
            var list = TareaDao.Leer();
            var t = list.FirstOrDefault(x => x.Id == m.Id);
            if (t == null) 
                return HttpNotFound();
            if ((string)Session["UserId"] != t.AutorId) 
                return new HttpStatusCodeResult(403);

            t.Titulo = m.Titulo; 
            t.Descripcion = m.Descripcion;
            t.Lenguajes = m.Lenguajes; 
            t.UrlRepo = m.UrlRepo; 
            t.Categoria = m.Categoria;
            TareaDao.Escribir(list);
            return RedirectToAction("Index", "Tarea");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (Session["UserId"] == null) 
                return RedirectToAction("Login", "Login");
            var t = TareaDao.Leer().FirstOrDefault(x => x.Id == id);
            if (t == null) 
                return HttpNotFound();
            if ((string)Session["UserId"] != t.AutorId) 
                return new HttpStatusCodeResult(403);
            return View(t); // vista de confirmación
        }

        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["UserId"] == null) 
                return RedirectToAction("Login", "Login");
            var list = TareaDao.Leer();
            var t = list.FirstOrDefault(x => x.Id == id);
            if (t == null) 
                return HttpNotFound();
            if ((string)Session["UserId"] != t.AutorId) 
                return new HttpStatusCodeResult(403);

            list.Remove(t);
            TareaDao.Escribir(list);
            return RedirectToAction("Index");
        }


    }
}