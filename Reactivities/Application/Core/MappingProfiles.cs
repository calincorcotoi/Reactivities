using API.DTOs;
using Application.Activities;
using Application.Comments;
using Application.Profiles;
using Domain;
using Profile = AutoMapper.Profile;

namespace Application.Core;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        string currentUsername = null;

        CreateMap<Activity, Activity>();

        CreateMap<Activity, ActivityDto>().ForMember(d => d.HostUsername,
            o => o.MapFrom(s => 
                s.Attendees.FirstOrDefault(a => a.IsHost).AppUser.UserName));

        CreateMap<ActivityAttendee, Profiles.Profile>()
            .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.AppUser.DisplayName))
            .ForMember(d => d.Username, o => o.MapFrom(s => s.AppUser.UserName))
            .ForMember(d => d.Bio, o => o.MapFrom(s => s.AppUser.Bio))
            .ForMember(d => d.FollowersCount, o => o.MapFrom(s => s.AppUser.Followers.Count))
            .ForMember(d => d.FollowingCount, o => o.MapFrom(s => s.AppUser.Followings.Count))
            .ForMember(d => d.Following, o => o.MapFrom(s => s.AppUser.Followers.Any(s => s.Observer.UserName == currentUsername)));

        CreateMap<ActivityAttendee, AttendeeDto>()
            .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.AppUser.DisplayName))
            .ForMember(d => d.Username, o => o.MapFrom(s => s.AppUser.UserName))
            .ForMember(d => d.Bio, o => o.MapFrom(s => s.AppUser.Bio))
            .ForMember(d => d.Image, o => o.MapFrom(s => s.AppUser.Photos.FirstOrDefault(p => p.IsMain).Url))
            .ForMember(d => d.FollowersCount, o => o.MapFrom(s => s.AppUser.Followers.Count))
            .ForMember(d => d.FollowingCount, o => o.MapFrom(s => s.AppUser.Followings.Count))
            .ForMember(d => d.Following, o => o.MapFrom(s => s.AppUser.Followers.Any(s => s.Observer.UserName == currentUsername)));

        CreateMap<Activity, UserActivityDto>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
            .ForMember(d => d.Title, o => o.MapFrom(s => s.Title))
            .ForMember(d => d.Category, o => o.MapFrom(s => s.Category))
            .ForMember(d => d.Date, o => o.MapFrom(s => s.Date))
            .ForMember(d => d.HostUserName,
                o => o.MapFrom(s =>
                    s.Attendees.FirstOrDefault(a => a.IsHost).AppUser.UserName));

        //auto mapper is smart
        CreateMap<AppUser, Profiles.Profile>()
            //.ForMember(d => d.DisplayName, o => o.MapFrom(s => s.DisplayName))
            //.ForMember(d => d.Username, o => o.MapFrom(s => s.UserName))
            //.ForMember(d => d.Bio, o => o.MapFrom(s => s.Bio))
            .ForMember(d => d.Image, o => o.MapFrom(s => s.Photos.FirstOrDefault(p => p.IsMain).Url))
            .ForMember(d => d.FollowersCount, o => o.MapFrom(s => s.Followers.Count))
            .ForMember(d => d.FollowingCount, o => o.MapFrom(s => s.Followings.Count))
            .ForMember(d => d.Following, o => o.MapFrom(s => s.Followers.Any( s=> s.Observer.UserName == currentUsername)));

            CreateMap<Comment, CommentDto>()
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.Author.DisplayName))
                .ForMember(d => d.Username, o => o.MapFrom(s => s.Author.UserName))
                .ForMember(d => d.CreatedAt, o => o.MapFrom(s => s.CreatedAt))
                .ForMember(d => d.Image, o => o.MapFrom(s => s.Author.Photos.FirstOrDefault(p => p.IsMain).Url));
    }
}