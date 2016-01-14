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

        public int CurrentDestination { private set; get; }
        public Connection CurrentConnection { private set; get; }
        public RectangleCharacter CurrentRectangle { set; get; }


        public DecisionMakingProcess (WorldModel WM, AStarPathfinding AStar) : base(WM)
        {
            this.AStar = AStar;
            this.Manual = new InstructionManualProcessor(WM);
        }

        public void RunNewSearch()
        {
            throw new NotImplementedException();
        }
        

        public int GetNextAction()
        {
            throw new NotImplementedException();
        }

    }
}
