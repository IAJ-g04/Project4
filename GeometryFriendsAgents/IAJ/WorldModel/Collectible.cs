using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryFriendsAgents.Model{
    public class Collectible : WorldObject, IComparable {

        public Collectible(WorldModel WM, float xPos, float yPos) : base(WM, xPos, yPos)
        {

        }

        public double Distance { get; set; }

        public int CompareTo(object obj) {
            Collectible col = (Collectible)obj;
            if (Distance < col.Distance) {
                return -1;
            } else if (Distance > col.Distance) {
                return 1;
            } else {
                return 0;
            }
        }
    }
}
