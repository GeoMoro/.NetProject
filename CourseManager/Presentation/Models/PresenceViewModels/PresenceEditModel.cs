namespace Presentation.Models.PresenceViewModels
{
    public class PresenceEditModel
    {
        public PresenceEditModel() {
            // EF
        }

        public PresenceEditModel(bool present)
        {
            Present = present;
        }

        public bool Present { get; set; }
    }
}