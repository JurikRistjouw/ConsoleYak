using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace YakHerd
{
    public class LabYak
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("age", DataType = "decimal")]
        public decimal Age { get; set; }
        [XmlAttribute("sex")]
        public string Sex { get; set; }  // I wasn't aware that male yaks can produce milk...
        private int AgeinDays { get => (int)(Age * 100); }
        private int daysNotShaven = 0;

        public decimal MilkAmount { get => Age < 10 ? 50 - AgeinDays * (decimal)0.03 : 0; }
        private decimal CalculateBeShavenEvery { get => (Age >= 1 && Age <= (decimal)10.00) ? (8 + AgeinDays * (decimal)0.01) : -1; }

        private int canBeShavenEvery;
        public bool CanBeShaven { get => daysNotShaven == 0 || (daysNotShaven % (canBeShavenEvery + 1) == 0); }
        public LabYak()
        {
            canBeShavenEvery = 1; // day one every yak can be shaved
        }

        public decimal LastAgeShaved { get => (AgeinDays - daysNotShaven) * (decimal)0.01; }

        public void DailyUpdate()
        {
            Age += (decimal)0.01;
            if (CanBeShaven)
            {
                canBeShavenEvery = (int)decimal.Truncate(CalculateBeShavenEvery);
            }
            if (daysNotShaven++ > canBeShavenEvery) daysNotShaven = 0;
        }
    }

    [XmlRootAttribute("herd")]
    public class Herd : IHerd
    {
        [XmlElement("labyak")]
        public LabYak[] LabYaks { get; set; }
        public int Hides { get; set; }
        public decimal Milk { get; set; }

        public void CalculateHerd(int Days)
        {
            for (int x = 0; x < Days; x++)
            {
                Milk += LabYaks.Sum(o => o.MilkAmount);
                Hides += LabYaks.Count(o => o.CanBeShaven);
                DailyUpdateYaks();
            }
        }

        public void DailyUpdateYaks()
        {
            foreach (var yak in LabYaks)
            {
                yak.DailyUpdate();
            }
        }

        public static Herd ReadHerd(string fileName)
        {
            var herd = new Herd();

            using (TextReader reader = new StreamReader(fileName))
            {
                var serializer = new XmlSerializer(typeof(Herd));

                herd = (Herd)serializer.Deserialize(reader);
            }

            return herd;
        }
    }
}
