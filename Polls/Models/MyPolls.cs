using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Polls.Models
{
    public class MyPolls
    {
        public int alllowedAnonymously { get; set; }
        public bool isEnabledComments { get; set; }
        public int demographicCompleted { get; set; }
        public int pollId { get; set; }
        public string hitId { get; set; }
        public int pollTypeId { get; set; }
        public DateTime createDate { get; set; }
        public string pollStatus { get; set; }
        public string pollTitle { get; set; }
        public string question { get; set; }
        public string keywords { get; set; }
        public DateTime expirationDate { get; set; }
        public string pollOverview { get; set; }
        public string firstOption { get; set; }
        public string secondOption { get; set; }
        public string firstImagePath { get; set; }
        public string secondImagePath { get; set; }
        public int assignmentDurationInSeconds { get; set; }
        public int maxAssignments { get; set; }
        public bool isPublished { get; set; }
        public int lifeTimeInSeconds { get; set; }
        public int choiceFirstCounter { get; set; }
        public int choiceSecondCounter { get; set; }
        public int resultCounter { get; set; }
        public bool isFilterOption { get; set; }
        public string filterMainCategory { get; set; }
        public string filtersJson { get; set; }
        public bool isEnablePublic { get; set; }
        public int catId { get; set; }
        public string catName { get; set; }
        public string userName { get; set; }
        public string ownerId { get; set; }
        public int totalCount { get; set; }
        public bool isAdult { get; set; }
        public DateTime completedOn { get; set; }
        public string tags { get; set; }
        public int responseCompleted { get; set; }
    }
}