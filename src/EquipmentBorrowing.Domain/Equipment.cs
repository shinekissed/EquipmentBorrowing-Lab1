namespace EquipmentBorrowing.Domain;

public class Equipment
{
    public int Id { get; }
    public string Name { get; }
    public bool IsAvailable { get; private set; }

    public Equipment(int id, string name, bool isAvailable = true)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Equipment name cannot be empty.", nameof(name));
        }

        Id = id;
        Name = name;
        IsAvailable = isAvailable;
    }

    public void MarkAsBorrowed()
    {
        if (!IsAvailable)
        {
            throw new InvalidOperationException($"Equipment '{Name}' is already borrowed.");
        }

        IsAvailable = false;
    }

    public void MarkAsReturned()
    {
        if (IsAvailable)
        {
            throw new InvalidOperationException($"Equipment '{Name}' was not marked as borrowed.");
        }

        IsAvailable = true;
    }
}
