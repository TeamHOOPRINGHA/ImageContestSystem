﻿namespace ImageContestSystem.Web.Models.ViewModels
{
    using Data.Models;
    using Infrastructure.Mapping;
    using Data.Models.Enums;
    using System;
    using System.ComponentModel.DataAnnotations;
    using AutoMapper;

    public class ContestViewModel : IHaveCustomMappings
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public VotingStrategy VotingStrategy { get; set; }

        public RewardStrategy RewardStrategy { get; set; }

        public ParticipationStrategy ParticipationStrategy { get; set; }

        public DeadlineStrategy DeadlineStrategy { get; set; }

        public DateTime? ClosesOn { get; set; }

        public int? NumberOfAllowedParticipants { get; set; }

        public int CountOfParticipants { get; set; }

        public bool HasParticipated { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Contest, ContestViewModel>()
                .ForMember(c => c.CountOfParticipants, opt => opt.MapFrom(contest => contest.Participants.Count));
        }
    }
}