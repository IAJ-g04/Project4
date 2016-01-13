namespace GeometryFriendsAgents.Model
{
    public abstract class WorldModelComponent
    {
        public WorldModel WM { private set; get; }

        public WorldModelComponent(WorldModel WM)
        {
            this.WM = WM;
        }
    }
}
