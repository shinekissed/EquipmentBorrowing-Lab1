using EquipmentBorrowing.Application.Interfaces;
using EquipmentBorrowing.Domain;

namespace EquipmentBorrowing.Infrastructure.Repositories;

public class InMemoryEquipmentRepository : IEquipmentRepository
{
    private readonly Dictionary<int, Equipment> _equipment = new();

    public InMemoryEquipmentRepository()
    {
        Add(new Equipment(id: 1, name: "Digital Multimeter", isAvailable: true));
        Add(new Equipment(id: 2, name: "Oscilloscope", isAvailable: false));
    }

    public void Add(Equipment equipment) => _equipment[equipment.Id] = equipment;

    public Task<Equipment?> GetByIdAsync(int equipmentId, CancellationToken cancellationToken = default)
    {
        _equipment.TryGetValue(equipmentId, out var equipment);
        return Task.FromResult(equipment);
    }

    public Task UpdateAsync(Equipment equipment, CancellationToken cancellationToken = default)
    {
        _equipment[equipment.Id] = equipment;
        return Task.CompletedTask;
    }
}
