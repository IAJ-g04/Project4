
using GeometryFriendsAgents.Model;

namespace GeometryFriendsAgents.Pathfinding.Heuristics
{
    public class ZeroHeuristic : IHeuristic
    {
        public float H(Point node, Point goalNode)
        {
            return 0;
        }
    }
}
