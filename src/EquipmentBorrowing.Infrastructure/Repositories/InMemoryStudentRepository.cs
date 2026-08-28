using EquipmentBorrowing.Application.Interfaces;
using EquipmentBorrowing.Domain;

namespace EquipmentBorrowing.Infrastructure.Repositories;

public class InMemoryStudentRepository : IStudentRepository
{
    private readonly Dictionary<int, Student> _students = new();

    public InMemoryStudentRepository()
    {
        Add(new Student(id: 1, name: "Juan Dela Cruz", isAllowedToBorrow: true, maxActiveBorrowings: 2));
        Add(new Student(id: 2, name: "Maria Santos", isAllowedToBorrow: false, maxActiveBorrowings: 3));
    }

    public void Add(Student student) => _students[student.Id] = student;

    public Task<Student?> GetByIdAsync(int studentId, CancellationToken cancellationToken = default)
    {
        _students.TryGetValue(studentId, out var student);
        return Task.FromResult(student);
    }
}
