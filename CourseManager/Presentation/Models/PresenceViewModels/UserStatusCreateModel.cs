using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Models.PresenceViewModels
{
    public class UserStatusCreateModel
    {
        public Guid LaboratoryId { get; set; }
        public bool Presence { get; set; }
    }
}
