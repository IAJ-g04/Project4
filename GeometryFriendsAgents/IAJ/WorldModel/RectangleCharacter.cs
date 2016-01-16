using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryFriendsAgents.Model{
    public class RectangleCharacter : WorldObject{

        public float heigth { set; get; }

        public RectangleCharacter(WorldModel WM, float xPos, float yPos) : base(WM, xPos, yPos)
        {
        }

        public RectangleCharacter(WorldModel WM, float xPos, float yPos, float h) : base(WM, xPos, yPos)
        {
            this.heigth = h;
        }

    }
}
