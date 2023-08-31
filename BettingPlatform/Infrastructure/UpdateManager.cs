namespace BettingPlatform.Infrastructure
{
    public class UpdateManager
    {
        public event EventHandler<UpdateEventArgs> UpdateOccurred;

        public void RaiseUpdateEvent(UpdateEventArgs e)
        {
            UpdateOccurred?.Invoke(this, e);
        }
    }
}
