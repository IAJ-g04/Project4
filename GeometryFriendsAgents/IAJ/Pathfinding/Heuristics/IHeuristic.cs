using GeometryFriendsAgents.Model;

namespace GeometryFriendsAgents.Pathfinding.Heuristics
{
    public interface IHeuristic
    {
        float H(NodeRecord node);
    }
}
