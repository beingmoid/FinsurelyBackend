using Microsoft.AspNetCore.Mvc;
using PanoramBackend.Data;
using PanoramBackend.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PanoramaBackend.Api.Controllers
{
    public class HomeController : Controller
    {
        private AMFContext _context;

        public HomeController(AMFContext context)
        {
            _context = context;
        }
        [Route("/header")]
        [HttpGet]
        public IActionResult Index([FromQuery]int id, [FromQuery] string from, [FromQuery] string to, [FromQuery] string Balance)
        {
            var agent = _context.Set<UserDetails>().SingleOrDefault(x =>x.Id == id);
            var statemenmtPDF = new AccountStatementPDF();
            statemenmtPDF.AccountTRN = agent.Id.ToString() + "/" + agent.CreateTime?.ToBinary().ToString() + "/" + DateTime.Now.Ticks;
            statemenmtPDF.AgentName = agent.DisplayNameAs;
            statemenmtPDF.DateFrom = from;
            statemenmtPDF.PhoneNumber = agent.Phone;
            statemenmtPDF.DateTo = to;
            statemenmtPDF.Country = "United Arab Emirates";
            statemenmtPDF.Emirates = "Dubai";
            statemenmtPDF.Balance = Balance;
            return View(statemenmtPDF);
        }
    }
}
