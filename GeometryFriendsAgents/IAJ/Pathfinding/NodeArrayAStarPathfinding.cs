using GeometryFriendsAgents.Model;
using GeometryFriendsAgents.Pathfinding.DataStructures;
using GeometryFriendsAgents.Pathfinding.Heuristics;
using System.Collections.Generic;

namespace GeometryFriendsAgents.Pathfinding
{
    public class NodeArrayAStarPathFinding : AStarPathfinding
    {
        protected NodeRecordArray NodeRecordArray { get; set; }
        public NodeArrayAStarPathFinding(WorldModel WM, IHeuristic heuristic) : base(WM,null,null,heuristic)
        {
            this.NodeRecordArray = new NodeRecordArray(WM);
            this.Open = this.NodeRecordArray;
            this.Closed = this.NodeRecordArray;
        }

        protected void ProcessChildNode(NodeRecord bestNode, Connection connectionEdge)
        {
            float f;
            float g;
            float h;

            var childNode = connectionEdge.Destination;
            var childNodeRecord = this.NodeRecordArray.GetNodeRecord(childNode);

            if (childNodeRecord == null)
            {
                //this piece of code is used just because of the special start nodes and goal nodes added to the RAIN Navigation graph when a new search is performed.
                //Since these special goals were not in the original navigation graph, they will not be stored in the NodeRecordArray and we will have to add them
                //to a special structure
                //it's ok if you don't understand this, this is a hack and not part of the NodeArrayA* algorithm
                childNodeRecord = new NodeRecord
                {
                    node = childNode,
                    parent = bestNode,
                    status = NodeStatus.Unvisited
                };
                this.NodeRecordArray.AddSpecialCaseNode(childNodeRecord);
            }


            // implement the rest of your code here

            if (childNodeRecord.status == NodeStatus.Closed) return;

            g = bestNode.gValue;
            h = this.Heuristic.H(childNode, this.GoalNode);
            f = F(g,h);

            if (childNodeRecord.status == NodeStatus.Open)
            {
                if (f <= childNodeRecord.fValue)
                {
                    childNodeRecord.gValue = g;
                    childNodeRecord.hValue = h;
                    childNodeRecord.fValue = f;
                    childNodeRecord.parent = bestNode;
                    this.NodeRecordArray.Replace(childNodeRecord,childNodeRecord);
                }
            }
            else
            {
                childNodeRecord.gValue = g;
                childNodeRecord.hValue = h;
                childNodeRecord.fValue = f;
                childNodeRecord.status = NodeStatus.Open;
				childNodeRecord.parent = bestNode;
                this.NodeRecordArray.AddToOpen(childNodeRecord);
            }
        }

        public override bool Search(bool returnPartialSolution = false)
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

                var bestNode = this.NodeRecordArray.GetBestAndRemove();

                //goal node found, return the shortest Path
                if (bestNode.node == this.GoalNode)
                {
                    this.InProgress = false;
                    return true;
                }

                this.NodeRecordArray.AddToClosed(bestNode);

                processedNodes++;
                this.TotalProcessedNodes++;


                //put your code here 

                //or if you would like, you can change just these lines of code this in the original A* Pathfinding Class, 
                //create a ProcessChildNode method in the base class with the code from the previous A* algorithm.
                //if you do this, then you don't need to implement this search method method. Don't forget to override the ProcessChildMethod if you do this
                var outConnections = bestNode.node.ConnectionList.Count;
                for (int i = 0; i < outConnections; i++)
                {
                    this.ProcessChildNode(bestNode,bestNode.node.ConnectionList[i]);
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


    }
}
