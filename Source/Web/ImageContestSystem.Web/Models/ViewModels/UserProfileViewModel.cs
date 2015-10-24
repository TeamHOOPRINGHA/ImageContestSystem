﻿namespace ImageContestSystem.Web.Models.ViewModels
{
    using Data.Models;
    using System;
    using System.Linq.Expressions;

    public class UserProfileViewModel
    {
        public string Username { get; set; }

        public int OwnContest { get; set; }

        public int ParticipatedInContests { get; set; }

        public int WonContests { get; set; }

        public static Expression<Func<User, UserProfileViewModel>> Create
        {
            get
            {
                return u => new UserProfileViewModel
                {
                    Username = u.UserName,
                    OwnContest = u.CreatedContests.Count,
                    ParticipatedInContests = u.ParticipatedIn.Count,
                    WonContests = u.WonContests.Count
                };
            }
        }
    }
}