using System;
using System.Collections.Generic;
using System.Linq;
using GitLabApiClient;
using GitLabApiClient.Models.Issues.Requests;
using GitLabApiClient.Models.Issues.Responses;
using GitLabApiClient.Models.Milestones.Requests;
using GitLabApiClient.Models.Milestones.Responses;
using GitLabApiClient.Models.Projects.Requests;
using GitLabApiClient.Models.Projects.Responses;
using GMS.Utils;

namespace GMS
{
    public class GitLabHelper
    {
        #region Fields

        private GitLabClient _client;
        private List<Issue> _issuesOfClosedMilestones;

        #endregion
        
        #region Constructors

        public GitLabHelper()
        {
            
        }
        
        public GitLabHelper(string url, string secretKey)
        {
            _client = new GitLabClient(url, secretKey);
            _issuesOfClosedMilestones = new List<Issue>();
        }

        #endregion

        #region Public interfaces

        public GitLabHelper Initialize(string url, string secretKey)
        {
            _client = new GitLabClient(url, secretKey);
            _issuesOfClosedMilestones = new List<Issue>();

            return this;
        }

        public GitLabHelper CloseOldMilestones()
        {
            _issuesOfClosedMilestones.Clear();
            var closingMilestones = GetClosingMilestones();

            foreach (var closingMilestone in closingMilestones)
            {
                var issues = _client.Issues.GetAllAsync(closingMilestone.ProjectId, null,
                    o =>
                    {
                        o.MilestoneTitle = closingMilestone.Title;
                        o.State = IssueState.Opened;
                    }).Result;
                _issuesOfClosedMilestones.AddRange(issues);
                _client.Projects.UpdateMilestoneAsync(closingMilestone.ProjectId, closingMilestone.Id,
                    new UpdateProjectMilestoneRequest
                    {
                        State = UpdatedMilestoneState.Close
                    });
            }
            
            return this;
        }

        public GitLabHelper OpenUpcomingMilestones()
        {
            var upcomingDateStart = DateTimeUtils.GetNextWeekday(DateTime.Now, DayOfWeek.Monday);
            var upcomingDateEnd = DateTimeUtils.GetNextWeekday(DateTime.Now, DayOfWeek.Sunday);
            var upcomingMilestone = GmsUtils.GetMilestoneNumber(upcomingDateStart);
            foreach (var projectId in GetProjectIds())
            {
                var milestones = _client.Projects.GetMilestonesAsync(projectId, o =>
                {
                    o.Search = upcomingMilestone;
                }).Result;

                var issues = _client.Issues.GetAllAsync(projectId, null, o => o.State = IssueState.Opened).Result;

                if (!milestones.Any() && issues.Any())
                {
                    _client.Projects.CreateMilestoneAsync(projectId,
                        new CreateProjectMilestoneRequest(upcomingMilestone)
                        {
                            StartDate = upcomingDateStart.ToString("yyyy-MM-dd"),
                            DueDate = upcomingDateEnd.ToString("yyyy-MM-dd")
                        });
                }
            }

            return this;
        }

        public GitLabHelper MoveIssuesFromClosedMilestonesToUpcomingMilestone(string[] excludeLabels = null)
        {
            var upcomingMilestone = GmsUtils.GetMilestoneNumber(DateTimeUtils.GetNextWeekday(DateTime.Now));
            var query = _issuesOfClosedMilestones;
            
            if (excludeLabels != null)
            {
                query = query.Where(x => !excludeLabels.Any(y => x.Labels.Contains(y))).ToList();
            }
            
            var groupedIssues = query.GroupBy(x => x.ProjectId)
                .ToDictionary(k => k.Key, v => v.ToList());
            
            foreach (var (projectId, issues) in groupedIssues)
            {
                var milestone =
                    _client.Projects.GetMilestonesAsync(projectId, o => o.Search = upcomingMilestone).Result
                        .First();
                foreach (var issue in issues)
                {
                    _client.Issues.UpdateAsync(issue.ProjectId, issue.Iid, new UpdateIssueRequest
                    {
                        MilestoneId = milestone.Id
                    });
                }
            }
            return this;
        }
        
        public GitLabHelper MoveAllIssuesToUpcomingMilestone()
        {
            var upcomingMilestone = GmsUtils.GetMilestoneNumber(DateTimeUtils.GetNextWeekday(DateTime.Now));

            var groupedIssues = _client.Issues.GetAllAsync(null, null, o => { 
                    o.State = IssueState.Opened;
                }).Result.Where(x => x.Milestone != null && x.Milestone.Title != upcomingMilestone).GroupBy(x => x.ProjectId)
                .ToDictionary(k => k.Key, v => v.ToList());
            foreach (var (projectId, issues) in groupedIssues)
            {
                var milestone =
                    _client.Projects.GetMilestonesAsync(projectId, o => o.Search = upcomingMilestone).Result
                        .First();
                foreach (var issue in issues)
                {
                    _client.Issues.UpdateAsync(issue.ProjectId, issue.Iid, new UpdateIssueRequest
                    {
                        MilestoneId = milestone.Id
                    });
                }
            }
            return this;
        }

        #endregion

        #region Private Methods

        private IList<Project> GetProjects()
        {
            return _client.Projects.GetAsync().Result;
        }
        
        private IList<int> GetProjectIds()
        {
            return _client.Projects.GetAsync().Result.Select(x => x.Id).ToList();
        }

        private IList<Milestone> GetClosingMilestones()
        {
            var milestones = new List<Milestone>();
            
            foreach (var project in GetProjects())
            {
                var m = _client.Projects.GetMilestonesAsync(project, o => { o.State = MilestoneState.Active; }).Result;
                milestones.AddRange(m);
            }

            var currentDate = DateTime.Now.Date;
            return milestones.Where(x =>
                !string.IsNullOrEmpty(x.DueDate) 
                && GmsUtils.IsWeekNumber(x.Title) 
                && DateTimeUtils.ParseToDate(x.DueDate) <= currentDate).ToList();
        }

        #endregion
    }
}