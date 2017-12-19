using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Models.PresenceViewModels
{
    public class UserStatusEditModel
    {
        public UserStatusEditModel()
        {
        }

        public double LaboratoryMark { get; set; }

        public double KataMark { get; set; }

        public bool Presence { get; set; }

        public UserStatusEditModel(bool presence, double laboratoryMark, double kataMark)
        {
            Presence = presence;
            LaboratoryMark = laboratoryMark;
            KataMark = kataMark;
        }
    }
}
