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
                this.OpenPoints.Add(pd);
            }
        }

        public void GenerateConnections()
        {
            while(OpenPoints.Count != 0)
            {
                /*Point op = OpenPoints.getBestAndRemove();
                Point nop = OpenPoints.getBest();

                //Right
                if (op.yPos == nop.yPos)
                {
                    //slide
                    if ((op.xPos - nop.xPos) <= 1 * this.WM.Matrix.WORLD_UNIT_SIZE)
                    {
                        op.addConnection(new Connection(WM, op, nop));
                        nop.addConnection(new Connection(WM, nop, op));
                    }
                    //fall slide
                    else if ((op.xPos - nop.xPos) <= 3*this.WM.Matrix.WORLD_UNIT_SIZE)
                    {
                        op.addConnection(new Connection(WM, op, nop));
                        nop.addConnection(new Connection(WM, nop, op));
                    }
                }

                //Down
                Point pf = this.WM.Matrix.GenerateNewPoint(op);

                //stair or gem TODO
                if((op.yPos - pf.yPos) <= 2 * this.WM.Matrix.WORLD_UNIT_SIZE)
                {
                    op.addConnection(new Connection(WM, op, pf));
                    pf.addConnection(new Connection(WM, pf, op));
                }
                //fall
                else
                {
                    op.addConnection(new Connection(WM, op, pf));
                }
                this.OpenPoints.Add(pf);*/
            }
        }
        
    }
}
