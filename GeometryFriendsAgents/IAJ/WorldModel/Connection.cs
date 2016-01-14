using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryFriendsAgents.Model
{
    public class Connection : WorldModelComponent
    {
        public int FALLING = 1;
        public int SLIDEONPLATFORM = 2;
        public int SLIDEONHOLE = 3;
        public int GRAB = 4;
        public int CLIMB = 5;

        public int categorie;

        public int LEFT = 1;
        public int UP = 2;
        public int RIGHT = 3;
        public int DOWN = 4;

        public int side;

        public Point Origin { get; private set; }
        public Point Destination { get; private set; }

        public Connection(WorldModel WM, Point origin, Point destination) : base (WM)
        {
            this.Origin = origin;
            this.Destination = destination;
        }

    }
}
