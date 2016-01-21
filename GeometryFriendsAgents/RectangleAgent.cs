

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using GeometryFriends.AI.Interfaces;

using GeometryFriendsAgents.ProblemDectection;
using GeometryFriendsAgents.Model;

using GeometryFriendsAgents.Pathfinding;
using GeometryFriendsAgents.Pathfinding.Heuristics;
using GeometryFriendsAgents.DecisionMaking;

namespace GeometryFriendsAgents
{
    class RectangleAgent : IRectangleAgent
    {
        private bool implementedAgent;
        private int lastAction;
        private int currentAction;
        private long lastMoveTime;
        private Random rnd;

        //Sensors Information
        private int[] numbersInfo;
        private float[] rectangleInfo;
        private float[] circleInfo;
        private float[] obstaclesInfo;
        private float[] rectanglePlatformsInfo;
        private float[] circlePlatformsInfo;
        private float[] collectiblesInfo;

        private int nCollectiblesLeft;

        private string agentName = "LearningRect";

        //Area of the game screen
        protected Rectangle area;

        //LEARNING AGENT
        protected WorldModel Model;
        private ProblemDectectionAlgorithm PdA;
        private NodeArrayAStarPathFinding AStar;
        private DecisionMakingProcess DMP;

        private RectangleCharacter CurrentRectangle;

        public RectangleAgent() 
        {
            //Change flag if agent is not to be used
            SetImplementedAgent(true);

            lastMoveTime = DateTime.Now.Second;
            lastAction = 0;
            currentAction = 0;
            rnd = new Random();

        }

