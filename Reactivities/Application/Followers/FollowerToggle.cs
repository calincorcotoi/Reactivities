using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Followers;

public class FollowerToggle
{
    public class Command : IRequest<Result<Unit>>
    {
        public string TargetName { get; set; }    
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;


        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var userObserver = _context.Users.FirstOrDefault(u => u.UserName == _userAccessor.GetUsername());

            if (userObserver == null) return null;

            var userTarget = _context.Users.FirstOrDefault(u => u.UserName == request.TargetName);

            if (userTarget == null) return null;

            var following = await _context.UserFollowings.FindAsync(userObserver.Id, userTarget.Id);

            if (following != null)
            {
                _context.UserFollowings.Remove(following);
            }
            else
            {
                var userFollowing = new UserFollowing()
                {
                    Observer = userObserver,
                    Target = userTarget
                };

                _context.UserFollowings.Add(userFollowing);
            }

            var result = await _context.SaveChangesAsync() > 0;

            if (result)
            {
                return Result<Unit>.Success(Unit.Value);
            }

            return Result<Unit>.Failure("Failed to toggle the following");
        }
    }
}