using EquipmentBorrowing.Application.Services;
using EquipmentBorrowing.Infrastructure.Repositories;
using Xunit;

namespace EquipmentBorrowing.Tests;

public class BorrowEquipmentServiceTests
{
    private static BorrowEquipmentService CreateService(
        out InMemoryStudentRepository students,
        out InMemoryEquipmentRepository equipment,
        out InMemoryBorrowingRepository borrowings)
    {
        students = new InMemoryStudentRepository();
        equipment = new InMemoryEquipmentRepository();
        borrowings = new InMemoryBorrowingRepository();
        return new BorrowEquipmentService(students, equipment, borrowings);
    }

    [Fact]
    public async Task ExecuteAsync_WithAvailableEquipmentAndEligibleStudent_Succeeds()
    {
        var service = CreateService(out _, out _, out _);

        var result = await service.ExecuteAsync(
            studentId: 1,
            equipmentId: 1,
            expectedReturnDate: DateTime.UtcNow.AddDays(3));

        Assert.True(result.Success);
        Assert.NotNull(result.Borrowing);
    }

    [Fact]
    public async Task ExecuteAsync_WithUnavailableEquipment_Fails()
    {
        var service = CreateService(out _, out _, out _);

        var result = await service.ExecuteAsync(
            studentId: 1,
            equipmentId: 2,
            expectedReturnDate: DateTime.UtcNow.AddDays(3));

        Assert.False(result.Success);
        Assert.Null(result.Borrowing);
        Assert.NotNull(result.ErrorMessage);
    }

    [Fact]
    public async Task ExecuteAsync_WithStudentNotAllowedToBorrow_Fails()
    {
        var service = CreateService(out _, out _, out _);

        var result = await service.ExecuteAsync(
            studentId: 2,
            equipmentId: 1,
            expectedReturnDate: DateTime.UtcNow.AddDays(3));

        Assert.False(result.Success);
    }

    [Fact]
    public async Task ExecuteAsync_WithNonExistentStudent_Fails()
    {
        var service = CreateService(out _, out _, out _);

        var result = await service.ExecuteAsync(
            studentId: 999,
            equipmentId: 1,
            expectedReturnDate: DateTime.UtcNow.AddDays(3));

        Assert.False(result.Success);
    }
}
