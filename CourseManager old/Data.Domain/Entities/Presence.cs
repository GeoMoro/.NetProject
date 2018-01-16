﻿using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using Data.Domain.Interfaces;

namespace Data.Domain.Entities
{
    public class Presence
    {
        public Presence()
        {
            //EF
        }

        [Key]
        public Guid Id { get; set; }

        public string Laboratory { get; set; }

        public bool Present { get; set; }

        public static Presence CreatePresence(string laboratory, bool present)
        {
            var instance = new Presence
            {
                Id = Guid.NewGuid()
            };
            instance.UpdatePresence(laboratory, present);

            return instance;
        }

        private void UpdatePresence(string laboratory, bool present)
        {
            Laboratory = laboratory;
            Present = present;
        }
    }
}