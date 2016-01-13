
using GeometryFriendsAgents.Model;
using GeometryFriendsAgents.Pathfinding.DataStructures;
using GeometryFriendsAgents.Pathfinding.Heuristics;
using System;

namespace GeometryFriendsAgents.Pathfinding
{
    public class AStarPathfinding : WorldModelComponent
    {
        //how many nodes do we process on each call to the search method
        public uint NodesPerSearch { get; set; }

        public uint TotalProcessedNodes { get; protected set; }
        public int MaxOpenNodes { get; protected set; }
        public bool InProgress { get; protected set; }

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

        public void InitializePathfindingSearch(Point startPosition, Point goalPosition)
        {
            this.StartNode = startPosition;
            this.GoalNode = goalPosition;
            

            this.InProgress = true;
            this.TotalProcessedNodes = 0;
            this.MaxOpenNodes = 0;

            var initialNode = new NodeRecord
            {
                gValue = 0,
                hValue = this.Heuristic.H(this.StartNode, this.GoalNode),
                node = this.StartNode
            };

            initialNode.fValue = AStarPathfinding.F(initialNode);

            this.Open.Initialize(); 
            this.Open.AddToOpen(initialNode);
            this.Closed.Initialize();
        }

        public virtual bool Search(bool returnPartialSolution = false)
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
            if (returnPartialSolution)
            {
                var bestNodeSoFar = this.Open.PeekBest();
            }
            else
            {
            }
            return false;
        }

        protected virtual NodeRecord GenerateChildNodeRecord(NodeRecord parent, Connection connectionEdge)
        {
            var childNodeRecord = new NodeRecord
            {
                node = connectionEdge.Destination,
                parent = parent,
                gValue = parent.gValue + connectionEdge.Destination.DistanceTo(parent.node),
                hValue = this.Heuristic.H(connectionEdge.Destination, this.GoalNode)
            };

            childNodeRecord.fValue = F(childNodeRecord);

            return childNodeRecord;
        }

        protected void CalculateSolution(NodeRecord node, bool partial)
        {

            throw new NotImplementedException();
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
