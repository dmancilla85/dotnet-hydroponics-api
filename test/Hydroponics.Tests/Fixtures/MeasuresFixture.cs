using Hydroponics.Data.Entities;

namespace Hydroponics.Tests.Fixtures;

internal class MeasuresFixture
{
    private readonly List<Measure> _data = [
        new Measure(){ Name = "COCO COIR" },
        new Measure(){ Name = "SPONGE" },
        new Measure(){ Name = "CLAY PEBBLES" },
        new Measure(){ Name = "PLASTIC PELLETS" },
        new Measure(){ Name = "PEAT PELLET" },
];

    public IEnumerable<Measure> GetAll() => _data;
}
