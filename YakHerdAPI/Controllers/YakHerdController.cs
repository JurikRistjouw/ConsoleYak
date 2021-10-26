﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;
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
            public decimal Milk { get; set; }
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
    }
}
