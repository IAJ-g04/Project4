﻿using GeometryFriendsAgents.Model;
using System;

namespace GeometryFriendsAgents.Pathfinding
{
    public enum NodeStatus
    {
        Unvisited,
        Open,
        Closed
    }

    public class NodeRecord  : IComparable<NodeRecord>
    {
        public Point node;
        public NodeRecord parent;
        public Connection parentConnection;
        public int Points;
        public float gValue;
        public float hValue;
        public float fValue;
        public NodeStatus status;

        public int CompareTo(NodeRecord other)
        {
            return this.fValue.CompareTo(other.fValue);
        }

        //two node records are equal if they refer to the same node
        public override bool Equals(object obj)
        {
            var target = obj as NodeRecord;
            if (target != null) return this.node == target.node;
            else return false;
        }

        public override int GetHashCode()
        {
            return this.node.GetHashCode();
        }
    }
}
