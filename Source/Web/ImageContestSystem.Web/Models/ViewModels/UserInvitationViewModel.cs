namespace ImageContestSystem.Web.Models.ViewModels
{
    using Data.Models;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq.Expressions;

    public class UserInvitationViewModel
    {
        [Required]
        public string UserId { get; set; }

        public int ContestId { get; set; }

        public string Username { get; set; }
    }
}