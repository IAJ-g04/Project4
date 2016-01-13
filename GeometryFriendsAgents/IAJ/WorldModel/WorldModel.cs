using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryFriendsAgents.Model {
    public class WorldModel {

        public LevelMatrix Matrix {get; private set;}
        public List<Platform> PlatformList { get; private set; }
        public List<Collectible> CollectibleList { get; private set; }

        public int NumberOfPlatforms { get; private set; }
        public int NumberOfCollectibles { get; private set; }

        public RectangleCharacter Character { get; private set; }

        public WorldModel(int[] nI, float[] sI, float[] cI, float[] oI, float[] sPI, float[] cPI, float[] colI, Rectangle area)
        {
            this.Matrix = new LevelMatrix(this, area.Height, area.Width);

            this.NumberOfPlatforms = nI[0];
            this.NumberOfCollectibles = nI[3];

            this.Character = new RectangleCharacter(this, sI[0], sI[1]);

            // Obstacles and Platforms Info Description
            //
            //  X = Center X Coordinate
            //  Y = Center Y Coordinate
            //
            //  H = Platform Height
            //  W = Platform Width
            //
            //  Position (X=0,Y=0) = Left Superior Corner

            // obstaclesInfo[] Description
            //
            // Index - Information
            //
            // If (Number of Obstacles > 0)
            //  [0 ; (NumObstacles * 4) - 1]      - Obstacles' info [X,Y,H,W]
            // Else
            //   0                                - 0
            //   1                                - 0
            //   2                                - 0
            //   3                                - 0
            
            if (this.NumberOfPlatforms > 0)
            {
                int count = 1;
                while (count <= this.NumberOfPlatforms)
                {
                    this.PlatformList.Add(new Platform(this, oI[(count * 4) - 4], oI[(count * 4) - 3], oI[(count * 4) - 2], oI[(count * 4) - 1]));
                    count++;
                }
            }

            //Collectibles' To Catch Coordinates (X,Y)
            //
            //  [0; (numCollectibles * 2) - 1]   - Collectibles' Coordinates (X,Y)

            if (this.NumberOfCollectibles > 0)
            {
                int count = 1;
                while (count <= this.NumberOfCollectibles)
                {
                    this.CollectibleList.Add(new Collectible(this, colI[(count * 2) - 2], colI[(count * 2) - 1]));
                    count++;
                }

            }

        }

    }
}
