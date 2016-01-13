using System.Collections.Generic;

namespace GeometryFriendsAgents.Model
{
    public class LevelMatrix : WorldModelComponent
    {
        public int[,] Matrix { private set; get; }
        public float Heigth { private set; get; }
        public float Width { private set; get; }

        public LevelMatrix(WorldModel WM, float heigth, float width) : base(WM)
        {
            this.Width = width;
            this.Heigth = heigth;
        }

    }
}
