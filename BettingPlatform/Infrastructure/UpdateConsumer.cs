namespace BettingPlatform.Infrastructure
{
    public class UpdateConsumer
    {
        private readonly UpdateManager updateManager;

        public UpdateConsumer(UpdateManager updateManager)
        {
            this.updateManager = updateManager;
            this.updateManager.UpdateOccurred += OnUpdateOccurred;
        }

        private void OnUpdateOccurred(object sender, UpdateEventArgs e)
        {
            Console.WriteLine($"Update occurred - Entity: {e.Entity}, EntityId: {e.EntityId}");
        }
    }
}
