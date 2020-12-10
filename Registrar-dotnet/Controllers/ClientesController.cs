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
using System.Web;
using System.IO;

//Popups de 360 x 640
//Medida standard de celulares

namespace Registrar_dotnet.Controllers
{
    public class ClientesController : Controller
    {
        public IActionResult LoginCliente(int reg_id, int creator_id){
            Registro registro = db.Registros.FirstOrDefault(r => r.ID == reg_id && r.CreadorID == creator_id);
            if(registro != null){
                if(registro.pauseLogs) ViewBag.noLogin = true;
                return View();
            }else{
                return View("PopUpErrorView");
            }
        }
        public IActionResult RegistroCliente(int reg_id, int creator_id){
            Registro registro = db.Registros.FirstOrDefault(r => r.ID == reg_id && r.CreadorID == creator_id);
            if(registro != null){
                if(registro.pauseRegs) ViewBag.noRegs = true;
                return View();
            }else{
                return View("PopUpErrorView");
            }
        }

        [HttpPost]
        public JsonResult LoginFinal(int reg_id, int creator_id, string username, string password){
            Registro registro = db.Registros.FirstOrDefault(r => r.ID == reg_id && r.CreadorID == creator_id);
            if(registro != null){
                UsuarioFinal usuario = db.UsuariosFinales.FirstOrDefault(u => (u.UserName == username || u.Mail == username) && u.Password == password);
                if(usuario != null){
                    string URL = String.Format($"https://session-controller.herokuapp.com/newsession/{usuario.UserName}");
                    WebRequest newUserLogged = WebRequest.Create(URL);
                    newUserLogged.Method = "POST";
                    newUserLogged.Headers.Add("api-key", "salamesalame");
                    using (var writer = new StreamWriter(newUserLogged.GetRequestStream())){
                        writer.Write("");
                        writer.Flush();
                        writer.Close();
                        var httpResponse = (HttpWebResponse)newUserLogged.GetResponse();
                        using (var reader = new StreamReader(httpResponse.GetResponseStream())){
                            string response = reader.ReadToEnd();
                            reader.Close();
                            return Json(response);
                        }
                    }
                }else{
                    return Json("NoUser");
                }
            }else{
                return Json("BadReg");
            }
        }

        public IActionResult RegistroFinal(int reg_id, int creator_id, string username, string email, string password, string password2){
            Registro registro = db.Registros.FirstOrDefault(r => r.ID == reg_id && r.CreadorID == creator_id);
            if(registro != null){
                UsuarioFinal userCheck = db.UsuariosFinales.FirstOrDefault(u => u.RegistroID == reg_id && (u.UserName == username || u.Mail == email));
                if(userCheck == null){
                    if(password == password2){
                        UsuarioFinal usuario = new UsuarioFinal(){
                            Mail = email,
                            UserName = username,
                            Password = password,
                            EmailVerif = false,
                            RegistroID = reg_id
                        };
                        db.UsuariosFinales.Add(usuario);
                        db.SaveChanges();
                        ViewBag.registrado = true;
                        if(registro.pauseLogs) ViewBag.noLogin = true;
                        return View("LoginCliente");
                    }else{
                        ViewBag.notSamePass = true;
                        return View("RegistroCliente");
                    }
                }else{
                    if(userCheck.UserName == username) ViewBag.notUser = true;
                    else ViewBag.notMail = true;
                    return View("RegistroCliente");
                }
            }else{
                ViewBag.badReg = true;
                return View("RegistroCliente");
            }
        }

        private readonly UserContext db;
        private readonly ILogger<ClientesController> _logger;
        public ClientesController(ILogger<ClientesController> logger, UserContext context){
            _logger = logger;
            db = context;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}