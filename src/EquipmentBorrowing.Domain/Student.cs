namespace EquipmentBorrowing.Domain;

public class Student
{
    public int Id { get; }
    public string Name { get; }
    public bool IsAllowedToBorrow { get; private set; }
    public int MaxActiveBorrowings { get; }

    public Student(int id, string name, bool isAllowedToBorrow = true, int maxActiveBorrowings = 3)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Student name cannot be empty.", nameof(name));
        }

        if (maxActiveBorrowings <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxActiveBorrowings), "Max active borrowings must be greater than zero.");
        }

        Id = id;
        Name = name;
        IsAllowedToBorrow = isAllowedToBorrow;
        MaxActiveBorrowings = maxActiveBorrowings;
    }

    public void Suspend() => IsAllowedToBorrow = false;

    public void Reinstate() => IsAllowedToBorrow = true;
}
