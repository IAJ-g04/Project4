using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryFriendsAgents.Model{
    public class RectangleCharacter : WorldObject{

        public float heigth { private set; get; }
        public float width { private set; get; }

        public RectangleCharacter(WorldModel WM, float xPos, float yPos) : base(WM, xPos, yPos)
        {
        }

        public RectangleCharacter(WorldModel WM, float xPos, float yPos, float h, float w) : base(WM, xPos, yPos)
        {
            this.heigth = h;
            this.width = w;
        }

    }
}
