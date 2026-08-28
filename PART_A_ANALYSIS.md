# Part A: Analysis

## A. Actors

**Student**
Expects the system to let them request to borrow available equipment, tell
them clearly if they're not eligible, and track what they currently have
borrowed.

**Technician (Lab Staff)**
Expects the system to keep an accurate record of which equipment is
currently borrowed or available, who has each item, and when it's due back,
so they can reliably manage the lab's equipment.

## B. Use Cases

**Use Case 1**

| Item | Description |
|---|---|
| Use Case | Borrow Equipment |
| Primary Actor | Student |
| Preconditions | The student is registered in the system, and the equipment record exists in the system. |
| Main Action | The student requests to borrow a specific piece of equipment. |
| Expected Result | The system creates a borrowing record containing the student, the equipment, the date borrowed, and the expected return date, with status set to Active. The equipment becomes unavailable. |
| Possible Failure | The student is not allowed to borrow the equipment; the equipment doesn't exist; the equipment is not available; the student already has the maximum number of active borrowings. |

**Use Case 2**

| Item | Description |
|---|---|
| Use Case | Return Equipment |
| Primary Actor | Student |
| Preconditions | An Active borrowing record exists linking this student and this equipment. |
| Main Action | The student returns the borrowed piece of equipment. |
| Expected Result | The borrowing record's status is updated to Returned, and the equipment becomes available again. |
| Possible Failure | No matching Active borrowing record exists for that student/equipment pair (e.g., wrong equipment specified, or it was already returned). |

**Use Case 3**

| Item | Description |
|---|---|
| Use Case | Check Equipment Availability |
| Primary Actor | Student |
| Preconditions | The equipment record exists in the system. |
| Main Action | The student checks whether a piece of equipment is currently available. |
| Expected Result | The system reports the equipment's current availability status to the student. |
| Possible Failure | The specific equipment ID the student asked about does not exist. |

## C. Domain Concepts

**Student**
1. Must contain: student ID, name, whether they're currently allowed to
   borrow, and their maximum number of active borrowings.
2. Rules/state it owns: its own "allowed to borrow" flag, and its borrowing
   limit.
3. Not its responsibility: tracking how many things it currently has
   borrowed (that's derived from Borrowing records, not stored on Student
   itself), or deciding whether a specific borrow request should succeed
   (that requires equipment info too).

**Equipment**
1. Must contain: equipment ID, name, and current availability.
2. Rules/state it owns: its own availability — it protects itself from being
   "borrowed" twice in a row.
3. Not its responsibility: knowing who currently has it borrowed or when
   it's due back (that's Borrowing's job).

**Borrowing**
1. Must contain: which student, which equipment, date borrowed, expected
   return date, and current status (Active/Returned).
2. Rules/state it owns: its own status transition (Active to Returned) — it
   refuses to be "returned" twice.
3. Not its responsibility: deciding whether the borrowing should have been
   allowed in the first place — by the time a Borrowing record exists, all
   the eligibility checks already happened in the Application layer
   (BorrowEquipmentService).