namespace GMS
{
    class Program
    {
        static void Main(string[] args)
        {
            var helper = new GitLabHelper("https:\\gitlab.com", "<secret key>");
            helper.CloseOldMilestones()
                .OpenUpcomingMilestones()
                .MoveIssuesFromClosedMilestonesToUpcomingMilestone();
        }
    }
}