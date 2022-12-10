using GitExtensions.AzureDevOpsCommitMessage;
using static GitExtensions.AzureDevOpsCommitMessage.Plugin;

namespace GitExtensions.AzureDevOpsCommitMessageTests
{
    public class PluginTests
    {
        // Taken from https://learn.microsoft.com/fr-fr/rest/api/azure/devops/wit/work-items/get-work-item?view=azure-devops-rest-7.0&tabs=HTTP#get-work-item
        private const string WorkItemContentExample =
            "{\r\n  \"id\": 12,\r\n  \"rev\": 3,\r\n  \"fields\": {\r\n    \"System.AreaPath\": \"MyAgilePrj2\",\r\n    \"System.TeamProject\": \"MyAgilePrj2\",\r\n    \"System.IterationPath\": \"MyAgilePrj2\\\\Iteration 1\",\r\n    \"System.WorkItemType\": \"User Story\",\r\n    \"System.State\": \"Active\",\r\n    \"System.Reason\": \"Implementation started\",\r\n    \"System.AssignedTo\": {\r\n      \"displayName\": \"Jamal Hartnett\",\r\n      \"url\": \"https://vssps.dev.azure.com/fabrikam/_apis/Identities/d291b0c4-a05c-4ea6-8df1-4b41d5f39eff\",\r\n      \"_links\": {\r\n        \"avatar\": {\r\n          \"href\": \"https://dev.azure.com/mseng/_apis/GraphProfile/MemberAvatars/aad.YTkzODFkODYtNTYxYS03ZDdiLWJjM2QtZDUzMjllMjM5OTAz\"\r\n        }\r\n      },\r\n      \"id\": \"d291b0c4-a05c-4ea6-8df1-4b41d5f39eff\",\r\n      \"uniqueName\": \"fabrikamfiber4@hotmail.com\",\r\n      \"imageUrl\": \"https://dev.azure.com/fabrikam/_api/_common/identityImage?id=d291b0c4-a05c-4ea6-8df1-4b41d5f39eff\",\r\n      \"descriptor\": \"aad.YTkzODFkODYtNTYxYS03ZDdiLWJjM2QtZDUzMjllMjM5OTAz\"\r\n    },\r\n    \"System.CreatedDate\": \"2017-09-04T06:11:59.05Z\",\r\n    \"System.CreatedBy\": {\r\n      \"displayName\": \"Jamal Hartnett\",\r\n      \"url\": \"https://vssps.dev.azure.com/fabrikam/_apis/Identities/d291b0c4-a05c-4ea6-8df1-4b41d5f39eff\",\r\n      \"_links\": {\r\n        \"avatar\": {\r\n          \"href\": \"https://dev.azure.com/mseng/_apis/GraphProfile/MemberAvatars/aad.YTkzODFkODYtNTYxYS03ZDdiLWJjM2QtZDUzMjllMjM5OTAz\"\r\n        }\r\n      },\r\n      \"id\": \"d291b0c4-a05c-4ea6-8df1-4b41d5f39eff\",\r\n      \"uniqueName\": \"fabrikamfiber4@hotmail.com\",\r\n      \"imageUrl\": \"https://dev.azure.com/fabrikam/_api/_common/identityImage?id=d291b0c4-a05c-4ea6-8df1-4b41d5f39eff\",\r\n      \"descriptor\": \"aad.YTkzODFkODYtNTYxYS03ZDdiLWJjM2QtZDUzMjllMjM5OTAz\"\r\n    },\r\n    \"System.ChangedDate\": \"2017-10-04T23:32:02.18Z\",\r\n    \"System.ChangedBy\": {\r\n      \"displayName\": \"Jamal Hartnett\",\r\n      \"url\": \"https://vssps.dev.azure.com/fabrikam/_apis/Identities/d291b0c4-a05c-4ea6-8df1-4b41d5f39eff\",\r\n      \"_links\": {\r\n        \"avatar\": {\r\n          \"href\": \"https://dev.azure.com/mseng/_apis/GraphProfile/MemberAvatars/aad.YTkzODFkODYtNTYxYS03ZDdiLWJjM2QtZDUzMjllMjM5OTAz\"\r\n        }\r\n      },\r\n      \"id\": \"d291b0c4-a05c-4ea6-8df1-4b41d5f39eff\",\r\n      \"uniqueName\": \"fabrikamfiber4@hotmail.com\",\r\n      \"imageUrl\": \"https://dev.azure.com/fabrikam/_api/_common/identityImage?id=d291b0c4-a05c-4ea6-8df1-4b41d5f39eff\",\r\n      \"descriptor\": \"aad.YTkzODFkODYtNTYxYS03ZDdiLWJjM2QtZDUzMjllMjM5OTAz\"\r\n    },\r\n    \"System.Title\": \"Epic 2\",\r\n    \"Microsoft.VSTS.Common.StateChangeDate\": \"2017-10-04T23:32:01.6Z\",\r\n    \"Microsoft.VSTS.Common.ActivatedDate\": \"2017-10-04T23:32:01.6Z\",\r\n    \"Microsoft.VSTS.Common.ActivatedBy\": \"Jamal Hartnett <fabrikamfiber4@hotmail.com>\",\r\n    \"Microsoft.VSTS.Common.Priority\": 2,\r\n    \"Microsoft.VSTS.Common.ValueArea\": \"Business\",\r\n    \"System.Tags\": \"client; sample; teamservices\"\r\n  },\r\n  \"_links\": {\r\n    \"self\": {\r\n      \"href\": \"https://dev.azure.com/fabrikam/_apis/wit/workItems/12\"\r\n    },\r\n    \"workItemUpdates\": {\r\n      \"href\": \"https://dev.azure.com/fabrikam/_apis/wit/workItems/12/updates\"\r\n    },\r\n    \"workItemRevisions\": {\r\n      \"href\": \"https://dev.azure.com/fabrikam/_apis/wit/workItems/12/revisions\"\r\n    },\r\n    \"workItemHistory\": {\r\n      \"href\": \"https://dev.azure.com/fabrikam/_apis/wit/workItems/12/history\"\r\n    },\r\n    \"html\": {\r\n      \"href\": \"https://dev.azure.com/fabrikam/web/wi.aspx?pcguid=20cda608-32f0-4e6e-9b7c-8def7b38d15a&id=12\"\r\n    },\r\n    \"workItemType\": {\r\n      \"href\": \"https://dev.azure.com/fabrikam/54332e84-3d54-4c67-9bd0-0e88a9849330/_apis/wit/workItemTypes/User%20Story\"\r\n    },\r\n    \"fields\": {\r\n      \"href\": \"https://dev.azure.com/fabrikam/_apis/wit/fields\"\r\n    }\r\n  },\r\n  \"url\": \"https://dev.azure.com/fabrikam/_apis/wit/workItems/12\"\r\n}";

