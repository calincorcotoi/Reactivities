﻿using Domain;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Persistence;
using System.ComponentModel.DataAnnotations;
using Application.Core;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Application.Activities;

public class Create
{
    public class Command : IRequest<Result<Unit>>
    {
        public Activity Activity { get; set; }
    }

    //because the validation is done in auto mode, this class can be deleted ,
    //but for the sake of the course will remain.
    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
        }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            _context.Activities.Add(request.Activity);

            var result = await _context.SaveChangesAsync() > 0;

            if(!result) { return Result<Unit>.Failure("Failed to create activity");}

            return Result<Unit>.Success(Unit.Value);
        }
    }
}