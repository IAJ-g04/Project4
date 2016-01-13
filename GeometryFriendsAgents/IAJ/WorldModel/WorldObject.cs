using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryFriendsAgents.Model {
   public abstract class WorldObject : WorldModelComponent {

        public WorldObject(WorldModel WM) : base(WM)
        {
        }

        public WorldObject(WorldModel WM, float x, float y) : base(WM)
        {
            this.xPos = x;
            this.yPos = y;
        }

        public float xPos { private set; get; }

        public float yPos { private set; get; }

    }
}
