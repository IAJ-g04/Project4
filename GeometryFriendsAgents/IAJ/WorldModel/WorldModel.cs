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
        


    }
}
