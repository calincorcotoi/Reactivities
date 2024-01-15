using API.DTOs;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles;

public class Details
{
    public class Query : IRequest<Result<Profile>>
    {
        public string Username { get; set; }    
    }

    public class Handler : IRequestHandler<Query, Result<Profile>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context , IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<Profile>> Handle(Query request, CancellationToken cancellationToken)
        {
            var profile = await _context.Users
                //.Include(u => u.Photos)
                .ProjectTo<Profile>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync( u => u.Username == request.Username);

            return Result<Profile>.Success(profile);
        }
    }
}