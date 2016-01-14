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

            foreach(Platform p in WM.PlatformList)
            {
                //add slide
                Point pl = new Point(WM, p.xPos - (p.Width/2) , p.yPos + (p.Height/2));
                Point pr = new Point(WM, p.xPos + (p.Width / 2), p.yPos + (p.Height / 2));
            }
            /* if(!(WM.Matrix.check(pl,0))){
                var temp = M.getNextPos(pl, Left);
                if(pl != temp){
                   OpenPoints.Add(pl);
                }
            }

            if(!(WM.Matrix.check(pr,0))){
                var temp = M.getNextPos(pr, Right);
                if(pr != temp){
                   OpenPoints.Add(pr);
                }
            }
            */

            foreach(Collectible c in WM.CollectibleList)
            {
                Point pd = new Point(WM, c.xPos, c.yPos);
                OpenPoints.Add(pd);
            }

            throw new NotImplementedException();
        }

        public void GenerateConnections()
        {

            throw new NotImplementedException();
        }

    }
}
