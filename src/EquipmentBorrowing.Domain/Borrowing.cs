namespace EquipmentBorrowing.Domain;

public class Borrowing
{
    public Guid Id { get; }
    public int StudentId { get; }
    public int EquipmentId { get; }
    public DateTime DateBorrowed { get; }
    public DateTime ExpectedReturnDate { get; }
    public BorrowingStatus Status { get; private set; }

    public Borrowing(Guid id, int studentId, int equipmentId, DateTime dateBorrowed, DateTime expectedReturnDate)
    {
        if (expectedReturnDate < dateBorrowed)
        {
            throw new ArgumentException("Expected return date cannot be earlier than the borrow date.", nameof(expectedReturnDate));
        }

        Id = id;
        StudentId = studentId;
        EquipmentId = equipmentId;
        DateBorrowed = dateBorrowed;
        ExpectedReturnDate = expectedReturnDate;
        Status = BorrowingStatus.Active;
    }

    public void MarkAsReturned()
    {
        if (Status == BorrowingStatus.Returned)
        {
            throw new InvalidOperationException("This borrowing has already been marked as returned.");
        }

        Status = BorrowingStatus.Returned;
    }
}
