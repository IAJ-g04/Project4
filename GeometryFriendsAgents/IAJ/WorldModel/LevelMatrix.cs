using System;
using System.Collections.Generic;

namespace GeometryFriendsAgents.Model
{
    public class LevelMatrix : WorldModelComponent
    {
        public int WORLD_UNIT_SIZE = 48;

        public int LEFT = 1;
        public int UP = 2;
        public int RIGHT = 3;
        public int DOWN = 4;

        public int EMPTY = 0;
        public int STAR = -1;

        //Matrix key
        // if (key <= 0)
        //  0 -> Empty
        //  -1 -> Star
        // else
        //  n -> platform id
        public int[,] Matrix { private set; get; }
        public float World_Height { private set; get; }
        public float World_Width { private set; get; }
        public int Matrix_Height { private set; get; }
        public int Matrix_Width { private set; get; }

        public LevelMatrix(WorldModel WM, float height, float width) : base(WM)
        {
            this.World_Width = width;
            this.World_Height = height;

            this.Matrix_Width = (int)width / WORLD_UNIT_SIZE;
            this.Matrix_Height = (int)height / WORLD_UNIT_SIZE;

            this.Matrix = new int[this.Matrix_Width, this.Matrix_Height];
            for (int i = 0; i < Matrix_Width; i++)
            {
                for (int j = 0; j < Matrix_Height; j++)
                    this.Matrix[i, j] = EMPTY;
            }

            //ConsolePrinter.PrintLine("World_Width: " + width + " | World_Height: " + height + " | Matrix_Width: " + this.Matrix_Width + " | Matrix_Height: " + this.Matrix_Height);

        }

        public void InitializeMatrix()
        {
            foreach (Platform p in this.WM.PlatformList.Values)
            {
                for (int i = p.LeftMatrix; i <= p.RightMatrix; i++)
                {
                    for (int j = p.TopMatrix; j <= p.BottomMatrix; j++)
                    {
                        this.Matrix[i, j] = p.ID;
                    }
                }
            }

            foreach (Collectible c in this.WM.CollectibleList)
            {
                this.Matrix[c.xMatrix, c.yMatrix] = STAR;
            }
        }

        public Point GenerateNewPoint(Platform p, int dir)
        {
            Point point;
            if (dir == LEFT)
            {
                point = new Point(this.WM, this.LEFT, p.Left, p.TopPointY);
            }
            else
            {
                point = new Point(this.WM, this.RIGHT, p.Right, p.TopPointY);
            }

            if (!(CheckPosition(point, this.WM.Matrix.EMPTY)) && !(CheckPosition(point, this.WM.Matrix.STAR)))
            {
                int cont = this.Matrix[point.xMatrix, point.yMatrix];
                Platform pTop;
                bool res = this.WM.PlatformList.TryGetValue(cont, out pTop);

                if (res)
                {
                    if (dir == LEFT)
                    {
                        if (pTop.Right < p.Right)
                            point.xPos = pTop.Right;
                        else
                            return null;
                    }
                    else
                    {
                        if (pTop.Left > p.Left)
                            point.xPos = pTop.Left;
                        else
                            return null;
                    }
                }
             }
            return point;
        }

        public bool CheckPosition(Point origPoint, int content)
        {
            return (this.Matrix[origPoint.xMatrix, origPoint.yMatrix] == content);
        }


    }
}
