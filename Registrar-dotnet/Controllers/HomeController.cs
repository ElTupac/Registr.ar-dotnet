using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Registrar_dotnet.Models;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Registrar_dotnet.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserContext db;

        [HttpPost]
        public IActionResult RegisterUser(string username, string email, string password)
        {
            //TODO
            //Encriptar las contrasenias y guardarlas encriptadas(Opcional mas adelante)
            //Crear campo de mail verificado, enviar mail con link de activacion
            //Link de activacion tiene que cambiar la propiedad para que pueda loguear

            User userCheck = db.Users.FirstOrDefault(n => n.Mail == email || n.UserName == username);
            if(userCheck == null)
            {
                User nuevoUser = new User(){
                    Mail = email,
                    UserName = username,
                    Password = password,
                    EmailVerif = false
                };
                db.Users.Add(nuevoUser);
                db.SaveChanges();

                return View("CheckMail");
            }
            else if(userCheck.UserName == username)
            {
                ViewBag.usernameUsed = true;
                ViewBag.email = email;
                return View("Index");
            }
            else if(userCheck.Mail == email)
            {
                ViewBag.mailUsed = true;
                ViewBag.username = username;
                return View("Index");
            }
            else
            {
                return View("ErrorView");
            }

            
        }

        [HttpPost]
        public IActionResult LoginUser(string username, string password)
        {
            //TODO
            //Checkear que conincida la contrasenia
            //Checkear que haya verficado el mail
            User userCheck = db.Users.FirstOrDefault(n => (n.Mail == username || n.UserName == username) && n.Password == password);
            if(userCheck != null)
            {
                AgregarUsuario(userCheck);

                ViewBag.username = userCheck.UserName;

                return View("Logueado", RegistrosDe(userCheck.ID));
            }
            else
            {
                ViewBag.badLogin = true;
                return View("Index");
            }
        }

        public IActionResult CrearRegistro(string nombre)
        {
            if(nombre != null){
                User usuario = HttpContext.Session.Get<User>("UsuarioLogueado");

                Registro nuevoRegistro = new Registro{
                    Nombre = nombre,
                    CreadorID = usuario.ID,
                    Premium = false
                };

                db.Registros.Add(nuevoRegistro);
                db.SaveChanges();
                return View("Logueado", RegistrosDe(usuario.ID));
            }else{
                //Por alguna razon no llego el nombre del nuevo registro
                return View("ErrorView");
            }
        }

        public IActionResult EliminarRegistros(string reg_ids){
            List<int> ids = JsonConvert.DeserializeObject<int[]>(reg_ids).ToList();
            User usuario = HttpContext.Session.Get<User>("UsuarioLogueado");
            
            int i;
            for(i = 0; i < ids.Count(); i++){
                Registro reg = db.Registros.FirstOrDefault(r => r.ID == ids[i] && r.CreadorID == usuario.ID);
                if(reg != null) db.Registros.Remove(reg);
            }
            db.SaveChanges();
            
            return View("Logueado", RegistrosDe(usuario.ID));
        }

        public IActionResult PausarLogs(string reg_ids){
            List<int> ids = JsonConvert.DeserializeObject<int[]>(reg_ids).ToList();
            User usuario = HttpContext.Session.Get<User>("UsuarioLogueado");

            pausaLogs(ids, usuario);

            return View("Logueado", RegistrosDe(usuario.ID));
        }

        public void pausaLogs(List<int> ids, User usuario){
            if(ids.Count() == 1){
                Registro reg = db.Registros.FirstOrDefault(r => r.ID == ids[0] && r.CreadorID == usuario.ID);
                if(reg != null){
                    reg.pauseLogs = reg.pauseLogs ? false : true;
                    db.Registros.Update(reg);
                    db.SaveChanges();
                }
                return;
            }

            int i;
            for(i = 0; i < ids.Count(); i++){
                Registro reg = db.Registros.FirstOrDefault(r => r.ID == ids[i] && r.CreadorID == usuario.ID);
                if(reg != null) {
                    reg.pauseLogs = true;
                    db.Registros.Update(reg);
                }
            }
            db.SaveChanges();
        }

        public IActionResult PausarRegs(string reg_ids){
            List<int> ids = JsonConvert.DeserializeObject<int[]>(reg_ids).ToList();
            User usuario = HttpContext.Session.Get<User>("UsuarioLogueado");

            pausaRegs(ids, usuario);

            return View("Logueado", RegistrosDe(usuario.ID));
        }
        public void pausaRegs(List<int> ids, User usuario){
            if(ids.Count() == 1){
                Registro reg = db.Registros.FirstOrDefault(r => r.ID == ids[0] && r.CreadorID == usuario.ID);
                if(reg != null){
                    reg.pauseRegs = reg.pauseRegs ? false : true;
                    db.Registros.Update(reg);
                    db.SaveChanges();
                }

                return;
            }

            int i;
            for(i = 0; i < ids.Count(); i++){
                Registro reg = db.Registros.FirstOrDefault(r => r.ID == ids[i] && r.CreadorID == usuario.ID);
                if(reg != null) {
                    reg.pauseRegs = true;
                    db.Registros.Update(reg);
                }
            }
            db.SaveChanges();
        }

        public IActionResult PausarTodo(string reg_ids){
            List<int> ids = JsonConvert.DeserializeObject<int[]>(reg_ids).ToList();
            User usuario = HttpContext.Session.Get<User>("UsuarioLogueado");

            pausaLogs(ids, usuario);
            pausaRegs(ids, usuario);

            return View("Logueado", RegistrosDe(usuario.ID));
        }

        public List<Registro> RegistrosDe(int _id){
            List<Registro> registros = db.Registros.Where(r => r.CreadorID == _id).ToList();
            return registros;
        }

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, UserContext context)
        {
            _logger = logger;
            db = context;
        }

        public IActionResult Index()
        {
            User usuario = HttpContext.Session.Get<User>("UsuarioLogueado");
            if(usuario != null) return View("Logueado", RegistrosDe(usuario.ID));
            else return View();
        }

        public JsonResult AgregarUsuario(User usuario)
        {
            HttpContext.Session.Set<User>("UsuarioLogueado", usuario);
            return Json(usuario);
        }

        public JsonResult ConsultarUsuario()
        {
            User usuario = HttpContext.Session.Get<User>("UsuarioLogueado");
            return Json(usuario);
        }

        public IActionResult SacarUsuario()
        {
            HttpContext.Session.Remove("UsuarioLogueado");
            return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
