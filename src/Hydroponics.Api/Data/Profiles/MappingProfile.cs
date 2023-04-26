using AutoMapper;
using Hydroponics.Api.Data.DataTransferObjects;
using Hydroponics.Data.Entities;

namespace Hydroponics.Api.Data.Profiles;

/// <summary>
/// automapper profile
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// constructor
    /// </summary>
    public MappingProfile()
    {
        CreateMap<NewCultivationMethod, CultivationMethod>();
        CreateMap<NewMeasure, Measure>();
        CreateMap<NewSubstrate, Substrate>();
        CreateMap<NewPot, Pot>();
        CreateMap<EditCultivationMethod, CultivationMethod>();
        CreateMap<EditMeasure, Measure>();
        CreateMap<EditSubstrate, Substrate>();
        CreateMap<EditPot, Pot>();
        CreateMap<CultivationMethod, ViewCultivationMethod>();
        CreateMap<Measure, ViewMeasure>();
        CreateMap<Substrate, ViewSubstrate>();
        CreateMap<Pot, ViewPot>();
    }
}
