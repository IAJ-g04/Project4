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
                ConsolePrinter.PrintLine("Passei aqui" + p.ID);
                Point pl = this.WM.Matrix.GenerateNewPoint(p, this.WM.Matrix.LEFT);
                if (pl != null)
                {
                    ConsolePrinter.PrintLine("Passei aquiL");
                    this.OpenPoints.AddToOpen(pl);
                }

                Point pr = this.WM.Matrix.GenerateNewPoint(p, this.WM.Matrix.RIGHT);
                if (pr != null)
                {
                    ConsolePrinter.PrintLine("Passei aquiR");
                    this.OpenPoints.AddToOpen(pr);
                }
            }


            foreach (Collectible c in WM.CollectibleList.Values)
            {
                ConsolePrinter.PrintLine("passei aqui" + c.ID);
                Point pd = new Point(WM, c.xPos, c.yPos);
                pd.categorie = pd.STAIR_POINT;
                pd.side = pd.UP;
                this.OpenPoints.AddToOpen(pd);
            }
        }

        public void GenerateConnections()
        {

            this.WM.Mesh = new Point[this.OpenPoints.All().Count];

            int pos = 0;
            while (this.OpenPoints.All().Count != 0)
            {
                Point op = this.OpenPoints.GetBestAndRemove();
                if (this.OpenPoints.All().Count == 0)
                {

                }
                else
                {
                    Point nop = this.OpenPoints.PeekBest();

                    //Right
                    if (op.yMatrix == nop.yMatrix)
                    {
                        if (!this.WM.Matrix.CheckWallBetween(op, nop))
                        {
                            if (this.WM.Matrix.CheckHoleBetween(op, nop))
                            {
                                if ((op.xPos - nop.xPos) <= 3 * this.WM.Matrix.WORLD_UNIT_SIZE)
                                {
                                    Connection c = new Connection(WM, op, nop);
                                    c.categorie = c.SLIDEONHOLE;
                                    c.side = c.RIGHT;
                                    op.addConnection(c);
                                    c = new Connection(WM, nop, op);
                                    c.categorie = c.SLIDEONHOLE;
                                    c.side = c.LEFT;
                                    nop.addConnection(c);
                                }
                            }
                            else
                            {
                                Connection c = new Connection(WM, op, nop);
                                c.categorie = c.SLIDEONPLATFORM;
                                c.side = c.RIGHT;
                                op.addConnection(c);
                                c = new Connection(WM, nop, op);
                                c.categorie = c.SLIDEONPLATFORM;
                                c.side = c.LEFT;
                                nop.addConnection(c);
                            }

                        }

                    }

                    if (this.WM.Matrix.IsCollectible(op))
                    {
                        if (!this.WM.Matrix.CheckPlatformBeneath(op))
                        {
                            Point pf = this.WM.Matrix.GenerateNewPointFalling(op, this.WM.Matrix.DOWN);

                            if ((op.yPos - pf.yPos) <= 4 * this.WM.Matrix.WORLD_UNIT_SIZE)
                            {
                                Connection c = new Connection(WM, op, pf);
                                c.categorie = c.FALLING;
                                c.side = c.DOWN;
                                op.addConnection(c);
                                c = new Connection(WM, pf, op);
                                c.categorie = c.GRAB;
                                c.side = c.UP;
                                pf.addConnection(c);

                            }

                        }
                    }

                    else if (this.WM.Matrix.CheckPointForFalling(op))
                    {
                        Point pf = this.WM.Matrix.GenerateNewPointFalling(op, this.WM.Matrix.DOWN);
                        if (WM.Matrix.CheckDownForStar(op))
                        {
                            Point ps = WM.Matrix.GenerateNewPointFallingStar(op);
                            if ((op.yPos - pf.yPos) >= 3 * this.WM.Matrix.WORLD_UNIT_SIZE)
                            {
                                Connection c = new Connection(WM, op, ps);
                                c.categorie = c.FALLING;
                                c.side = c.DOWN;
                                op.addConnection(c);
                                op.categorie = op.CLIFF_POINT;
                                c = new Connection(WM, ps, pf);
                                c.categorie = c.FALLING;
                                c.side = c.DOWN;
                                ps.addConnection(new Connection(WM, ps, pf));
                            }
                            else
                            {
                                if ((op.yPos - pf.yPos) <= 2 * this.WM.Matrix.WORLD_UNIT_SIZE)
                                {
                                    Connection c = new Connection(WM, op, pf);
                                    c.categorie = c.FALLING;
                                    c.side = c.DOWN;
                                    op.addConnection(c);
                                    op.categorie = op.CLIFF_POINT;
                                    c = new Connection(WM, pf, op);
                                    c.categorie = c.CLIMB;
                                    c.side = c.UP;
                                    pf.addConnection(c);
                                }
                                //fall
                                else
                                {
                                    Connection c = new Connection(WM, op, pf);
                                    c.categorie = c.FALLING;
                                    c.side = c.DOWN;
                                    op.addConnection(c);
                                }
                            }
                        }
                        else
                        {
                            //stair or gem TODO
                            if ((op.yPos - pf.yPos) <= 2 * this.WM.Matrix.WORLD_UNIT_SIZE)
                            {
                                Connection c = new Connection(WM, op, pf);
                                c.categorie = c.FALLING;
                                c.side = c.DOWN;
                                op.addConnection(c);
                                c = new Connection(WM, pf, op);
                                c.categorie = c.CLIMB;
                                c.side = c.UP;
                                pf.addConnection(c);
                            }
                            //fall
                            else
                            {
                                Connection c = new Connection(WM, op, pf);
                                c.categorie = c.FALLING;
                                c.side = c.DOWN;
                                op.addConnection(c);
                            }
                        }
                        this.OpenPoints.AddToOpen(pf);
                    }
                }
                WM.Mesh[pos] = op;
                pos++;
            }
        }
        
    }
}
