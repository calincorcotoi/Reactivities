using API.DTOs;
using Application.Activities;
using AutoMapper;
using Domain;

namespace Application.Core;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Activity, Activity>();

        CreateMap<Activity, ActivityDto>().ForMember(d => d.HostUsername,
            o => o.MapFrom(s => 
                s.Attendees.FirstOrDefault(a => a.IsHost).AppUser.UserName));

        CreateMap<ActivityAttendee, Profiles.Profile>()
            .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.AppUser.DisplayName))
            .ForMember(d => d.Username, o => o.MapFrom(s => s.AppUser.UserName))
            .ForMember(d => d.Bio, o => o.MapFrom(s => s.AppUser.Bio));

        CreateMap<ActivityAttendee, AttendeeDto>()
            .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.AppUser.DisplayName))
            .ForMember(d => d.Username, o => o.MapFrom(s => s.AppUser.UserName))
            .ForMember(d => d.Bio, o => o.MapFrom(s => s.AppUser.Bio))
            .ForMember(d => d.Image, o => o.MapFrom(s => s.AppUser.Photos.FirstOrDefault(p => p.IsMain).Url));

        //auto mapper is smart
        CreateMap<AppUser, Profiles.Profile>()
            //.ForMember(d => d.DisplayName, o => o.MapFrom(s => s.DisplayName))
            //.ForMember(d => d.Username, o => o.MapFrom(s => s.UserName))
            //.ForMember(d => d.Bio, o => o.MapFrom(s => s.Bio))
            .ForMember(d => d.Image, o => o.MapFrom(s => s.Photos.FirstOrDefault(p => p.IsMain).Url));
            //.ForMember(d => d.Photos, o => o.MapFrom(s => s.Photos));
    }
}