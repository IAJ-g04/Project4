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

            this.xMatrix = (int)Math.Round((xPos * this.WM.Matrix.Matrix_Width) / this.WM.Matrix.World_Width);
            this.yMatrix = (int)Math.Round((yPos * this.WM.Matrix.Matrix_Height) / this.WM.Matrix.World_Height);
        }

        protected float _xPos;

        public float xPos
        {
            set
            {
                _xPos = value;
                this.xMatrix = (int)Math.Round((_xPos * this.WM.Matrix.Matrix_Width) / this.WM.Matrix.World_Width);
            }
            get { return _xPos; }
        }

        protected float _yPos;

        public float yPos
        {
            set
            {
                _yPos = value;
                this.yMatrix = (int)Math.Round((_yPos * this.WM.Matrix.Matrix_Height) / this.WM.Matrix.World_Height);
            }
            get { return _yPos; }
        }

        public int xMatrix { set; get; }

        public int yMatrix {set; get; }

        public override bool Equals(object obj)
        {
            WorldObject wo = (WorldObject)obj;
            return (this.xPos == wo.xPos) &&
                      (this.yPos == wo.yPos);
        }


    }
}
