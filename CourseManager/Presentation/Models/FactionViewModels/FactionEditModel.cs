using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Models
{
    public class FactionEditModel
    {
        public FactionEditModel()
        {

        }

        public DateTime Date { get; set; }

        public string Week { get; set; }

        public FactionEditModel(DateTime date, string week)
        {
            Date = date;
            Week = week;
        }

    }
}
