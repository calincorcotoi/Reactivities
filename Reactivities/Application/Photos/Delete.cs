using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos;

public class Delete
{
    public class Command : IRequest<Result<Unit>>
    {
        public string Id { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly IPhotoAccessor _photoAccessor;

        public Handler(DataContext context, IUserAccessor userAccessor, IPhotoAccessor photoAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
            _photoAccessor = photoAccessor;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.Include(u => u.Photos)
                .FirstOrDefaultAsync(u => u.UserName == _userAccessor.GetUsername());

            if(user == null) return null;

            var photoToDelete = user.Photos.FirstOrDefault(p => p.Id == request.Id);

            if (photoToDelete == null) return null;
            
            if(photoToDelete.IsMain) { return Result<Unit>.Failure("You can not delete the main photo"); }

            var resultPhotoAccessor  = _photoAccessor.DeletePhoto(photoToDelete.Id);
            
            if(resultPhotoAccessor == null) { return Result<Unit>.Failure("Failed to delete photo in cloud"); }

            user.Photos.Remove(photoToDelete);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) { return Result<Unit>.Failure("Failed to delete photo"); }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}