using System;
using System.Collections;
using System.Collections.Generic;

namespace GeometryFriendsAgents.Model
{
    public class Point : WorldObject
    {
        public List<Connection> ConnectionList;

        public Point(WorldModel WM, float x, float y) : base (WM, x, y)
        {
            this.ConnectionList = new List<Connection>();
        }

        public bool existConnection(Connection con)
        {
            throw new NotImplementedException();
        }

        public void addConnection(Connection con)
        {
            throw new NotImplementedException();
        }

        public void removeConnection(Connection con)
        {
            throw new NotImplementedException();
        }

        public float DistanceTo(Point otherPoint)
        {
            throw new NotImplementedException();
        }
    }
}
