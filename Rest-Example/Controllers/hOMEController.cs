using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Rest_Example.Controllers
{
    public class HomeController : ApiController
    {


        /// <summary>
        /// Metodo Saluar
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns>saludo</returns>
        [Route("api/home/saludar")]
        [HttpGet]
        public string Saludar(string nombre)
        {
            return string.Format("Hola {0}", nombre);
        }

    }
}
