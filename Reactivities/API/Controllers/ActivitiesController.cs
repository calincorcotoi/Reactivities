using Application.Activities;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ActivitiesController : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<List<Activity>>> GetActivities()
    {
        var activities = await Mediator.Send(new List.Query());
        return Ok(activities);
    }

    [HttpGet("{id}")] //api/activities/121bc2f5-56c4-410d-b19b-dae3938fbc81
    public async Task<ActionResult<Activity>> GetActivity(Guid id)
    {
        return await Mediator.Send(new Details.Query { Id = id });
    } 

    [HttpPost]
    public async Task<IActionResult> CreateActivity(Activity activity)
    {
        await Mediator.Send(new Create.Command { Activity = activity });
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Edit(Guid id, Activity activity)
    {
        activity.Id = id;

        await Mediator.Send(new Edit.Command { Activity = activity });

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Edit(Guid id)
    {
        await Mediator.Send(new Delete.Command { Id = id });

        return Ok();
    }
}