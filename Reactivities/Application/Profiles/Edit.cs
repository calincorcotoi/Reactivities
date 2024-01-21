using Application.Core;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles;

public class Edit
{
    public class Command : IRequest<Result<Unit>>
    {
        public string DisplayName { get; set; }
        public string Bio { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;

        public Handler(IMapper mapper, DataContext context, IUserAccessor userAccessor)
        {
            _mapper = mapper;
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == _userAccessor.GetUsername(),
                cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if(user == null) return null;

            user.DisplayName = request.DisplayName;

            user.Bio = request.Bio;

            if( await _context.SaveChangesAsync(cancellationToken) <= 0) return Result<Unit>.Failure("Failed to update");

            var profile = _mapper.Map<Profile>(user);

            return Result<Unit>.Success(Unit.Value);
        }
    }
}