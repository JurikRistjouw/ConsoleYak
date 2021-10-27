namespace YakHerd
{
    public interface IHerd
    {
        int Hides { get; set; }
        LabYak[] LabYaks { get; set; }
        decimal Milk { get; set; }

        void CalculateHerd(int Days);
        void DailyUpdateYaks();
    }
}