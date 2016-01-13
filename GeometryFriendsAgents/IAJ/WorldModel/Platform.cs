using GeometryFriendsAgents.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryFriendsAgents.Model
{
    public class Platform : WorldObject, IComparable {

        private const int TOBEINITIALIZED = -1;
        protected int _id;

        public int ID {
            get { return _id; }
            set { _id = value; }
        }

        public bool LeftWall = false;

        public bool RightWall = false;

        protected int _width;

        public int Width {
            get { return _width; }
            set { _width = value; }
        }

        protected int _height;

        public int Height {
            get { return _height; }
            set { _height = value; }
        }

        public int Left {
            get { return (int)xPos - Width / 2; }
        }

        public int Right {
            get { return (int)xPos + Width / 2; }
        }

        public int Top { 
            get { return (int)yPos - Height / 2; } 
        }

        public int Bottom {
            get { return (int)yPos + Height / 2; }
        }

        protected List<Collectible> _collectibles;

        public List<Collectible> Collectibles {
            get { return _collectibles; }
        }

        protected int _totalNumberCollectibles;

        public int TotalNumberCollectibles {
            get { return _totalNumberCollectibles; }
        }

        protected DateTime _startTime;

        public DateTime StartTime {
            get { return _startTime; }
            set { _startTime = value; }
        }

        protected DateTime _endTime;

        public DateTime EndTime {
            get { return _endTime; }
            set { _endTime = value; }
        }

        protected Platform(WorldModel WM, int xPos, int yPos, int height, int width) : base(WM, xPos, yPos)
        {
            _width = width;
            _height = height;
            _totalNumberCollectibles = TOBEINITIALIZED;
            _collectibles = new List<Collectible>();
        }

        public void AddCollectibles(List<Collectible> c) {
            if (_totalNumberCollectibles == -1) {
                _totalNumberCollectibles = c.Count;
            }
            _collectibles = c;
        }

        public int CompareTo(object obj) {
            Platform p = (Platform)obj;

            int myTop = (int)yPos - _height / 2;
            int hisTop = (int)p.yPos - p.Height / 2;

            int myLeft = (int)xPos - _width / 2;
            int hisLeft = (int)p.xPos - p.Width / 2;

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

        public int NumberCollectiblesCaught { get { return _totalNumberCollectibles - _collectibles.Count; } }

        public int PercentageCollectiblesCaught {
            get {
                if (TotalNumberCollectibles == 0) {
                    return 100;
                } else {
                    return (int)(((float)NumberCollectiblesCaught / TotalNumberCollectibles) * 100);
                }
            }
        }

        internal Platform Clone()
        {
            throw new NotImplementedException();
        }

        internal virtual bool Colides(Platform analysingPlatform) {
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
            return ((Platform)obj).ID == ID;
        }
    }
}
