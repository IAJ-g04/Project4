using GeometryFriendsAgents.Model;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GeometryFriendsAgents.ProblemDectection
{
    public class LeftPriorityList : IOpenSet
    {
        private List<Point> Open { get; set; }

        public LeftPriorityList()
        {
            this.Open = new List<Point>();    
        }
        public void Initialize()
        {
            this.Open.Clear();
        }

        public void Replace(Point nodeToBeReplaced, Point nodeToReplace)
        {
            this.Open.Remove(nodeToBeReplaced);
            this.AddToOpen(nodeToReplace);
        }

        public Point GetBestAndRemove()
        {
            var best = this.Open[0];
            this.Open.RemoveAt(0);
            return best;
        }

        public Point PeekBest()
        {
            return this.Open[0];
        }

        public void AddToOpen(Point nodeRecord)
        {
            int index = this.Open.BinarySearch(nodeRecord);
            if (index < 0)
            {
                this.Open.Insert(~index, nodeRecord);
            }
        }

        public void RemoveFromOpen(Point nodeRecord)
        {
            this.Open.Remove(nodeRecord);
        }

        public Point SearchInOpen(Point nodeRecord)
        {
            return this.Open.FirstOrDefault(n => n.Equals(nodeRecord));
        }

        public ICollection<Point> All()
        {
            return this.Open;
        }

        public int CountOpen()
        {
            return this.Open.Count;
        }
    }
}
