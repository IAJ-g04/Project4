
using GeometryFriendsAgents.Model;
using GeometryFriendsAgents.Pathfinding.DataStructures;
using GeometryFriendsAgents.Pathfinding.Heuristics;
using System;
using System.Collections.Generic;

namespace GeometryFriendsAgents.Pathfinding
{
    public class AStarPathfinding : WorldModelComponent
    {
        //how many nodes do we process on each call to the search method
        public uint NodesPerSearch { get; set; }

        public uint TotalProcessedNodes { get; protected set; }
        public int MaxOpenNodes { get; protected set; }
        public bool InProgress { get; protected set; }
        public NodeRecord initialNode { get; protected set; }

        public IOpenSet Open { get; protected set; }
        public IClosedSet Closed { get; protected set; }

        public Point GoalNode { get; protected set; }
        public Point StartNode { get; protected set; }

        //heuristic function
        public IHeuristic Heuristic { get; protected set; }

        public AStarPathfinding(WorldModel WM, IOpenSet open, IClosedSet closed, IHeuristic heuristic) : base(WM)
        {
            this.Open = open;
            this.Closed = closed;
            this.NodesPerSearch = uint.MaxValue; //by default we process all nodes in a single request
            this.InProgress = false;
            this.Heuristic = heuristic;
        }

        public void InitializePathfindingSearch(RectangleCharacter startPosition)
        {
            Point initp = new Point(WM, WM.Character.xPos, WM.Character.yPos);
            Point aux = new Point(WM, 9999, 9999);
            float dist = 99999999999;
            foreach (Point p in WM.Mesh)
            {
                float auxd = initp.DistanceTo(p);
                if (auxd <= dist)
                {
                    aux = p;
                    dist = auxd;
                }
            }
            Connection c = new Connection(WM, initp, aux);
            c.categorie = c.SLIDEONPLATFORM;
            if (initp.xPos > aux.xPos)
            {
                c.side = c.LEFT;
            }
            else
            {
                c.side = c.RIGHT;
            }
            initp.addConnection(c);
            NodeRecord nri = new NodeRecord();
            nri.node = initp;
            nri.gValue = 0;
            nri.hValue = 0;
            nri.fValue = 0;
            nri.Points = 0;
            var bestNode = nri;
            this.StartNode = initp;

            

            this.InProgress = true;
            this.TotalProcessedNodes = 0;
            this.MaxOpenNodes = 0;
            

            this.Open.Initialize();

            this.Open.AddToOpen(bestNode);
            this.Closed.Initialize();
        }

        public virtual bool Search()
        {
            var processedNodes = 0;
            int count;

            while (processedNodes < this.NodesPerSearch)
            {
                count = this.Open.CountOpen();
                if (count == 0)
                {
                    
                    this.InProgress = false;
                    return true;
                }

                if (count > this.MaxOpenNodes)
                {
                    this.MaxOpenNodes = count;
                }

                var bestNode = this.Open.GetBestAndRemove();

                //goal node found, return the shortest Path
                if (bestNode.node == this.GoalNode)
                {
                    this.InProgress = false;
                    return true;
                }

                this.Closed.AddToClosed(bestNode);
                processedNodes++;
                this.TotalProcessedNodes++;

                var outConnections = bestNode.node.ConnectionList.Count;
                for (int i = 0; i < outConnections; i++)
                {
                    var childNode = GenerateChildNodeRecord(bestNode, bestNode.node.ConnectionList[i]);
                    var closedSearch = this.Closed.SearchInClosed(childNode);
                    if (closedSearch != null) continue;

                    var openSearch = this.Open.SearchInOpen(childNode);
                    if (openSearch != null)
                    {
                        if (childNode.fValue <= openSearch.fValue)
                        {
                            this.Open.Replace(openSearch, childNode);    
                        }
                    }
                    else
                    {
                        this.Open.AddToOpen(childNode);
                    }
                }
            }
            

            //this is very unlikely but it might happen that we process all nodes alowed in this cycle but there are no more nodes to process
            if (this.Open.CountOpen() == 0)
            {
                this.InProgress = false;
                return true;
            }

            //if the caller wants create a partial Path to reach the current best node so far
            
            return false;
        }

        protected virtual NodeRecord GenerateChildNodeRecord(NodeRecord parent, Connection connectionEdge)
        {
            var childNodeRecord = new NodeRecord
            {
                node = connectionEdge.Destination,
                parent = parent,
                gValue = parent.gValue + connectionEdge.Destination.DistanceTo(parent.node)
            };

            childNodeRecord.hValue = this.Heuristic.H(childNodeRecord);
            childNodeRecord.fValue = F(childNodeRecord);

            return childNodeRecord;
        }

        protected void CalculateSolution(NodeRecord node)
        {
            List<NodeRecord> plist = new List<NodeRecord>();
            var currentNode = node;

            plist.Add(node);
            if (currentNode.parent != null)
            {
                currentNode = currentNode.parent;
            }

            while (currentNode.parent != null)
            {
                plist.Add(currentNode); //we need to reverse the list because this operator add elements to the end of the list

                if (currentNode.parent.parent == null) break; //this skips the first node
                currentNode = currentNode.parent;
            }

            plist.Reverse();
            var first = plist[0];
            this.WM.Path = new Connection[plist.Count-1];
            int i = 0;
            foreach (NodeRecord nr in plist)
            {
                //ConsolePrinter.PrintLine("aqui: " + nr.parentConnection.Destination.xPos);
                if(!first.Equals(nr))
                {

                    this.WM.Path[i] = nr.parentConnection;
                    i++;
                }
            }
            this.InProgress = false;
        }

        public static float F(NodeRecord node)
        {
            return F(node.gValue,node.hValue);
        }

        public static float F(float g, float h)
        {
            return g + h;
        }

    }
}
