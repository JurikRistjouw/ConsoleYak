using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using YakHerd;

namespace YakHerdAPI.Controllers
{
    [Route("yak-shop/")]
    [ApiController]
    public class YakHerdController : ControllerBase
    {
        // TODO: figure out entrypoint to / instead of ?

        private readonly Herd yakHerd;

        public YakHerdController()
        {
            yakHerd = YakHerd.Herd.ReadHerd(@"Z:\Sollicitatie TalentIncubator\ConsoleYak\herd.xml");
        }


        public class HerdFormat
        {
            public string Name { get; set; }
            public decimal Age { get; set; }
            // TODO: change parametername in output
            public decimal AgeLastShaved { get; set; }
        }

        [HttpGet("herd/")]
        public List<HerdFormat> Herd(int daysPast)
        {
            yakHerd.CalculateHerd(daysPast);

            var ret = new List<HerdFormat>();

            foreach (LabYak l in yakHerd.LabYaks)
            {
                var format = new HerdFormat
                {
                    Age = l.Age,
                    AgeLastShaved = l.LastAgeShaved,
                    Name = l.Name
                };

                ret.Add(format);
            }
            
            return ret;
        }


        public class StockFormat
        {
            public decimal Milk { get; set; }
            public int Skins { get; set; }
        }

        [HttpGet("stock/")]
        public StockFormat Stock(int T)
        {
            yakHerd.CalculateHerd(T);


            return new StockFormat
            {
                Milk = yakHerd.Milk,
                Skins = yakHerd.Hides
            }; 
        }


    }
}
