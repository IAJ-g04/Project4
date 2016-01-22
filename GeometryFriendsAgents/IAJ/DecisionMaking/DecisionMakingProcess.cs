using GeometryFriendsAgents.Model;
using GeometryFriendsAgents.Pathfinding;
using GeometryFriendsAgents.InstructionManual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeometryFriendsAgents.Pathfinding.Heuristics;

namespace GeometryFriendsAgents.DecisionMaking
{
    public class DecisionMakingProcess : WorldModelComponent
    {
        public InstructionManualProcessor Manual { private set; get; }
        public NodeArrayAStarPathFinding AStar { private set; get; }

        public int CurrentConnectionID { private set; get; }
        
        public int CurrentActionID { private set; get; }
        public int[] CurrentSolution;
        
        public RectangleCharacter CurrentRectangle { set; get; }


        public DecisionMakingProcess (WorldModel WM) : base(WM)
        {
            this.AStar = new NodeArrayAStarPathFinding(WM, new ZeroHeuristic());
            this.Manual = new InstructionManualProcessor(WM);
            this.CurrentConnectionID = -1;
            this.CurrentActionID = -1;
            this.CurrentRectangle = WM.Character;
            this.CurrentSolution = new int[0];
        }

        public int GetNextAction(RectangleCharacter cube)
        {
            if ((cube.Equals(CurrentRectangle) && this.CurrentConnectionID != -1) || 
                (((this.CurrentConnectionID != -1) && (this.CurrentActionID >= this.CurrentSolution.Length - 1) && !isOutConn(this.WM.Path[this.CurrentConnectionID]) && !isOnGoal())))
            {
                ConsolePrinter.PrintLine("Improvise");
                this.calculateNewAction();
            }
            
            this.CurrentRectangle = cube;

            if((this.CurrentConnectionID != -1) && isOnGoal()){
                
                this.Manual.Update(this.WM.Path[this.CurrentConnectionID], getSolution());
                this.CurrentConnectionID++;
                this.ResetSolution();

                ConsolePrinter.PrintLine("Follow");
            }

            if (this.CurrentConnectionID == -1 || this.isOutPath()) { 
                this.AStar.InitializePathfindingSearch(this.CurrentRectangle);

                if (this.AStar.InProgress)
                {
                    if (this.AStar.Search())
                    {
                        this.CurrentConnectionID = 0;
                        this.ResetSolution();
                        ConsolePrinter.PrintLine("Search");
                    } 
                } 
            }

            this.CurrentActionID++;
          /*  if ((this.CurrentActionID >= this.CurrentSolution.Length - 1))
            {

                ConsolePrinter.PrintLine("Improvise a lot");
                this.calculateNewAction();
            }*/


            ConsolePrinter.PrintLine("ActionID: " + this.CurrentActionID);
            ConsolePrinter.PrintLine("Action  : " + this.CurrentSolution[this.CurrentActionID]);
            return this.CurrentSolution[this.CurrentActionID];
        }

        private void ResetSolution()
        {
            this.CurrentActionID = -1;

            this.setSolution(this.Manual.getSolution(this.WM.Path[this.CurrentConnectionID]));
            
        }

        public bool isOutPath()
        {
            bool result = true;
            foreach (Connection cc in this.WM.Path)
                result &= isOutConn(cc);
            return result;

        }

        //HERE
        //this.WM.Matrix.WORLD_UNIT_SIZE;

        public bool isOnGoal()
        {
            float xPos = this.CurrentRectangle.xPos;
            float yPos = this.CurrentRectangle.yPos;
            Connection cc = this.WM.Path[this.CurrentConnectionID];

            if (Math.Abs(xPos - cc.Destination.xPos) < this.WM.Matrix.WORLD_UNIT_SIZE * 2 &&
                Math.Abs(yPos - cc.Destination.yPos) < this.WM.Matrix.WORLD_UNIT_SIZE * 2)
                return true;

            return false;
        }

