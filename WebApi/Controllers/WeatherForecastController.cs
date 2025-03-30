using System;
using Dominio;
using Persistencia;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        public readonly CursosOnlineContext context;
        public WeatherForecastController(CursosOnlineContext _context){
            this.context = _context;
        }

        [HttpGet]
        public IEnumerable<Curso> Get(){
            return context.Curso.ToList();
        }
    }
}
