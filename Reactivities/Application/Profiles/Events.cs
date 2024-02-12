using Application.Core;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.DTOs;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Application.Profiles
{
    public class Events
    {
        public class Query : IRequest<Result<List<UserActivityDto>>>
        {
            public string Username { get; set; }
            public string Predicate { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<UserActivityDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _context = context;
                _mapper = mapper;
                _userAccessor = userAccessor;
            }

            public async Task<Result<List<UserActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _context.Activities
                    .Where(a => a.Attendees.Any(a => a.AppUser.UserName == _userAccessor.GetUsername()))
                    .OrderBy(a => a.Date)
                    .ProjectTo<UserActivityDto>(_mapper.ConfigurationProvider)
                    .AsQueryable();

                if (request.Predicate == "past")
                {
                    var result = await query
                        .Where(a => a.Date < DateTime.UtcNow)
                        .ToListAsync();

                    return Result<List<UserActivityDto>>.Success(result);
                }

                if (request.Predicate == "future")
                {
                    var result = await query
                        .Where(a => a.Date > DateTime.UtcNow)
                        .ToListAsync();

                    return Result<List<UserActivityDto>>.Success(result);
                }

                if (request.Predicate == "hosting")
                {
                    var result = await query
                        .Where(a => a.Date > DateTime.UtcNow)
                        .Where(a => a.HostUserName  == _userAccessor.GetUsername())
                        .ToListAsync();

                    return Result<List<UserActivityDto>>.Success(result);
                }

                return Result<List<UserActivityDto>>.Failure("Problem listing the events");
            }
        }
    }
}
