
using PackmanOrigin.Interfaces.BaseClasses;

namespace PackmanOrigin.GameClasses
{
    class Coin : ObjectToCollect
    {
        public const int point = 1;
        public static int TotalCoins { get; set; }
        public Coin(double leftLocation, double topLocation, string imagePath = "/Assets/Images/coinImage.png")
            : base(leftLocation, topLocation, imagePath)
        {
            TotalCoins++;
        }
    }
}
