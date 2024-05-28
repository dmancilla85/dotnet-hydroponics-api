using Hydroponics.Data.Entities;

namespace Hydroponics.Tests.Fixtures;

internal class PotsFixture
{
    private readonly List<Pot> _data = [
        new Pot
        {
            Name = "WASP1",
            Height = 0.45m,
            Length = 0.34m,
            Width = 0.26m,
            Liters = 20,
            Status = true
        },
        new Pot
        {
            Name = "WASP3",
            Height = 0.45m,
            Length = 0.34m,
            Width = 0.26m,
            Liters = 20,
            Status = true
        },
        new Pot
        {
            Name = "PIRANHA1",
            Height = 0.45m,
            Length = 0.34m,
            Width = 0.26m,
            Liters = 20,
            Status = true
        },
        new Pot
        {
            Name = "PIRANHA2",
            Height = 0.45m,
            Length = 0.34m,
            Width = 0.26m,
            Liters = 20,
            Status = true
        }
    ];

    public IEnumerable<Pot> GetAll() => _data;
}
