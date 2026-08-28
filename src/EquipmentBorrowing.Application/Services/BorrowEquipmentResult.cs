using EquipmentBorrowing.Domain;

namespace EquipmentBorrowing.Application.Services;

public sealed class BorrowEquipmentResult
{
    public bool Success { get; }
    public string? ErrorMessage { get; }
    public Borrowing? Borrowing { get; }

    private BorrowEquipmentResult(bool success, string? errorMessage, Borrowing? borrowing)
    {
        Success = success;
        ErrorMessage = errorMessage;
        Borrowing = borrowing;
    }

    public static BorrowEquipmentResult Ok(Borrowing borrowing) => new(true, null, borrowing);

    public static BorrowEquipmentResult Fail(string errorMessage) => new(false, errorMessage, null);
}
