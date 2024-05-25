using Hydroponics.Data.Entities;

namespace Hydroponics.Tests.Fixtures;

internal class SubstratesFixture
{
    private readonly List<Substrate> _data = [
        new Substrate(){ Name = "COCO COIR" },
    new Substrate(){ Name = "SPONGE" },
    new Substrate(){ Name = "CLAY PEBBLES" },
    new Substrate(){ Name = "PLASTIC PELLETS" },
    new Substrate(){ Name = "PEAT PELLET" },
    new Substrate(){ Name = "SPHAGNUM" }
    ];

    public IEnumerable<Substrate> GetAll() => _data;
}
