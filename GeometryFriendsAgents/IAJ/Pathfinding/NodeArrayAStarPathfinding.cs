using GeometryFriendsAgents.Model;
using GeometryFriendsAgents.Pathfinding.DataStructures;
using GeometryFriendsAgents.Pathfinding.Heuristics;
using System.Collections.Generic;

namespace GeometryFriendsAgents.Pathfinding
{
    public class NodeArrayAStarPathFinding : AStarPathfinding
    {
        protected NodeRecordArray NodeRecordArray { get; set; }
        protected List<NodeRecord> lstars { get; set; }
        public NodeArrayAStarPathFinding(WorldModel WM, IHeuristic heuristic) : base(WM,null,null,heuristic)
        {
            this.NodeRecordArray = new NodeRecordArray(WM);
            this.lstars = new List<NodeRecord>();
            this.Open = this.NodeRecordArray;
            this.Closed = this.NodeRecordArray;
        }

        protected void ProcessChildNode(NodeRecord bestNode, Connection connectionEdge)
        {
            float f;
            float g;
            float h;

            var childNode = connectionEdge.Destination;
            //var childNodeRecord = this.NodeRecordArray.GetNodeRecord(childNode);

            var childNodeRecord = new NodeRecord();
            childNodeRecord.node = childNode;
            childNodeRecord.parent = bestNode;
            childNodeRecord.parentConnection = connectionEdge;
            if (childNode.categorie == childNode.STAR_POINT)
            {
                int i = 0;
                foreach(NodeRecord nr in lstars)
                {
                    if (childNodeRecord.Equals(nr))
                    {
                        i = -1;
                        break;
                    }
                    i++;
                }
                if(i >= 0) {
                    lstars.Add(childNodeRecord);
                    childNodeRecord.Points = i+1;
                }
                else
                {
                    childNodeRecord.Points = childNodeRecord.parent.Points;
                }
               
            }
            else
            {
                childNodeRecord.Points = childNodeRecord.parent.Points;
            }



            // implement the rest of your code here

            if (childNodeRecord.status == NodeStatus.Closed) return;

            g = bestNode.gValue +1;
            h = this.Heuristic.H(childNodeRecord);
            f = F(g,h);

            if (childNodeRecord.status == NodeStatus.Open)
            {
                if (f <= childNodeRecord.fValue)
                {
                    childNodeRecord.gValue = g;
                    childNodeRecord.hValue = h;
                    childNodeRecord.fValue = f;
                    this.NodeRecordArray.Replace(childNodeRecord,childNodeRecord);
                }
            }
            else
            {
                childNodeRecord.gValue = g;
                childNodeRecord.hValue = h;
                childNodeRecord.fValue = f;
                childNodeRecord.status = NodeStatus.Open;
                this.NodeRecordArray.AddToOpen(childNodeRecord);
            }
        }

        public override bool Search()
        {
            var bestNode = this.NodeRecordArray.PeekBest();
            while (bestNode.Points < WM.NumberOfCollectibles){

                if (this.Open.CountOpen() == 0)
                {
                    //ConsolePrinter.PrintLine("YOYO");
                    this.CalculateSolution(bestNode);
                    //ConsolePrinter.PrintLine("Zero cenas");
                    return true;
                }

                bestNode = this.NodeRecordArray.GetBestAndRemove();

                //goal node found, return the shortest Path
                /*if (bestNode.node.Points == WM.CollectibleList.Count)
                {
                    this.CalculateSolution(bestNode);
                    this.InProgress = false;
                    return true;
                }*/

                this.NodeRecordArray.AddToClosed(bestNode);

                this.TotalProcessedNodes++;
                //put your code here 

                //or if you would like, you can change just these lines of code this in the original A* Pathfinding Class, 
                //create a ProcessChildNode method in the base class with the code from the previous A* algorithm.
                //if you do this, then you don't need to implement this search method method. Don't forget to override the ProcessChildMethod if you do this
                var outConnections = bestNode.node.ConnectionList.Count;
                for (int i = 0; i < outConnections; i++)
                {
                    this.ProcessChildNode(bestNode, bestNode.node.ConnectionList[i]);
                }
            }
            //ConsolePrinter.PrintLine("YOYO");

            //ConsolePrinter.PrintLine("All Collectibles");
            this.CalculateSolution(bestNode);
            return true;
        }


    }
}
