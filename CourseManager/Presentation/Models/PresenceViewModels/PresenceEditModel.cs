using Data.Domain.Entities;
using System.Collections.Generic;

namespace Presentation.Models.PresenceViewModels
{
    public class PresenceEditModel
    {
        public string Name { get; set; }

      //  public List<UserStatus> Students { get; set; }

        public PresenceEditModel(string name)//, List<UserStatus> students)
        {
            Name = name;
           // Students = students;
        }
    }
}