        public bool isOutConn(Connection cc)
        {
            float xPos = this.CurrentRectangle.xPos;
            float yPos = this.CurrentRectangle.yPos;

            if (cc.side == cc.LEFT)
                if (xPos > cc.Origin.xPos + this.WM.Matrix.WORLD_UNIT_SIZE || xPos < cc.Destination.xPos - this.WM.Matrix.WORLD_UNIT_SIZE)
                    return true;
                else
                 if (xPos < cc.Origin.xPos - this.WM.Matrix.WORLD_UNIT_SIZE || xPos > cc.Destination.xPos + this.WM.Matrix.WORLD_UNIT_SIZE)
                    return true;

            return false;
        }

        public void setSolution(String s)
        {
            int size = s.Count();
            this.CurrentSolution = new int[size];
            for(int count = 0; count < size; count++)
            {
                this.CurrentSolution[count] = int.Parse(s[count].ToString());
            }
        }

        public string getSolution()
        {
            string result = "";
            foreach(int i in this.CurrentSolution)
            {
                result = result + i;
            }
            return result;
        }

        //this.WM.Matrix.WORLD_UNIT_SIZE
        public void calculateNewAction()
        {
            Connection cc = this.WM.Path[this.CurrentConnectionID];
            float xPos = this.CurrentRectangle.xPos;
            float yPos = this.CurrentRectangle.yPos;
            int newAction = 5;
            float xOriginCheck = Math.Abs(xPos - cc.Origin.xPos);
            float yOriginCheck = Math.Abs(yPos - cc.Origin.yPos);
            float xDestinationCheck = Math.Abs(xPos - cc.Destination.xPos);
            float yDestinationCheck = Math.Abs(yPos - cc.Destination.yPos);

            switch (cc.categorie)
            {
                case 1:
                    if (yDestinationCheck > this.WM.Matrix.WORLD_UNIT_SIZE) // still up
                    {
                        if (xDestinationCheck < this.WM.Matrix.WORLD_UNIT_SIZE && this.CurrentRectangle.heigth < 192) // is low
                            newAction = 7;
                        else
                        {
                            if (xPos - cc.Destination.xPos < 0)
                                newAction = 6;
                            else
                                newAction = 5;
                        }
                    }
                    else
                    {
                        if (xDestinationCheck > this.WM.Matrix.WORLD_UNIT_SIZE)
                        {
                            if (xPos - cc.Destination.xPos < 0)
                                newAction = 6;
                            else
                                newAction = 5;
                        }
                        else
                        {
                            newAction = 8;
                        }
                    }
                    break;

                case 2:
                    if (xDestinationCheck > this.WM.Matrix.WORLD_UNIT_SIZE)
                    {
                        if (xPos - cc.Destination.xPos < 0)
                            newAction = 6;
                        else
                            newAction = 5;
                    }
                    break;

                case 3:
                    if (this.CurrentRectangle.heigth > 52)
                        newAction = 8;
                    else
                        if (xDestinationCheck > this.WM.Matrix.WORLD_UNIT_SIZE)
                        {
                            if (xPos - cc.Destination.xPos < 0)
                                newAction = 6;
                            else
                                newAction = 5;
                        }
                    break;

                case 4:
                    if (xDestinationCheck > this.WM.Matrix.WORLD_UNIT_SIZE)
                    {
                        if (xPos - cc.Destination.xPos < 0)
                            newAction = 6;
                        else
                            newAction = 5;
                    }
                    else
                    {
                        newAction = 7;
                    }
                    break;

                case 5:
                    if (yDestinationCheck > this.WM.Matrix.WORLD_UNIT_SIZE)
                    {
                        if (xOriginCheck < this.WM.Matrix.WORLD_UNIT_SIZE)
                        {

                            if (this.CurrentRectangle.heigth < 192)
                                newAction = 7;
                            else
                                if (xPos - cc.Destination.xPos < 0)
                                newAction = 6;
                            else
                                newAction = 5;
                        }
                        else
                        {
                            if (xPos - cc.Origin.xPos < 0)
                                newAction = 6;
                            else
                                newAction = 5;
                        }
                    }
                    else
                    {
                      if (xPos - cc.Destination.xPos < 0)
                            newAction = 6;
                      else
                            newAction = 5;
                        
                    }
                    break;

                default:
                    if (xPos - cc.Destination.xPos < 0)
                        newAction = 6;
                    else
                        newAction = 5;
                    break;
            }
            Array.Resize(ref CurrentSolution, CurrentSolution.Length + 1);
            CurrentSolution[CurrentActionID+1] = newAction;
        }
    }
}
