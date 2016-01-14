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

            foreach (Platform p in WM.PlatformList.Values)
            {
                //add slide
                Point pl = this.WM.Matrix.GenerateNewPoint(p, this.WM.Matrix.LEFT);
                if (pl != null)
                    this.OpenPoints.Add(pl);

                Point pr = this.WM.Matrix.GenerateNewPoint(p, this.WM.Matrix.RIGHT);
                if (pr != null)
                    this.OpenPoints.Add(pr);
            }


            foreach (Collectible c in WM.CollectibleList)
            {
                Point pd = new Point(WM, c.xPos, c.yPos);
                OpenPoints.Add(pd);
            }
        }

        public void GenerateConnections()
        {

            throw new NotImplementedException();
        }

    }
}
