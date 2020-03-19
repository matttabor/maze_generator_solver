using AutoMapper;
using Maze.Models;
using MazeApi.Models;

namespace maze_api.Mapping
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            CreateMap<MazeModel, MazeViewModel>();
            CreateMap<Cell, CellViewModel>();
            CreateMap<Door, DoorViewModel>()
                .ForMember(dest=> dest.Status, opt => opt.MapFrom(src => src.IsClosed ? DoorStatus.CLOSED : DoorStatus.OPEN));
            CreateMap<MazeViewModel, MazeModel>();
            CreateMap<CellViewModel, Cell>();
            CreateMap<DoorViewModel, Door>()
                .ConstructUsing(x => new Door(x.Status));;
        }
        
    }
}