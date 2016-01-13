using GeometryFriendsAgents.Model;
using System;
using System.Collections.Generic;

namespace GeometryFriendsAgents.ProblemDectection
{
    public class ProblemDectectionAlgorithm
    {
        public WorldModel WM { get; private set; }
        public List<Point> OpenPoints { get; private set; }

        public ProblemDectectionAlgorithm(WorldModel WM)
        {
            this.WM = WM;
            this.OpenPoints = new List<Point>();
        }
        
        public void GeneratePoints()
        {

            throw new NotImplementedException();
        }

        public void GenerateConnections()
        {

            throw new NotImplementedException();
        }

    }
}
