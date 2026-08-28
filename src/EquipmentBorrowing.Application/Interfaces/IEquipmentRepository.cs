using EquipmentBorrowing.Domain;

namespace EquipmentBorrowing.Application.Interfaces;

public interface IEquipmentRepository
{
    Task<Equipment?> GetByIdAsync(int equipmentId, CancellationToken cancellationToken = default);

    Task UpdateAsync(Equipment equipment, CancellationToken cancellationToken = default);
}
