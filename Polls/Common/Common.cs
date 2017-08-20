using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Polls.Common
{
    public class Common
    {
        public static string ApirUrl = string.Empty;
        public static string FullImageBaseUrl { get; set; }
        public static string ThumbnailBaseUrl { get; set; }
    }
    public static class LoggedInUserDetails
    {
        public static string Username { get; set; }
        public static bool IsEmailVerified { get; set; }
    }
}