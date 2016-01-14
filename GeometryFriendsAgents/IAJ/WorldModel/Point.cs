﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace GeometryFriendsAgents.Model
{
    public class Point : WorldObject, IComparable
    {
        public int CLIFF_POINT = 1;
        public int FALLING_POINT = 2;
        public int STAIR_POINT = 3;
        public int WALL_POINT = 4;

        public int categorie;

        public int LEFT = 1;
        public int UP = 2;
        public int RIGHT = 3;
        public int DOWN = 4;

        public int side;
        public List<Connection> ConnectionList;

        public Point(WorldModel WM, float x, float y) : base (WM, x, y)
        {
            this.ConnectionList = new List<Connection>();
        }

        public Point(WorldModel WM, int side, float x, float y) : base(WM, x, y)
        {
            this.side = side;
            this.ConnectionList = new List<Connection>();
        }

        public Point(WorldModel WM, int categorie, int side, float x, float y) : base(WM, x, y)
        {
            this.categorie = categorie;
            this.side = side;
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

        
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        public static implicit operator Point(RectangleCharacter v)
        {
            return new Point(v.WM, v.xPos, v.yPos);
        }
    }
}
