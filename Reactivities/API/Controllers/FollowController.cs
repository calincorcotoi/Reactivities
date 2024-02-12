using Application.Followers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class FollowController:BaseApiController
    {
        [HttpPost("{username}")]
        public async Task<IActionResult> Follow(string username)
        {
            return HandlResult(await Mediator.Send(new FollowerToggle.Command() { TargetName = username }));
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetFollowers(string username, string predicate)
        {
            return HandlResult(await Mediator.Send(new List.Query() { Username = username, Predicate = predicate }));
        }
    }
}
