using System.ComponentModel.DataAnnotations;

namespace Presentation.Models
{
    public class PresenceCreateModel
    {
        public string Laboratory { get; set; }

        public bool Present { get; set; }
    }
}