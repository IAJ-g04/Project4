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
        public int[] CurrentSolution { private set; get; }
        
        public RectangleCharacter CurrentRectangle { set; get; }


        public DecisionMakingProcess (WorldModel WM) : base(WM)
        {
            this.AStar = new NodeArrayAStarPathFinding(WM, new ZeroHeuristic());
            this.Manual = new InstructionManualProcessor(WM);
            this.CurrentConnectionID = -1;
            this.CurrentActionID = -1;
            this.CurrentRectangle = WM.Character;
        }

        public int GetNextAction(RectangleCharacter cube)
        {
            if (cube.Equals(CurrentRectangle) && this.CurrentConnectionID != -1)
            {
                this.setSolution(this.Manual.getAlternative(this.WM.Path[this.CurrentConnectionID]));
                this.CurrentActionID = -1;
            }
            
            this.CurrentRectangle = cube;

            if (this.CurrentConnectionID == -1 || this.isOutPath()) { 
                this.AStar.InitializePathfindingSearch(this.CurrentRectangle);

                if (this.AStar.InProgress)
                {
                    if (this.AStar.Search())
                    {

                        this.CurrentConnectionID = 0;
                        this.setSolution(this.Manual.getSolution(this.WM.Path[this.CurrentConnectionID]));
                        this.CurrentActionID = -1;
                    }
                }
            }
            /*
            if (this.isOutConn())
            {
                this.CurrentConnectionID++;
                this.setSolution(this.Manual.getSolution(this.WM.Path[this.CurrentConnectionID]));
                this.CurrentActionID = -1;
            }
            */
            this.CurrentActionID++;
            return this.CurrentSolution[this.CurrentActionID];
        }

        public bool isOutPath()
        {

            return false;
        }

        public bool isOutConn()
        {

            throw new NotImplementedException();
        }

        public void setSolution(String s)
        {
            int size = s.Count();
            this.CurrentSolution = new int[size];
            int count = 0;
            foreach(Char c in s)
            {
                this.CurrentSolution[count] = Convert.ToInt32(c);
                count++;
            }
        }

    }
}
