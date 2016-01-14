using GeometryFriendsAgents.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryFriendsAgents.InstructionManual
{
    public class InstructionManualProcessor : WorldModelComponent
    {
        public InstructionManualProcessor(WorldModel WM) : base(WM)
        {
        }

        public int[] GetSolution(Connection conn)
        {
            throw new NotImplementedException();
        }

        public int[] GetAlternative(Connection conn)
        {
            throw new NotImplementedException();
        }
    }
}
