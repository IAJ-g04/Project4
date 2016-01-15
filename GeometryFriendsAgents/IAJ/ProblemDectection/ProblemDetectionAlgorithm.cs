using GeometryFriendsAgents.Model;
using System;
using System.Collections.Generic;

namespace GeometryFriendsAgents.ProblemDectection
{
    public class ProblemDectectionAlgorithm
    {
        public WorldModel WM { get; private set; }
        public LeftPriorityList OpenPoints { get; private set; }

        public ProblemDectectionAlgorithm(WorldModel WM)
        {
            this.WM = WM;
            this.OpenPoints = new LeftPriorityList();
        }

        public void GeneratePoints()
        {

            foreach (Platform p in WM.PlatformList.Values)
            {
                Point pl = this.WM.Matrix.GenerateNewPoint(p, this.WM.Matrix.LEFT);
                if (pl != null)
                    this.OpenPoints.AddToOpen(pl);

                Point pr = this.WM.Matrix.GenerateNewPoint(p, this.WM.Matrix.RIGHT);
                if (pr != null)
                    this.OpenPoints.AddToOpen(pr);
            }


            foreach (Collectible c in WM.CollectibleList.Values)
            {
                Point pd = new Point(WM, c.xPos, c.yPos);
                this.OpenPoints.AddToOpen(pd);
            }
        }

        public void GenerateConnections()
        {

            this.WM.Mesh = new Point[this.OpenPoints.All().Count];

            int pos = 0;
            while(this.OpenPoints.All().Count != 1)
            {
                Point op = this.OpenPoints.GetBestAndRemove();
                Point nop = this.OpenPoints.PeekBest();

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
                if (this.WM.Matrix.CheckPointForFalling(op))
                {
                    Point pf = this.WM.Matrix.GenerateNewPointFalling(op, this.WM.Matrix.DOWN);
                    if (WM.Matrix.CheckDownForStar(op))
                    {
                        Point ps = WM.Matrix.GenerateNewPointFallingStar(op);
                        if ((op.yPos - pf.yPos) <= 2 * this.WM.Matrix.WORLD_UNIT_SIZE)
                        {
                            op.addConnection(new Connection(WM, op, ps));
                            ps.addConnection(new Connection(WM, ps, op));
                            ps.addConnection(new Connection(WM, ps, pf));
                            pf.addConnection(new Connection(WM, pf, ps));
                        }
                        //fall
                        else
                        {
                            op.addConnection(new Connection(WM, op, ps));
                            ps.addConnection(new Connection(WM, ps, pf));
                        }

                    }
                    else{ 
                    //stair or gem TODO
                        if ((op.yPos - pf.yPos) <= 2 * this.WM.Matrix.WORLD_UNIT_SIZE)
                        {
                            op.addConnection(new Connection(WM, op, pf));
                            pf.addConnection(new Connection(WM, pf, op));
                        }
                        //fall
                        else
                        {
                            op.addConnection(new Connection(WM, op, pf));
                        }
                    }
                    this.OpenPoints.AddToOpen(pf);
                }
                WM.Mesh[pos] = op;
            }
        }
        
    }
}
