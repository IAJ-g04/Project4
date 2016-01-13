using GeometryFriendsAgents.Model;

namespace GeometryFriendsAgents.Pathfinding.Heuristics
{
    public class EuclideanDistanceHeuristic : IHeuristic
    {
        public float H(Point node, Point goalNode)
        {
            return node.DistanceTo(goalNode);
        }
    }
}
