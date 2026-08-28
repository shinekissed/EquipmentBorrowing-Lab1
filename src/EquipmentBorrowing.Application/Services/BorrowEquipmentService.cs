using EquipmentBorrowing.Application.Interfaces;
using EquipmentBorrowing.Domain;

namespace EquipmentBorrowing.Application.Services;

public class BorrowEquipmentService
{
    private readonly IStudentRepository _studentRepository;
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly IBorrowingRepository _borrowingRepository;

    public BorrowEquipmentService(
        IStudentRepository studentRepository,
        IEquipmentRepository equipmentRepository,
        IBorrowingRepository borrowingRepository)
    {
        _studentRepository = studentRepository;
        _equipmentRepository = equipmentRepository;
        _borrowingRepository = borrowingRepository;
    }

    public async Task<BorrowEquipmentResult> ExecuteAsync(
        int studentId,
        int equipmentId,
        DateTime expectedReturnDate,
        CancellationToken cancellationToken = default)
    {

        var student = await _studentRepository.GetByIdAsync(studentId, cancellationToken);
        if (student is null)
        {
            return BorrowEquipmentResult.Fail($"Student with id {studentId} does not exist.");
        }

        if (!student.IsAllowedToBorrow)
        {
            return BorrowEquipmentResult.Fail($"Student '{student.Name}' is not currently allowed to borrow equipment.");
        }

        var equipment = await _equipmentRepository.GetByIdAsync(equipmentId, cancellationToken);
        if (equipment is null)
        {
            return BorrowEquipmentResult.Fail($"Equipment with id {equipmentId} does not exist.");
        }

        if (!equipment.IsAvailable)
        {
            return BorrowEquipmentResult.Fail($"Equipment '{equipment.Name}' is not currently available.");
        }

        var activeCount = await _borrowingRepository.CountActiveBorrowingsByStudentAsync(studentId, cancellationToken);
        if (activeCount >= student.MaxActiveBorrowings)
        {
            return BorrowEquipmentResult.Fail(
                $"Student '{student.Name}' has reached the maximum of {student.MaxActiveBorrowings} active borrowings.");
        }

        equipment.MarkAsBorrowed();
        await _equipmentRepository.UpdateAsync(equipment, cancellationToken);

        var borrowing = new Borrowing(
            id: Guid.NewGuid(),
            studentId: student.Id,
            equipmentId: equipment.Id,
            dateBorrowed: DateTime.UtcNow,
            expectedReturnDate: expectedReturnDate);

        await _borrowingRepository.AddAsync(borrowing, cancellationToken);

        return BorrowEquipmentResult.Ok(borrowing);
    }
}
