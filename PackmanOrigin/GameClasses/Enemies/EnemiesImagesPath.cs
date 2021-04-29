namespace PackmanOrigin.GameClasses.Enemies
{
    class EnemiesImagesPath
    {
        public readonly string[] ImagePath =
        {
            "/Assets/Images/greyGhost (2).jpg", "/Assets/Images/InkyBlue (3).jpg",
            "/Assets/Images/PinkyPink (2).jpg", "/Assets/Images/OrangeLyde (2).jpg" 
         };


        public string this[int index] { get { return ImagePath[index]; } }
    }
}
