namespace PackmanOrigin.GameClasses.Enemies
{
    public class EnemiesDirection
    {
        public bool IsEnemyCanGo { get; set; }
        public EnemiesDirection(DirectionE direction)
        {
            Direction = direction;
        }
        public DirectionE Direction { get;}
    }

    public enum DirectionE
    {
        none,
        Right,
        Left,
        Down,
        Up
    }
}