        public void Setup(int[] nI, float[] sI, float[] cI, float[] oI, float[] sPI, float[] cPI, float[] colI, Rectangle area, double timeLimit) {
            // Time limit is the time limit of the level - if negative then there is no time constrains
            this.area = area;
            int temp;

            // numbersInfo[] Description
            //
            // Index - Information
            //
            //   0   - Number of Obstacles
            //   1   - Number of Rectangle Platforms
            //   2   - Number of Circle Platforms
            //   3   - Number of Collectibles

            numbersInfo = new int[4];
            int i;
            for (i = 0; i < nI.Length; i++)
            {
                numbersInfo[i] = nI[i];
            }

            nCollectiblesLeft = nI[3];

            // rectangleInfo[] Description
            //
            // Index - Information
            //
            //   0   - Rectangle X Position
            //   1   - Rectangle Y Position
            //   2   - Rectangle X Velocity
            //   3   - Rectangle Y Velocity
            //   4   - Rectangle Height

            rectangleInfo = new float[5];

            rectangleInfo[0] = sI[0];
            rectangleInfo[1] = sI[1];
            rectangleInfo[2] = sI[2];
            rectangleInfo[3] = sI[3];
            rectangleInfo[4] = sI[4];

            // circleInfo[] Description
            //
            // Index - Information
            //
            //   0  - Circle X Position
            //   1  - Circle Y Position
            //   2  - Circle X Velocity
            //   3  - Circle Y Velocity
            //   4  - Circle Radius

            circleInfo = new float[5];

            circleInfo[0] = cI[0];
            circleInfo[1] = cI[1];
            circleInfo[2] = cI[2];
            circleInfo[3] = cI[3];
            circleInfo[4] = cI[4];


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

            if (numbersInfo[0] > 0)
                obstaclesInfo = new float[numbersInfo[0] * 4];
            else obstaclesInfo = new float[4];

            temp = 1;
            if (nI[0] > 0)
            {
                while (temp <= nI[0])
                {
                    obstaclesInfo[(temp * 4) - 4] = oI[(temp * 4) - 4];
                    obstaclesInfo[(temp * 4) - 3] = oI[(temp * 4) - 3];
                    obstaclesInfo[(temp * 4) - 2] = oI[(temp * 4) - 2];
                    obstaclesInfo[(temp * 4) - 1] = oI[(temp * 4) - 1];
                    temp++;
                }
            }
            else
            {
                obstaclesInfo[0] = oI[0];
                obstaclesInfo[1] = oI[1];
                obstaclesInfo[2] = oI[2];
                obstaclesInfo[3] = oI[3];
            }

            // rectanglePlatformsInfo[] Description
            //
            // Index - Information
            //
            // If (Number of Rectangle Platforms > 0)
            //  [0; (numRectanglePlatforms * 4) - 1]   - Rectangle Platforms' info [X,Y,H,W]
            // Else
            //   0                                  - 0
            //   1                                  - 0
            //   2                                  - 0
            //   3                                  - 0

            if (numbersInfo[1] > 0)
                rectanglePlatformsInfo = new float[numbersInfo[1] * 4];
            else
                rectanglePlatformsInfo = new float[4];

            temp = 1;
            if (nI[1] > 0)
            {
                while (temp <= nI[1])
                {
                    rectanglePlatformsInfo[(temp * 4) - 4] = sPI[(temp * 4) - 4];
                    rectanglePlatformsInfo[(temp * 4) - 3] = sPI[(temp * 4) - 3];
                    rectanglePlatformsInfo[(temp * 4) - 2] = sPI[(temp * 4) - 2];
                    rectanglePlatformsInfo[(temp * 4) - 1] = sPI[(temp * 4) - 1];
                    temp++;
                }
            }
            else
            {
                rectanglePlatformsInfo[0] = sPI[0];
                rectanglePlatformsInfo[1] = sPI[1];
                rectanglePlatformsInfo[2] = sPI[2];
                rectanglePlatformsInfo[3] = sPI[3];
            }

            // circlePlatformsInfo[] Description
            //
            // Index - Information
            //
            // If (Number of Circle Platforms > 0)
            //  [0; (numCirclePlatforms * 4) - 1]   - Circle Platforms' info [X,Y,H,W]
            // Else
            //   0                                  - 0
            //   1                                  - 0
            //   2                                  - 0
            //   3                                  - 0

            if (numbersInfo[2] > 0)
                circlePlatformsInfo = new float[numbersInfo[2] * 4];
            else
                circlePlatformsInfo = new float[4];

            temp = 1;
            if (nI[2] > 0)
            {
                while (temp <= nI[2])
                {
                    circlePlatformsInfo[(temp * 4) - 4] = cPI[(temp * 4) - 4];
                    circlePlatformsInfo[(temp * 4) - 3] = cPI[(temp * 4) - 3];
                    circlePlatformsInfo[(temp * 4) - 2] = cPI[(temp * 4) - 2];
                    circlePlatformsInfo[(temp * 4) - 1] = cPI[(temp * 4) - 1];
                    temp++;
                }
            }
            else
            {
                circlePlatformsInfo[0] = cPI[0];
                circlePlatformsInfo[1] = cPI[1];
                circlePlatformsInfo[2] = cPI[2];
                circlePlatformsInfo[3] = cPI[3];
            }

            //Collectibles' To Catch Coordinates (X,Y)
            //
            //  [0; (numCollectibles * 2) - 1]   - Collectibles' Coordinates (X,Y)

            collectiblesInfo = new float[numbersInfo[3] * 2];

            temp = 1;
            while (temp <= nI[3])
            {

                collectiblesInfo[(temp * 2) - 2] = colI[(temp * 2) - 2];
                collectiblesInfo[(temp * 2) - 1] = colI[(temp * 2) - 1];

                temp++;
            }


            this.Model = new WorldModel(nI,sI, cI, oI, sPI, cPI, colI, area);
            this.CurrentRectangle = this.Model.Character;
            this.CurrentRectangle.heigth = sI[4];
            this.PdA = new ProblemDectectionAlgorithm(this.Model);

            this.PdA.GeneratePoints();
            this.PdA.GenerateConnections();

            this.DMP = new DecisionMakingProcess(this.Model);

            DebugSensorsInfo();
        }

        private void SetImplementedAgent(bool b)
        {
            implementedAgent = b;
        }

        public void setAgentPane(GeometryFriends.AgentDebugPane aP)
        {
            //this.agentPane = aP;
        }

        public bool ImplementedAgent()
        {
            return implementedAgent;
        }

        private void RandomAction()
        {
            /*
             Rectangle Actions
             MOVE_LEFT = 5
             MOVE_RIGHT = 6
             MORPH_UP = 7
             MORPH_DOWN = 8
            */
            
            currentAction = rnd.Next(5, 9);

            if (currentAction == lastAction)
            {
                if (currentAction == 8)
                {
                    currentAction = rnd.Next(5, 8);
                }
                else
                {
                    currentAction = currentAction + 1;
                }
            }

            switch (currentAction)
            {
                case 5:
                    SetAction(Moves.MOVE_LEFT);
                    break;
                case 6:
                    SetAction(Moves.MOVE_RIGHT);
                    break;
                case 7:
                    SetAction(Moves.MORPH_UP);
                    break;
                case 8:
                    SetAction(Moves.MORPH_DOWN);
                    break;
                default:
                    break;
            }
        }

