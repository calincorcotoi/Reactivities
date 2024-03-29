﻿using API.DTOs;
using Application.Profiles;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProfilesController : BaseApiController
{
    [HttpGet("{username}/activities")]
    public async Task<IActionResult> GetEvents(string username, string predicate)
    {
        var events = await Mediator.Send(new Events.Query { Username = username , Predicate = predicate });
        return HandlResult(events);
    }

    [HttpGet("{username}")]
    public async Task<IActionResult> GetProfile(string username)
    {
        var profile = await Mediator.Send(new Details.Query{Username = username});
        return HandlResult(profile);
    }

    [HttpPut]
    public async Task<IActionResult> EditProfile(EditProfileDto newProfile)
    {
        var profile = await Mediator.Send(new Edit.Command(){DisplayName = newProfile.DisplayName, Bio = newProfile.Bio});
        return HandlResult(profile);
    }
}