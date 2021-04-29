using PackmanOrigin.Classes;

namespace PackmanOrigin.GameClasses
{
    internal class BlackTile : TileObject
    {
       
        public BlackTile(double leftLocation, double topLocatin, int columnInd, int rowInd, string imagePath = "/Assets/Images/blackImage.jpg")
           : base(leftLocation, topLocatin,  columnInd,  rowInd ,imagePath)
        {
           
        }
    }
}
