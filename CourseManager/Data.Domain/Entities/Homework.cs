﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Domain.Entities
{
    class Homework
    {
        [Required(ErrorMessage = "Please enter the seminar week number")]
        public int Seminar { get; set; }

        [Required(ErrorMessage = "Is this a Homework or Kata assignment?")]
        public string Type { get; set; }


/*        public static Kata CreateKata(string title, string description)
        {
            var instance = new Kata { Id = Guid.NewGuid() };
            instance.UpdateKata(title, description);

            return instance;
        }

        private void UpdateKata(string title, string description)
        {
            Title = title;
            Description = description;
        }*/
    }
}
