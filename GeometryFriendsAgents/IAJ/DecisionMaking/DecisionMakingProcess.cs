using GeometryFriendsAgents.Model;
using GeometryFriendsAgents.Pathfinding;
using GeometryFriendsAgents.InstructionManual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryFriendsAgents.DecisionMaking
{
    public class DecisionMakingProcess : WorldModelComponent
    {
        public InstructionManualProcessor Manual { private set; get; }
        public AStarPathfinding AStar { private set; get; }

        public int CurrentConnectionID { private set; get; }
        
        public int CurrentActionID { private set; get; }
        public int[] CurrentSolution { private set; get; }
        
        public RectangleCharacter CurrentRectangle { set; get; }


        public DecisionMakingProcess (WorldModel WM, AStarPathfinding AStar) : base(WM)
        {
            this.AStar = AStar;
            this.Manual = new InstructionManualProcessor(WM);
            this.CurrentConnectionID = -1;
            this.CurrentActionID = -1;
        }

        public int GetNextAction(RectangleCharacter cube)
        {
            return 6;
            if (cube.Equals(CurrentRectangle) && this.CurrentConnectionID != -1)
            {
                this.CurrentSolution = this.Manual.GetAlternative(this.WM.Path[this.CurrentConnectionID]);
                this.CurrentActionID = -1;
            }
            
            this.CurrentRectangle = cube;

            if (this.CurrentConnectionID == -1 || this.isOutPath()) { 
                this.AStar.InitializePathfindingSearch(this.CurrentRectangle);

                bool res = false;
                while (!res) { 
                    res = this.AStar.Search();
                }

                this.CurrentConnectionID = 0;
                this.CurrentSolution = this.Manual.GetSolution(this.WM.Path[this.CurrentConnectionID]);
                this.CurrentActionID = -1;
            }

            if (this.isOutConn())
            {
                this.CurrentConnectionID++;
                this.CurrentSolution = this.Manual.GetSolution(this.WM.Path[this.CurrentConnectionID]);
                this.CurrentActionID = -1;
            }

            this.CurrentActionID++;
            return this.CurrentSolution[this.CurrentActionID];
        }

        public bool isOutPath()
        {

            throw new NotImplementedException();
        }

        public bool isOutConn()
        {

            throw new NotImplementedException();
        }

    }
}
