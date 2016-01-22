﻿using GeometryFriendsAgents.Model;
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
                {
                    this.OpenPoints.AddToOpen(pl);
                }

                Point pr = this.WM.Matrix.GenerateNewPoint(p, this.WM.Matrix.RIGHT);
                if (pr != null)
                {
                    this.OpenPoints.AddToOpen(pr);
                }
            }


            foreach (Collectible c in WM.CollectibleList.Values)
            {
                Point pd = new Point(WM, c.xPos, c.yPos);
                pd.categorie = pd.STAR_POINT;
                pd.side = pd.UP;
                this.OpenPoints.AddToOpen(pd);
            }
        }

        public void GenerateConnections()
        {
            List<Point> tempMesh = new List<Point>();
            
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
                                if (op.DistanceToInX(nop) <= 3 * this.WM.Matrix.WORLD_UNIT_SIZE)
                                {
                                    Connection c = new Connection(WM, op, nop);
                                    c.categorie = c.SLIDEONHOLE;
                                    op.addConnection(c);
                                    c = new Connection(WM, nop, op);
                                    c.categorie = c.SLIDEONHOLE;
                                    nop.addConnection(c);
                                }
                            }
                            else
                            {
                                Connection c = new Connection(WM, op, nop);
                                c.categorie = c.SLIDEONPLATFORM;
                                op.addConnection(c);
                                c = new Connection(WM, nop, op);
                                c.categorie = c.SLIDEONPLATFORM;
                                nop.addConnection(c);
                            }

                        }

                    }
                    if (this.WM.Matrix.IsCollectible(op))
                    {
                        if (!this.WM.Matrix.CheckPlatformBeneath(op))
                        {

                            Point pf = this.WM.Matrix.GenerateNewPointFalling(op, this.WM.Matrix.DOWN);
                            if (op.DistanceToInY(pf) <= 4 * this.WM.Matrix.WORLD_UNIT_SIZE)
                            {
                                Connection c = new Connection(WM, op, pf);
                                c.categorie = c.FALLING;
                                op.addConnection(c);
                                c = new Connection(WM, pf, op);
                                c.categorie = c.GRAB;
                                pf.addConnection(c);
                                pf.categorie = pf.GRAB_POINT;
                            }
                            else
                            {
                                Connection c = new Connection(WM, op, pf);
                                c.categorie = c.FALLING;
                                op.addConnection(new Connection(WM, op, pf));
                            }
                            this.OpenPoints.AddToOpen(pf);

                        }
                    }

                    else if (this.WM.Matrix.CheckPointForFalling(op))
                    {

                        Point pf = this.WM.Matrix.GenerateNewPointFalling(op, this.WM.Matrix.DOWN);
                        if (WM.Matrix.CheckDownForStar(op))
                        {
                            Point ps = WM.Matrix.GenerateNewPointFallingStar(op);
                            if (op.DistanceToInY(pf) >= 3 * this.WM.Matrix.WORLD_UNIT_SIZE)
                            {
                                ps = OpenPoints.SearchInOpen(ps);
                                Connection c = new Connection(WM, op, ps);
                                c.categorie = c.FALLING;
                                op.addConnection(c);
                              
                            }
                            else
                            {
                                    Connection c = new Connection(WM, op, pf);
                                    c.categorie = c.FALLING;
                                    op.addConnection(c);
                                    c = new Connection(WM, pf, op);
                                    c.categorie = c.CLIMB;
                                    pf.categorie = pf.STAIR_POINT;
                                    pf.addConnection(c);
                            }
                        }
                        else
                        {
                            //stair or gem TODO
                            if (op.DistanceToInY(pf) <= 3 * this.WM.Matrix.WORLD_UNIT_SIZE)
                            {
                                Connection c = new Connection(WM, op, pf);
                                c.categorie = c.FALLING;
                                op.addConnection(c);
                                c = new Connection(WM, pf, op);
                                c.categorie = c.CLIMB;
                                pf.categorie = pf.STAIR_POINT;
                                pf.addConnection(c);
                            }
                            //fall
                            else
                            {
                                Connection c = new Connection(WM, op, pf);
                                c.categorie = c.FALLING;
                                op.addConnection(c);
                            }
                        }
                        op.categorie = op.CLIFF_POINT;
                        this.OpenPoints.AddToOpen(pf);
                    }
                }
                tempMesh.Add(op);
            }
            
            this.WM.Mesh = tempMesh.ToArray();
        }
        
    }
}
