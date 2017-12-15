using System;

namespace Presentation.Models.FactionViewModels
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
