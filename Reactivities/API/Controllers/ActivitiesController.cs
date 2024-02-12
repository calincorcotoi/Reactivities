using Application.Activities;
using Application.Core;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ActivitiesController : BaseApiController
{

    [HttpGet]
    public async Task<IActionResult> GetActivities([FromQuery] ActivityParams param)
    {
        var activities = await Mediator.Send(new List.Query(){Params = param});
        return HandlePagedResult(activities);
    }

    [HttpGet("{id}")] //api/activities/121bc2f5-56c4-410d-b19b-dae3938fbc81
    public async Task<IActionResult> GetActivity(Guid id)
    {
        var result = await Mediator.Send(new Details.Query { Id = id });

        return HandlResult(result);
    } 

    [HttpPost]
    public async Task<IActionResult> CreateActivity(Activity activity)
    {
        var result = await Mediator.Send(new Create.Command { Activity = activity });
        return HandlResult(result);
    }

    [Authorize(Policy = "IsActivityHost")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Edit(Guid id, Activity activity)
    {
        activity.Id = id;

       var result = await Mediator.Send(new Edit.Command { Activity = activity });

        return HandlResult(result);
    }

    [Authorize(Policy = "IsActivityHost")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await Mediator.Send(new Delete.Command { Id = id });

        return HandlResult(result);
    }

    [HttpPost("{id}/attend")]
    public async Task<IActionResult> Attend(Guid id)
    {
        var result = await Mediator.Send(new UpdateAttendance.Command() { Id = id });

        return HandlResult(result);
    }
}