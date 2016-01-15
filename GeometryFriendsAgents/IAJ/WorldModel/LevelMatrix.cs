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

        public int PLAT = -1;
        public int EMPTY = 0;
        public int STAR = 1;

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

            foreach (Collectible c in this.WM.CollectibleList.Values)
            {
                this.Matrix[c.xMatrix, c.yMatrix] = c.ID;
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

        public Point GenerateNewPointFalling(Point p, int dir)
        {
            float xFalling;
            if (p.side == LEFT)
                xFalling = p.xPos - (this.WORLD_UNIT_SIZE / 2);
            else
                xFalling = p.xPos + (this.WORLD_UNIT_SIZE / 2);

            if (xFalling < 0)
                xFalling = 0;
            if (xFalling > World_Width)
                xFalling = World_Width;

            float yFalling = p.yPos;
            Point pf = new Point(this.WM, xFalling, yFalling);
            bool res = false;
            for (int j = pf.yMatrix + 1; j <= this.Matrix_Height; j++)
            {
                if (this.Matrix[pf.xMatrix, j] > 0)
                {
                    Platform plat;
                    res = this.WM.PlatformList.TryGetValue(this.Matrix[pf.xMatrix, j], out plat);
                    if (res)
                    {
                        pf.yPos = plat.TopPointY;
                    }
                }
            }

            if (!res)
            {
                pf.yPos = World_Height - WORLD_UNIT_SIZE;
            }
            
            pf.categorie = pf.FALLING_POINT;
            pf.side = pf.DOWN;
            return pf;
        }

        public bool CheckDownForStar(Point p)
        {
            float xFalling;
            if (p.side == LEFT)
                xFalling = p.xPos - (this.WORLD_UNIT_SIZE / 2);
            else
                xFalling = p.xPos + (this.WORLD_UNIT_SIZE / 2);

            if (xFalling < 0)
                xFalling = 0;
            if (xFalling > World_Width)
                xFalling = World_Width;

            float yFalling = p.yPos;
            Point pf = new Point(this.WM, xFalling, yFalling);
            bool res = false;
            for (int j = pf.yMatrix + 1; j <= this.Matrix_Height; j++)
            {
                if (this.Matrix[pf.xMatrix, j] == STAR)
                {
                    res = true;
                }
            }

            return res;
        }

        public Point GenerateNewPointFallingStar(Point p)
        {
            float xFalling;
            if (p.side == LEFT)
                xFalling = p.xPos - (this.WORLD_UNIT_SIZE / 2);
            else
                xFalling = p.xPos + (this.WORLD_UNIT_SIZE / 2);

            if (xFalling < 0)
                xFalling = 0;
            if (xFalling > World_Width)
                xFalling = World_Width;

            float yFalling = p.yPos;
            Point pf = new Point(this.WM, xFalling, yFalling);
            bool res = false;
            for (int j = pf.yMatrix + 1; j <= this.Matrix_Height; j++)
            {
                if (this.Matrix[pf.xMatrix, j] == STAR)
                {
                    Collectible col;
                    res = this.WM.CollectibleList.TryGetValue(this.Matrix[pf.xMatrix, j], out col);
                    if (res)
                    {
                        pf.xPos = col.xPos;
                        pf.yPos = col.yPos;
                    }
                }
            }

            if (!res)
            {
                pf.yPos = World_Height - WORLD_UNIT_SIZE;
            }

            pf.categorie = pf.FALLING_POINT;
            pf.side = pf.DOWN;
            return pf;
        }

        public bool CheckPointForFalling(Point origPoint)
        {
            if (origPoint.side == DOWN)
                return false;
            //DOWN 
            if (origPoint.yMatrix == 0)
                return false;

            int leftPosX = origPoint.xMatrix - 1;
            int rigthPosX = origPoint.xMatrix + 1;

            int downPosY = origPoint.yMatrix - 1;
            if (leftPosX >= 0)
            {
                bool result = (!this.CheckPosition(leftPosX, origPoint.yMatrix, this.WM.Matrix.EMPTY) && !this.CheckPosition(leftPosX, origPoint.yMatrix, this.WM.Matrix.STAR)) && (!this.CheckPosition(leftPosX, downPosY, this.WM.Matrix.EMPTY) && !this.CheckPosition(leftPosX, downPosY, this.WM.Matrix.STAR));

                if (result)
                    return true;
            }

            if (rigthPosX < this.Matrix_Width)
            {
                bool result = (!this.CheckPosition(rigthPosX, origPoint.yMatrix, this.WM.Matrix.EMPTY) && !this.CheckPosition(rigthPosX, origPoint.yMatrix, this.WM.Matrix.STAR)) && (!this.CheckPosition(rigthPosX, downPosY, this.WM.Matrix.EMPTY) && !this.CheckPosition(rigthPosX, downPosY, this.WM.Matrix.STAR));

                if (result)
                    return true;

            }
            return false;
        }

        public bool CheckPosition(Point origPoint, int content)
        {
           return this.CheckPosition(origPoint.xMatrix, origPoint.yMatrix, content);
        }

        public bool CheckPosition(int xOrig, int yOrig, int content)
        {
            int matrixContent = this.Matrix[xOrig, yOrig];
            if (content == PLAT && matrixContent > 0)
                return true;
            if (content == STAR && matrixContent < 0)
                return true;
            return (this.Matrix[xOrig, yOrig] == content);
        }

    }
}
