using API.ViewModels;
using AutoMapper;
using Repository.Models;

namespace API.Mappings
{
    public class Maps: Profile
    {
        public Maps()
        {
            CreateMap<Task, TaskVM>().ReverseMap();
        }
    }
}
