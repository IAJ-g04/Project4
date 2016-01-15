
using GeometryFriendsAgents.Model;

namespace GeometryFriendsAgents.Pathfinding.Heuristics
{
    public class ZeroHeuristic : IHeuristic
    {
        public float H(NodeRecord node)
        {
            return node.node.WM.NumberOfCollectibles - node.Points;
        }
    }
}
