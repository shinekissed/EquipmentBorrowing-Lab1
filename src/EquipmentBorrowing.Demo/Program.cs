using EquipmentBorrowing.Application.Services;
using EquipmentBorrowing.Infrastructure.Repositories;

var studentRepository = new InMemoryStudentRepository();
var equipmentRepository = new InMemoryEquipmentRepository();
var borrowingRepository = new InMemoryBorrowingRepository();

var service = new BorrowEquipmentService(studentRepository, equipmentRepository, borrowingRepository);

Console.WriteLine("=== Campus Equipment Borrowing System - Demo ===\n");

Console.WriteLine("Case 1: Successful borrow (Student 1, Equipment 1)");
var successResult = await service.ExecuteAsync(
    studentId: 1,
    equipmentId: 1,
    expectedReturnDate: DateTime.UtcNow.AddDays(7));

PrintResult(successResult);

Console.WriteLine("\nCase 2: Failure - equipment unavailable (Student 1, Equipment 2)");
var equipmentUnavailableResult = await service.ExecuteAsync(
    studentId: 1,
    equipmentId: 2,
    expectedReturnDate: DateTime.UtcNow.AddDays(7));

PrintResult(equipmentUnavailableResult);

Console.WriteLine("\nCase 3: Failure - student not allowed to borrow (Student 2, Equipment 1)");
var studentNotAllowedResult = await service.ExecuteAsync(
    studentId: 2,
    equipmentId: 1,
    expectedReturnDate: DateTime.UtcNow.AddDays(7));

PrintResult(studentNotAllowedResult);

Console.WriteLine("\nCase 4: Failure - equipment does not exist (Student 1, Equipment 999)");
var equipmentNotFoundResult = await service.ExecuteAsync(
    studentId: 1,
    equipmentId: 999,
    expectedReturnDate: DateTime.UtcNow.AddDays(7));

PrintResult(equipmentNotFoundResult);

static void PrintResult(BorrowEquipmentResult result)
{
    if (result.Success && result.Borrowing is not null)
    {
        Console.WriteLine($"  SUCCESS -> Borrowing {result.Borrowing.Id} created. " +
                           $"Status: {result.Borrowing.Status}, " +
                           $"Expected return: {result.Borrowing.ExpectedReturnDate:d}");
    }
    else
    {
        Console.WriteLine($"  FAILED -> {result.ErrorMessage}");
    }
}
