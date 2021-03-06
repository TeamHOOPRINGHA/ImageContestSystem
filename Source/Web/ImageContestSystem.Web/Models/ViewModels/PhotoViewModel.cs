﻿namespace ImageContestSystem.Web.Models.ViewModels
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class PhotoViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Author { get; set; }

        public int ContestId { get; set; }

        public bool HasVoted { get; set; }

        [DefaultValue(0)]
        public int Votes { get; set; }

        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}