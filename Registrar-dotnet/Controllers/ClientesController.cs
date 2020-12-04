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