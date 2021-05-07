using System;

namespace RedmineCore
{
    public static class Constants
    {
        //Change below urls accordingly based on your custom issues on your owned projects
        public static string BaseURL { get; } = "https://dev.unosquare.com/redmine/login?back_url=https%3A%2F%2Fdev.unosquare.com%2Fredmine%2F";
        public static string TaskURLAutomation { get; } = "https://dev.unosquare.com/redmine/issues/59204";
        public static string TaskURLNonProject { get; } = "https://dev.unosquare.com/redmine/projects/axos-bank-qa-automation-non-project-work";
        public static string Username { get; } = "you.user@unosquare.com";
        public static string Password { get; } = "password";
        public static string CommentsMeeting { get; } = "UDB Automation Meeting";
        public static string CommentsDesign { get; } = "UDB Script Development";
        public static string CommentsTesting { get; } = "UDB Test Case Execution";
        public static string CommentsOther { get; } = "SDET Unosquare moodle course";
        public static string CommentsDevelopment1 { get; } = "UDB pull request send and review";
        public static string CommentsDevelopment2 { get; } = "Automation frameworks maintenance";
    }
}
