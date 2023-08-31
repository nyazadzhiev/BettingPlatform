using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BettingPlatform.Infrastructure
{
    public class UpdateEventArgs : EventArgs
    {
        //public UpdateType Type { get; set; }
        public EntityType Entity { get; set; }
        public string EntityId { get; set; }
    }
}
