using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;

namespace OpenApiBugRepro
{
    public sealed class GroupModel
    {
        public GroupModel? ChildGroup { get; set; }
        public UserModel? Owner { get; set; }
        public UserModel? CoOwner { get; set; }
    }

    public sealed class UserModel
    {
        public Guid Id { get; set; }
    }

    public sealed class GetGroupFunction
    {
        [OpenApiOperation("GetGroup", "Groups", Summary = "Get a group", Description = "Get a group.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(GroupModel), Summary = "Created", Description = "The group was retrieved.")]
        [FunctionName("GetGroup")]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "groups/test")]
            HttpRequest req,
            ILogger log,
            CancellationToken cancellationToken)
        {
            // Simulate some async.
            await Task.Delay(500, cancellationToken);

            return new OkObjectResult(new GroupModel());
        }
    }
}
