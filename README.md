# Campus Equipment Borrowing System

ITSD 81 – Desktop Application Development
Laboratory Activity 1: From Requirements to Application Structure

## 1. Solution Structure

We split the solution into four main projects plus a test project, following the layered structure from the lab instructions.

**EquipmentBorrowing.Domain** holds the core concepts of the problem itself: Student, Equipment, Borrowing, and BorrowingStatus. These classes hold their own data and enforce the rules that belong to them specifically (for example, equipment can't be marked as borrowed twice in a row, and a borrowing can't be returned twice). This project doesn't depend on anything else in the solution.

**EquipmentBorrowing.Application** contains the actual use case logic. This is where BorrowEquipmentService lives, along with the repository interfaces (IStudentRepository, IEquipmentRepository, IBorrowingRepository) that describe what data the use case needs without saying how that data is actually stored.

**EquipmentBorrowing.Infrastructure** has the concrete implementations of those repository interfaces. Right now that just means in-memory versions (InMemoryStudentRepository, InMemoryEquipmentRepository, InMemoryBorrowingRepository) that store data in dictionaries/lists. If we added SQLite or EF Core later, this is the only project that would need new classes.

**EquipmentBorrowing.Demo** is a small console program that creates the in-memory repositories, passes them into BorrowEquipmentService, and runs a few borrow attempts to show it working. This stands in for where a real desktop UI (Avalonia) would eventually go.

**EquipmentBorrowing.Tests** has xUnit tests for BorrowEquipmentService covering both the success case and a few failure cases.

## 2. Dependency Direction
 EquipmentBorrowing.Demo (Executable / Future UI)
              │
              ▼
 EquipmentBorrowing.Application
              │
              ▼
   EquipmentBorrowing.Domain
              ▲
              │


Domain doesn't depend on anything else. Application depends only on Domain, since it works with Student, Equipment, and Borrowing objects and defines the repository interfaces that Infrastructure has to implement. Infrastructure depends on both Domain (to build the domain objects) and Application (to implement its interfaces) — but Application never references Infrastructure directly, it only knows about the interfaces. Demo depends on Application and Infrastructure because it's the piece responsible for actually creating the concrete repositories and wiring everything together.

## 3. Use Case Mapping
```text
Actor: Student
Use Case: Borrow Equipment
Application Service: BorrowEquipmentService
Domain Objects Used: Student, Equipment, Borrowing, BorrowingStatus
Repository Interfaces Used: IStudentRepository, IEquipmentRepository, IBorrowingRepository
Infrastructure Implementations Used: InMemoryStudentRepository, InMemoryEquipmentRepository, InMemoryBorrowingRepository
```

## 4. Reflection

**Why should the application service depend on a repository interface instead of directly depending on a database implementation?**

Because the use case itself (check if the student and equipment are valid, then create a borrowing) doesn't actually care where the data comes from. If BorrowEquipmentService depended directly on something like a SQLite connection, then every part of the app that uses this service would be forced to depend on SQLite too, and switching to a different storage method later would mean touching the business logic itself instead of just writing a new repository class.

**Which parts of your current solution could remain unchanged if SQLite were added later?**

Domain and Application wouldn't need to change at all. We'd just add something like SqliteStudentRepository in Infrastructure that implements the same interfaces, and update the composition code (currently in Program.cs) to use the new class instead of the in-memory one.

**Which project would eventually contain Avalonia Views?**

A new project, probably something like EquipmentBorrowing.Desktop, sitting where EquipmentBorrowing.Demo is now. It would depend on Application and Infrastructure the same way Demo does now.

**Should an Avalonia button directly execute database queries? Why or why not?**

No. A button click should call into an Application service, the same way Program.cs calls BorrowEquipmentService.ExecuteAsync. If the UI ran database queries directly, the borrowing rules (student eligibility, equipment availability, borrowing limits) would either end up duplicated in the UI code or skipped entirely, and there'd be no way to reuse or test that logic separately from the UI — which is exactly why our tests can run BorrowEquipmentService without any UI at all.

**What part of your implementation represents the actual business operation requested by the actor?**

BorrowEquipmentService.ExecuteAsync. That's the one place where all six borrowing rules from the scenario actually get checked, and where the Borrowing record itself gets created. Everything else in the solution exists to support that operation.