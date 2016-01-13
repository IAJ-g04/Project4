using GeometryFriendsAgents.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryFriendsAgents.Model
{
    public class Platform : WorldObject, IComparable {
        
        public float Width { private set; get; }
        public float Height { private set; get; }

        public Platform(WorldModel WM, float xPos, float yPos, float height, float width) : base(WM, xPos, yPos)
        {
            this.Width = width;
            this.Height = height;
        }

        public bool LeftWall = false;

        public bool RightWall = false;
        
        public float Left {
            get { return xPos - Width / 2; }
        }

        public float Right {
            get { return xPos + Width / 2; }
        }

        public float Top { 
            get { return yPos - Height / 2; } 
        }

        public float Bottom {
            get { return yPos + Height / 2; }
        }

        public int CompareTo(object obj) {
            Platform p = (Platform)obj;

            float myTop = (float)yPos - Height / 2;
            float hisTop = (float)p.yPos - p.Height / 2;

            float myLeft = (float)xPos - Width / 2;
            float hisLeft = (float)p.xPos - p.Width / 2;

            if (myTop < hisTop)
                return -1;
            else if (myTop > hisTop)
                return 1;
            else if (myLeft < hisLeft)
                return -1;
            else if (hisLeft < myLeft)
                return 1;
            else
                return 0;
        }

        public virtual bool Colides(Platform analysingPlatform) {
            Platform topPlatform = null, bottomPlatform = null;
            
            if (this.Top <= analysingPlatform.Top) {
                topPlatform = this;
                bottomPlatform = analysingPlatform;
            } else if (this.Top > analysingPlatform.Top) {
                topPlatform = analysingPlatform;
                bottomPlatform = this;
            } 

            return IsThereColision(topPlatform, bottomPlatform);
        }

        protected bool IsThereColision(Platform topPlatform, Platform bottomPlatform)
        {
            throw new NotImplementedException();
        }


        public List<Platform> SplitPlatform(Platform p)
        {
            throw new NotImplementedException();
        }


        public override bool Equals(object obj) {
           Platform plat = (Platform)obj;
            return    (this.xPos == plat.xPos) &&
                      (this.yPos == plat.yPos) &&
                      (this.Height == plat.Height) &&
                      (this.Width == plat.Width);
        }
    }
}
