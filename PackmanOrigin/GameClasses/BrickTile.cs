using PackmanOrigin.Classes;

namespace PackmanOrigin.GameClasses
{
    class BrickTile : TileObject
    {
        public BrickTile(double leftLocation, double topLocatin, int columnInd, int rowInd, string imagePath = "/Assets/Images/Bricks.jpg") 
            : base(leftLocation, topLocatin, columnInd,rowInd,imagePath)
        {
        }
    }
}
