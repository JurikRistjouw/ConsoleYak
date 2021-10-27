using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using YakHerd;

namespace YakHerdAPI.Controllers
{
    [Route("yak-shop/")]
    [ApiController]
    public class YakHerdController : ControllerBase
    {
        private readonly Herd yakHerd;

        public YakHerdController()
        {
            yakHerd = YakHerd.Herd.ReadHerd(@"Z:\Sollicitatie TalentIncubator\ConsoleYak\herd.xml");
        }

        public class HerdWrapper
        {
            public List<HerdFormat> Herd { get; set; }
        }

        public class HerdFormat
        {
            public string Name { get; set; }
            public decimal Age { get; set; }
            [JsonPropertyName("age-last-shaved")]
            public decimal AgeLastShaved { get; set; }
        }

        [HttpPost("herd/{T}")]
        public HerdWrapper Herd(int T)
        {
            yakHerd.CalculateHerd(T);

            var ret = new HerdWrapper
            {
                Herd = new List<HerdFormat>()
            };

            foreach (LabYak l in yakHerd.LabYaks)
            {
                var format = new HerdFormat
                {
                    Age = l.Age,
                    AgeLastShaved = l.LastAgeShaved,
                    Name = l.Name
                };

                ret.Herd.Add(format);
            }

            return ret;
        }


        public class StockFormat
        {
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public decimal Milk { get; set; }
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public int Skins { get; set; }
        }

        [HttpPost("stock/{T}")]
        public StockFormat Stock(int T)
        {
            yakHerd.CalculateHerd(T);


            return new StockFormat
            {
                Milk = yakHerd.Milk,
                Skins = yakHerd.Hides
            };
        }

        public class OrderFormat
        {
            public string Customer { get; set; }
            public StockFormat Order { get; set; }
        }

        [HttpPost("order/{T}/{customer}/{skins}/{milk}")]
        public ActionResult<StockFormat> Order(int T, string customer, int skins, decimal milk)
        {
            yakHerd.CalculateHerd(T);

            if (yakHerd.Milk >= (decimal)milk || yakHerd.Hides >= skins)  
            {
                var ret = new StockFormat
                {
                    Milk = yakHerd.Milk >= (decimal)milk ? milk : (decimal)0,
                    Skins = yakHerd.Hides >= skins ? skins : 0
                };

                if (yakHerd.Milk == 0 || yakHerd.Hides == 0)
                {
                    return Created("partial", ret);
                    // return this.Request.CreateResponse<OrderFormat>(HttpStatusCode.Partial, ret);
                }

                return Created("", ret);
                // TODO: URI and partial result 206 and partial json without omitted orderpart
            }

            return NotFound();
        }
    }
}
