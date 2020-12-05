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

        //[Route("{reg_id}")]
        [HttpGet]
        public IActionResult RegSettings(string reg_id)
        {
            User usuario = HttpContext.Session.Get<User>("UsuarioLogueado");
            if(usuario != null){
                int registro_id = JsonConvert.DeserializeObject<int>(reg_id);
                Registro registro = db.Registros.FirstOrDefault(r => r.ID == registro_id && r.CreadorID == usuario.ID);
                if(registro != null){
                    string admins = registro.Administradores;
                    if(admins != null){
                        List<int> ids = JsonConvert.DeserializeObject<int[]>(admins).ToList();
                        registro.Admins = new List<UserEssentials>();
                        foreach(int id in ids){
                            User admin = db.Users.FirstOrDefault(u => u.ID == id);
                            registro.Admins.Add(new UserEssentials(){
                                ID = admin.ID,
                                Username = admin.UserName,
                                Mail = admin.Mail
                            });
                        }
                    }
                    ViewBag.registro = registro;
                    return View("RegistroConfig");
                }else{
                    ViewBag.badReg = true;
                    return VistaLogueado(usuario.ID, usuario.UserName);
                }
            }else{
                return View("Index");
            }
        }

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

                return VistaLogueado(userCheck.ID, userCheck.UserName);
            }
            else
            {
                ViewBag.badLogin = true;
                return View("Index");
            }
        }

        public IActionResult AgregarAdministrador(string reg_id, string admin_mail){
            User usuario = HttpContext.Session.Get<User>("UsuarioLogueado");
            bool sameMail = false;
            if(admin_mail == usuario.Mail) sameMail = true;
            User newAdmin = db.Users.FirstOrDefault(u => u.Mail == admin_mail);
            
            if(usuario != null && newAdmin != null && !sameMail){
                int registro_id = JsonConvert.DeserializeObject<int>(reg_id);
                Registro registro = db.Registros.FirstOrDefault(r => r.ID == registro_id && r.CreadorID == usuario.ID);
                if(registro != null){
                    Solicitud solicitud = new Solicitud(){
                        From = usuario.ID,
                        FromName = usuario.UserName,
                        To = newAdmin.ID,
                        ToName = newAdmin.UserName,
                        RelatedReg = registro_id,
                        Action = "AddAdmin"
                    };

                    Solicitud copia = db.Solicitudes.FirstOrDefault(s => s.From == solicitud.From && s.To == solicitud.To && s.Action == solicitud.Action && s.RelatedReg == solicitud.RelatedReg);
                    
                    if(copia == null){
                        db.Solicitudes.Add(solicitud);
                        db.SaveChanges();
                        ViewBag.AddAdmin = true;
                    }else{
                        ViewBag.AddAdmin = false;
                    }
                    ViewBag.newAdmin = admin_mail;
                    
                }else{
                    ViewBag.NoReg = false;
                }

                return VistaLogueado(usuario.ID, usuario.UserName);
            }else{
                if(usuario == null) {
                    return View("Index");
                }else{
                    if(sameMail) ViewBag.autoAdmin = true;
                    if(newAdmin == null) ViewBag.NoMail = true;
                    return VistaLogueado(usuario.ID, usuario.UserName);
                }
            }        
        }

        public IActionResult AceptarSoli(int soli_id){
            User usuario = HttpContext.Session.Get<User>("UsuarioLogueado");
            if(usuario != null){
                Solicitud solicitud = db.Solicitudes.FirstOrDefault(s => s.ID == soli_id && s.To == usuario.ID);
                if(solicitud != null){
                    bool check = AgregarAdmin(solicitud.To, solicitud.RelatedReg, solicitud.From);
                    if(check){
                        db.Solicitudes.Remove(solicitud);
                        db.SaveChanges();
                        ViewBag.aceptarSoli = true;
                        return VistaLogueado(usuario.ID, usuario.UserName);
                    }else{
                        ViewBag.aceptarSoli = false;
                        return VistaLogueado(usuario.ID, usuario.UserName);
                    }
                }else{
                    ViewBag.aceptarSoli = false;
                    return VistaLogueado(usuario.ID, usuario.UserName);
                }
            }else{
                return View("Index");
            }
        }

        public IActionResult CancelarSoli(int soli_id){
            User usuario = HttpContext.Session.Get<User>("UsuarioLogueado");
            if(usuario != null){
                Solicitud solicitud = db.Solicitudes.FirstOrDefault(s => s.ID == soli_id);
                if(solicitud != null){
                    db.Solicitudes.Remove(solicitud);
                    db.SaveChanges();
                    ViewBag.cancelSoli = true;
                    return VistaLogueado(usuario.ID, usuario.UserName);
                }else{
                    ViewBag.cancelSoli = false;
                    return VistaLogueado(usuario.ID, usuario.UserName);
                }
            }else{
                return View("Index");
            }
        }

        public IActionResult EliminarAdministrador(string reg_id, string admin_id){
            User usuario = HttpContext.Session.Get<User>("UsuarioLogueado");
            if(usuario != null){
                int _idParsed= JsonConvert.DeserializeObject<int>(admin_id);
                User admin = db.Users.FirstOrDefault(u => u.ID == _idParsed);
                if(admin != null){
                    int registro_id = JsonConvert.DeserializeObject<int>(reg_id);
                    Registro registro = db.Registros.FirstOrDefault(r => r.ID == registro_id);
                    if(registro != null){
                        bool check = EliminarAdmin(_idParsed, registro_id, usuario.ID);
                        if(check){
                            ViewBag.adminDeleted = true;
                        }else{
                            ViewBag.adminDeleted = false;
                        }
                        return VistaLogueado(usuario.ID, usuario.UserName);
                    }else{
                        ViewBag.badReg = true;
                        return VistaLogueado(usuario.ID, usuario.UserName);
                    }
                }else {
                    ViewBag.aceptarSoli = false;
                    return VistaLogueado(usuario.ID, usuario.UserName);
                }
            }else return View("Index");
        }

        public ViewResult VistaLogueado(int _id, string name){
            ViewBag.Creador = RegistrosCreador(_id);
            ViewBag.Admin = RegistrosAdmin(_id);
            ViewBag.Solicitudes = MisSolicitudes(_id);
            ViewBag.username = name;

            return View("Logueado");
        }
        private bool AgregarAdmin(int admin_id, int reg_id, int creador_id){
            if(admin_id == creador_id) return false;
            Registro registro = db.Registros.FirstOrDefault(r => r.ID == reg_id && r.CreadorID == creador_id);
            if(registro != null){
                User newAdmin = db.Users.FirstOrDefault(u => u.ID == admin_id);
                if(newAdmin != null){
                    string admins = registro.Administradores;
                    List<int> admins_ids;
                    if(admins != null){
                        admins_ids = JsonConvert.DeserializeObject<int[]>(admins).ToList();
                        if(!admins_ids.Contains(newAdmin.ID)) admins_ids.Add(newAdmin.ID);
                    }else{
                        admins_ids = new List<int>();
                        admins_ids.Add(newAdmin.ID);
                    }
                    registro.Administradores = JsonConvert.SerializeObject(admins_ids);
                    db.Registros.Update(registro);
                    db.SaveChanges();
                    return true;
                }else{
                    return false;
                }
            }else{
                return false;
            }
        }

        private bool EliminarAdmin(int admin_id, int reg_id, int creador_id){
            if(admin_id == creador_id) return false;
            Registro registro = db.Registros.FirstOrDefault(r => r.ID == reg_id && r.CreadorID == creador_id);
            if(registro != null){
                User admin = db.Users.FirstOrDefault(u => u.ID == admin_id);
                if(admin != null){
                    List<int> admins = JsonConvert.DeserializeObject<int[]>(registro.Administradores).ToList();
                    admins.Remove(admin_id);
                    registro.Administradores = JsonConvert.SerializeObject(admins);
                    db.Registros.Update(registro);
                    db.SaveChanges();
                    return true;
                }else return false;
            }else return false;
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
                return VistaLogueado(usuario.ID, usuario.UserName);
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

            return VistaLogueado(usuario.ID, usuario.UserName);
        }

        public IActionResult PausarLogs(string reg_ids){
            List<int> ids = JsonConvert.DeserializeObject<int[]>(reg_ids).ToList();
            User usuario = HttpContext.Session.Get<User>("UsuarioLogueado");

            pausaLogs(ids, usuario);

            return VistaLogueado(usuario.ID, usuario.UserName);
        }

        public void pausaLogs(List<int> ids, User usuario){
            if(ids.Count() == 1){
                Registro reg = db.Registros.FirstOrDefault(r => r.ID == ids[0] && (r.CreadorID == usuario.ID || r.Administradores.Contains($"{usuario.ID}")));
                if(reg != null){
                    reg.pauseLogs = reg.pauseLogs ? false : true;
                    db.Registros.Update(reg);
                    db.SaveChanges();
                }
                return;
            }

            int i;
            for(i = 0; i < ids.Count(); i++){
                Registro reg = db.Registros.FirstOrDefault(r => r.ID == ids[i] && (r.CreadorID == usuario.ID || r.Administradores.Contains($"{usuario.ID}")));
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

            return VistaLogueado(usuario.ID, usuario.UserName);
        }
        public void pausaRegs(List<int> ids, User usuario){
            if(ids.Count() == 1){
                Registro reg = db.Registros.FirstOrDefault(r => r.ID == ids[0] && (r.CreadorID == usuario.ID || r.Administradores.Contains($"{usuario.ID}")));
                if(reg != null){
                    reg.pauseRegs = reg.pauseRegs ? false : true;
                    db.Registros.Update(reg);
                    db.SaveChanges();
                }

                return;
            }

            int i;
            for(i = 0; i < ids.Count(); i++){
                Registro reg = db.Registros.FirstOrDefault(r => r.ID == ids[i] && (r.CreadorID == usuario.ID || r.Administradores.Contains($"{usuario.ID}")));
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

            return VistaLogueado(usuario.ID, usuario.UserName);
        }

        public List<Registro> RegistrosCreador(int _id){
            List<Registro> registrosCreador = db.Registros.Where(r => r.CreadorID == _id).ToList();
            foreach(Registro registro in registrosCreador){
                if(registro.Administradores != null){
                    registro.Admins = new List<UserEssentials>();
                    List<int> ids = JsonConvert.DeserializeObject<int[]>(registro.Administradores).ToList();
                    foreach(int id in ids){
                        User admin = db.Users.FirstOrDefault(u => u.ID == id);
                        registro.Admins.Add(new UserEssentials(){
                            ID = admin.ID,
                            Username = admin.UserName,
                            Mail = admin.UserName
                        });
                    }
                    
                }
            }
            return registrosCreador;
        }
        public List<Registro> RegistrosAdmin(int _id){
            List<Registro> registrosAdmin = db.Registros.Where(r => r.Administradores.Contains($"{_id}")).ToList();

            return registrosAdmin;
        }

        private List<Solicitud> MisSolicitudes(int _id){
            List<Solicitud> solicitudes = db.Solicitudes.Where(s => s.From == _id || s.To == _id).ToList();

            return solicitudes;    
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
            if(usuario != null) {
                return VistaLogueado(usuario.ID, usuario.UserName);
            }
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