        private void SetAction(int a)
        {
            currentAction = a;
        }

        //Manager gets this action from agent
        public int GetAction()
        {
            return currentAction;
        }

        public void SensorsUpdated(int nC, float[] sI, float[] cI, float[] colI)
        {
            int temp;

            nCollectiblesLeft = nC;

            rectangleInfo[0] = sI[0];
            rectangleInfo[1] = sI[1];
            rectangleInfo[2] = sI[2];
            rectangleInfo[3] = sI[3];
            rectangleInfo[4] = sI[4];

            this.CurrentRectangle = new RectangleCharacter(this.Model, rectangleInfo[0], rectangleInfo[1], sI[4]);

            circleInfo[0] = cI[0];
            circleInfo[1] = cI[1];
            circleInfo[2] = cI[2];
            circleInfo[3] = cI[3];
            circleInfo[4] = cI[4];

            Array.Resize(ref collectiblesInfo, (nCollectiblesLeft * 2));

            temp = 1;
            while (temp <= nCollectiblesLeft)
            {
                collectiblesInfo[(temp * 2) - 2] = colI[(temp * 2) - 2];
                collectiblesInfo[(temp * 2) - 1] = colI[(temp * 2) - 1];

                temp++;
            }

            
            //  ConsolePrinter.PrintLine(sI[4].ToString());

        }

        // this method is deprecated, please use SensorsUpdated instead
        public void UpdateSensors(int nC, float[] sI, float[] cI, float[] colI)
        {
            
        }

        public void Update(TimeSpan elapsedGameTime) {

            //Console.WriteLine("    now = {0}   --  square last {1} ", DateTime.Now.Second, lastMoveTime);

            //Every second one new action is choosen
            if (lastMoveTime == 60)
                lastMoveTime = 0;
            if ((lastMoveTime) <= (DateTime.Now.Second) && (lastMoveTime < 60))
            {
                if (!(DateTime.Now.Second == 59))
                {

                    lastMoveTime = lastMoveTime + 1;
                    this.SetAction(this.DMP.GetNextAction(this.CurrentRectangle));
                   // RandomAction();
                    DebugSensorsInfo();
                    ConsolePrinter.PrintLine("?????");
                }
                else
                {
                    lastMoveTime = 60;

                    ConsolePrinter.PrintLine("inner");
                }

            }

            ConsolePrinter.PrintLine(lastMoveTime.ToString());

        }

        public void toggleDebug()
        {
            //this.agentPane.AgentVisible = !this.agentPane.AgentVisible;
        }

        protected void DebugSensorsInfo()
        {
            int t = 0;
            /*
            foreach (int i in numbersInfo)
            {
                Console.WriteLine("RECTANGLE - Numbers info - {0} - {1}", t, i);
                t++;
            }
            */

            Console.WriteLine("RECTANGLE - collectibles left - {0}", nCollectiblesLeft);
            Console.WriteLine("RECTANGLE - collectibles info size - {0}", collectiblesInfo.Count());

            /*
            t = 0;
            foreach (long i in rectangleInfo)
            {
                Console.WriteLine("RECTANGLE - Rectangle info - {0} - {1}", t, i);
                t++;
            }

            t = 0;
            foreach (long i in circleInfo)
            {
                Console.WriteLine("RECTANGLE - Circle info - {0} - {1}", t, i);
                t++;
            }
            
            t = 0;
            foreach (long i in obstaclesInfo)
            {
                Console.WriteLine("RECTANGLE - Obstacles info - {0} - {1}", t, i);
                t++;
            }

            t = 0;
            foreach (long i in rectanglePlatformsInfo)
            {
                Console.WriteLine("RECTANGLE - Rectangle Platforms info - {0} - {1}", t, i);
                t++;
            }

            t = 0;
            foreach (long i in circlePlatformsInfo)
            {
                Console.WriteLine("RECTANGLE - Circle Platforms info - {0} - {1}", t, i);
                t++;
            }
            */
            t = 0;
            foreach (float i in collectiblesInfo)
            {
                Console.WriteLine("RECTANGLE - Collectibles info - {0} - {1}", t, i);
                t++;
            }
        }

        public string AgentName()
        {
            return agentName;
        }

        public void EndGame(int collectiblesCaught, int timeElapsed) {
            Console.WriteLine("RECTANGLE - Collectibles caught = {0}, Time elapsed - {1}", collectiblesCaught, timeElapsed);
        }
    }
}