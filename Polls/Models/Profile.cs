using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Polls.Models
{   
    public class Profile
    {
        public string id { get; set; }
        public string userName { get; set; }
        public string profileId { get; set; }
        public string status { get; set; }
        public bool isEmailVerified { get; set; }
        public string token { get; set; }
        public bool isPhoneVerified { get; set; }
        public bool isCallVerified { get; set; }
        public string source { get; set; }
        public string displayName { get; set; }
        public int userType { get; set; }
        public bool buyCompanyPolls { get; set; }
        public bool buyTurkPolls { get; set; }
        public bool isRequiedApproval { get; set; }
        public string org { get; set; }
        public string emailInvites { get; set; }
        public Newprofilecontact newProfileContact { get; set; }
        public Newdevice newDevice { get; set; }
        public string emailAddress { get; set; }
        public string userId { get; set; }
        public bool isActive { get; set; }
        public string userRole { get; set; }
        public bool isCanRunFreePoll { get; set; }
        public bool isBuyCredits { get; set; }
        public bool isPartner { get; set; }
        public int departmentId { get; set; }
        public DateTime lastLogin { get; set; }
        public DateTime createDate { get; set; }
    }

    public class Newprofilecontact
    {
        public string id { get; set; }
        public string companyName { get; set; }
        public string jobTitle { get; set; }
        public string educationLevel { get; set; }
        public string ownerId { get; set; }
        public string relationshipStatus { get; set; }
        public string maritalStatus { get; set; }
        public string pictureId { get; set; }
        public bool isPublicProfile { get; set; }
        public bool isPhonePublic { get; set; }
        public bool isEmailPublic { get; set; }
        public int viewCounter { get; set; }
        public int phoneType { get; set; }
        public string userId { get; set; }
        public int countryCode { get; set; }
        public string phoneNumber { get; set; }
        public string gender { get; set; }
        public string birthDate { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string addressCity { get; set; }
        public string addressCountry { get; set; }
        public string addressState { get; set; }
        public string addressZip { get; set; }
        public string pictureUrl { get; set; }
        public Profilepictures profilePictures { get; set; }
        public bool isEmailVerified { get; set; }
        public bool isPhoneVerified { get; set; }
        public int publicPolls { get; set; }
        public int businessPolls { get; set; }
        public int privatePolls { get; set; }
        public int totalPolls { get; set; }
        public string[] deletedPictures { get; set; }
        public DateTime lastLogin { get; set; }
        public DateTime memberSince { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string payPalEmail { get; set; }
        public DateTime phoneVerifiedDateTime { get; set; }
        public string phoneVerificationMethod { get; set; }
        public DateTime emailVerifiedDateTime { get; set; }
    }

    public class Profilepictures
    {
        public string af2454b966884befa898fe4518e71963 { get; set; }
        public string fdddc6a1b9384c189950b12cc9af73e2 { get; set; }
    }

   

}