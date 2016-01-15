using GeometryFriendsAgents.Model;
using System.Collections.Generic;

namespace GeometryFriendsAgents.ProblemDectection
{
    public interface IOpenSet
    {
        void Initialize();
        void Replace(Point nodeToBeReplaced, Point nodeToReplace);
        Point GetBestAndRemove();
        Point PeekBest();
        void AddToOpen(Point nodeRecord);
        void RemoveFromOpen(Point nodeRecord);
        //should return null if the node is not found
        Point SearchInOpen(Point nodeRecord);
        ICollection<Point> All();
        int CountOpen();
    }
}
