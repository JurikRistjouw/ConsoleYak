using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YakHerd;

namespace YakHerdAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YakHerdController : ControllerBase
    {

        private readonly Herd yakHerd;

        public YakHerdController()
        {
            yakHerd = Herd.ReadHerd(@"Z:\Sollicitatie TalentIncubator\ConsoleYak\herd.xml");
        }

        [HttpGet]
        public IEnumerable<LabYak> GetYaks()
        {
            return yakHerd.LabYaks;
        }
    }
}
