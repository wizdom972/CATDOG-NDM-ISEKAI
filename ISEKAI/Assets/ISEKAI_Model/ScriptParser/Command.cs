namespace ISEKAI_Model
{
    public enum SpriteLocation
    {
        None = 0, Left = 1, Center = 2, Right = 3
    }
    public abstract class Command
    {
        public abstract int commandNumber {get;}

        public (int, int) choiceDependency {get; set;} // (-1, -1) if there is no choice dependency. (-1, n) if there is dependency on recent choice, which resulted 'n'.
                                                       // (n, m) if there is dependency on n'th choice, which resulted 'm'.

        public Command()
        {
            choiceDependency = (-1, -1);
        }
    }
}