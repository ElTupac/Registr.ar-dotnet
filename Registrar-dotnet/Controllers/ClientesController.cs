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

//Popups de 360 x 640
//Medida standard de celulares

namespace Registrar_dotnet.Controllers
{
    public class ClientesController : Controller
    {
        public IActionResult LoginCliente(){
            return View();
        }
        public IActionResult RegistroCliente(){
            return View();
        }

        [HttpPost]
        public IActionResult LoginFinal(int reg_id, int creator_id, string username, string password){
            Registro registro = db.Registros.FirstOrDefault(r => r.ID == reg_id && r.CreadorID == creator_id);
            if(registro != null){
                UsuarioFinal usuario = db.UsuariosFinales.FirstOrDefault(u => (u.UserName == username || u.Mail == username) && u.Password == password);
                if(usuario != null){
                    return View("ClienteLogueado");
                }else{
                    ViewBag.badLogin = true;
                    return View("LoginCliente");
                }
            }else{
                ViewBag.badReg = true;
                return View("LoginCliente");
            }
        }

        public IActionResult RegistroFinal(int reg_id, int creator_id, string username, string email, string password, string password2){
            Registro registro = db.Registros.FirstOrDefault(r => r.ID == reg_id && r.CreadorID == creator_id);
            if(registro != null){
                UsuarioFinal userCheck = db.UsuariosFinales.FirstOrDefault(u => u.Mail == email || u.UserName == username);
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
                        return View("LoginCliente");
                    }else{
                        ViewBag.notSamePass = true;
                        return View("RegistroCliente");
                    }
                }else{
                    //El mail o el usuario ya existen
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