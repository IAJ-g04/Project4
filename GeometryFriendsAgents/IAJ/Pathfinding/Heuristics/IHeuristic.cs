using GeometryFriendsAgents.Model;

namespace GeometryFriendsAgents.Pathfinding.Heuristics
{
    public interface IHeuristic
    {
        float H(Point node, Point goalNode);
    }
}
