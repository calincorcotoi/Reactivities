﻿using Application.Photos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class PhotosController : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> AddPhoto([FromForm] Add.Command command)
    //public async Task<IActionResult> AddPhoto(IFormFile file)
    {
        //var result = await Mediator.Send(new Add.Command() { File = file });
        var result = await Mediator.Send(command);

        return HandlResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePhoto(string id)
    {
        var result = await Mediator.Send(new Delete.Command() { Id = id });

        return HandlResult(result);
    }

    [HttpPost("{id}/setmain")]
    public async Task<IActionResult> SetMainPhoto(string id)
    {
        var result = await Mediator.Send(new SetMain.Command() { Id = id });

        return HandlResult(result);
    }
}