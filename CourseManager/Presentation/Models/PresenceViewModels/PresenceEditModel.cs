using Data.Domain.Entities;
using System.Collections.Generic;

namespace Presentation.Models.PresenceViewModels
{
    public class PresenceEditModel
    {
        public string Name { get; set; }

        public PresenceEditModel(string name)
        {
            Name = name;
            //Week = week;
        }
    }
}