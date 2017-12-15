using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Domain.Entities
{
    public class Faction
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime Date { get; set; }
        
        public string Week { get; set; }

        public static Faction CreateFaction(DateTime date, string week)
        {
            var instance = new Faction
            {
                Id = Guid.NewGuid()
            };
            instance.UpdateFaction(date, week);

            return instance;
        }

        private void UpdateFaction(DateTime date, string week)
        {
            Date = date;
            Week = week;
        }
    }
}