        private readonly TestAccessor _sut = new Plugin().GetTestAccessor();

        [Fact]
        public void GetWorkItems()
        {
            var workItems = _sut.GetWorkItems("{\r\n \"workItems\": [{\r\n    \"id\": 12,\r\n    \"url\": \"https://dev.azure.com/fabrikam/_apis/wit/workItems/12\"\r\n}, {\r\n    \"id\": 25,\r\n    \"url\": \"https://dev.azure.com/fabrikam/_apis/wit/workItems/25\"\r\n}]}");

            Assert.Equal(new List<(string id, string url)> { (id: "12", url: "https://dev.azure.com/fabrikam/_apis/wit/workItems/12"), (id: "25", url: "https://dev.azure.com/fabrikam/_apis/wit/workItems/25") }, workItems);
        }

        [Theory]
        [InlineData("{id}/{System.Title}", "134/Epic 2")]
        [InlineData("{id}/{System.Title}/{System.State}", "134/Epic 2/Active")]
        [InlineData("{id}/{System.Title}/{System.CreatedBy|displayName}", "134/Epic 2/Jamal Hartnett")]
        [InlineData("{id}/{System.Title}/{System.AssignedTo|_links|avatar|href}", "134/Epic 2/https://dev.azure.com/mseng/_apis/GraphProfile/MemberAvatars/aad.YTkzODFkODYtNTYxYS03ZDdiLWJjM2QtZDUzMjllMjM5OTAz")]
        public void GetCommitTemplateFromWorkItemData(string template, string value)
        {
            var commitTemplate = _sut.GetCommitTemplateFromWorkItemData("134", template, WorkItemContentExample);

            Assert.Equal("134: Epic 2", commitTemplate.Title);
            Assert.Equal(value, commitTemplate.Text);
        }

        [Theory]
        [InlineData("{id}/{System.Title}/{System.AssignedTo|notExisting}", "134/Epic 2/{System.AssignedTo|notExisting}")]
        [InlineData("{id}/{System.Title}/{System.NotExisting}", "134/Epic 2/{System.NotExisting}")]
        public void GetCommitTemplateFromWorkItemData_PatternNotFoundIsNotReplaced(string template, string value)
        {
            var plugin = new Plugin().GetTestAccessor();
            var commitTemplate = _sut.GetCommitTemplateFromWorkItemData("134", template, WorkItemContentExample);

            Assert.Equal("134: Epic 2", commitTemplate.Title);
            Assert.Equal(value, commitTemplate.Text);
        }
    }
}