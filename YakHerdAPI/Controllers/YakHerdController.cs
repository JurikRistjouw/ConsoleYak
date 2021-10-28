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
        private readonly IHerd yakHerd;

        public YakHerdController()
        {
            yakHerd = YakHerd.Herd.ReadHerd(@"Z:\Sollicitatie TalentIncubator\ConsoleYak\herd.xml");
        }

        public YakHerdController(IHerd herd)
        {
            yakHerd = herd;
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
            // output in swagger is also shaven, the output of decimals is omitted if not needed apparently, see raw output for decimal digits.
            public decimal AgeLastShaved { get; set; }
        }

        [HttpGet("herd/{T}")]
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

        [HttpGet("stock/{T}")]
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

        [HttpPost("order/{T}/{order}")]
        public ActionResult<StockFormat> Order(int T, OrderFormat order)
        {
            yakHerd.CalculateHerd(T);

            var milk = order.Order.Milk;
            var skins = order.Order.Skins;

            if (yakHerd.Milk >= milk || yakHerd.Hides >= skins)  
            {
                var ret = new StockFormat
                {
                    Milk = yakHerd.Milk >= milk ? milk : 0,
                    Skins = yakHerd.Hides >= skins ? skins : 0
                };

                if (ret.Milk == 0 || ret.Skins == 0)
                {
                    return StatusCode(206, ret);
                }

                return Created("order created", ret);
            }

            return NotFound();
        }
    }
}
