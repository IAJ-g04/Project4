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

        protected float _xPos;

        public float xPos
        {
            set
            {
                _xPos = value;
                this.xMatrix = (int)Math.Round((_xPos * (this.WM.Matrix.Matrix_Width - 1)) / this.WM.Matrix.World_Width);
                if (this.xMatrix >= this.WM.Matrix.Matrix_Width)
                    this.xMatrix = this.WM.Matrix.Matrix_Width - 1;
            }
            get { return _xPos; }
        }

        protected float _yPos;

        public float yPos
        {
            set
            {
                _yPos = value;
                this.yMatrix = (int)Math.Round((_yPos * (this.WM.Matrix.Matrix_Height - 1)) / this.WM.Matrix.World_Height);
                if (this.yMatrix >= this.WM.Matrix.Matrix_Height)
                    this.yMatrix = this.WM.Matrix.Matrix_Height -1;
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

        public float DistanceTo(WorldObject wo)
        {
            double dX = wo.xPos - this.xPos;
            double dY = wo.yPos - this.yPos;
            double multi = dX * dX + dY * dY;
            return (float)Math.Sqrt(multi);
        }


    }
}
