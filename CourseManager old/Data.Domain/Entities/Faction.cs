using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using Data.Domain.Interfaces;

namespace Data.Domain.Entities
{
    public class Faction
    {
        public Faction()
        {
            //EF
        }

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