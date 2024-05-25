using Hydroponics.Data.Entities;

namespace Hydroponics.Tests.Fixtures;

internal class CultivationMethodsFixture
{
    private readonly List<CultivationMethod> _data = [
        new CultivationMethod(){ Name = "WICK SYSTEM" },
        new CultivationMethod(){ Name = "DWC" },
        new CultivationMethod(){ Name = "RDWC" },
        new CultivationMethod(){ Name = "DRIP SYSTEM" },
        new CultivationMethod(){ Name = "NUTRIENT FILM" },
        ];

    public IEnumerable<CultivationMethod> GetAll() => _data;
}
