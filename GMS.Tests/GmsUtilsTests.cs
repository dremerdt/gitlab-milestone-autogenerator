using System;
using GMS.Utils;
using Xunit;

namespace GMS.Tests
{
    public class GmsUtilsTests
    {
        [Fact]
        public void IsWeekNumber_ShouldMatchWeekNumber_Matched()
        {
            var isNumber = GmsUtils.IsWeekNumber("2021w31");
            var wrongNumber = GmsUtils.IsWeekNumber("August 21");
            
            Assert.True(isNumber);
            Assert.False(wrongNumber);
        }

        [Fact]
        public void GetMilestoneNumber_ShouldGenerateMilestoneNumber_Generated()
        {
            var date = new DateTime(2021, 08, 22);

            var milestoneNumber = GmsUtils.GetMilestoneNumber(date);
            
            Assert.Equal("2021w33", milestoneNumber);
        }

        [Fact]
        public void GetMilestoneNumber_ShouldGenerateMilestoneNumberZeroBased_Generated()
        {
            var date = new DateTime(2021, 01, 4);

            var milestoneNumber = GmsUtils.GetMilestoneNumber(date);
            
            Assert.Equal("2021w01", milestoneNumber);
        }
    }
}