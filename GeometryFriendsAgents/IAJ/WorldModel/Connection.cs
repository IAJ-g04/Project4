using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryFriendsAgents.Model
{
    public class Connection : WorldModelComponent
    {
        public Point Origin { get; private set; }
        public Point Destination { get; private set; }

        public Connection(WorldModel WM, Point origin, Point destination) : base (WM)
        {
            this.Origin = origin;
            this.Destination = destination;
        }

    }
}
