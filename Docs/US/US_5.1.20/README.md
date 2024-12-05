# US 5.1.20 - As an Admin, I want to add new types of operations, so that I can reflect the available medical procedures in the system. (#20)

## 1. Context

### 1.1. Customer Specifications and Clarifications

* The system must allow Admin users to define new operation types that include the name, required medical staff, and estimated duration. This will ensure that new procedures can be added as needed to reflect the available medical procedures in the hospital.

## 1.2. Explanation

* This user story addresses the need for flexibility in managing medical procedures. Admins need to be able to add new operation types as new procedures become available or existing ones change. The newly added operations should become available for scheduling immediately after creation.

## 2. Requirements


#### Use Cases:

* Admin can add new operation types.

#### Functionality:

* Admin can specify the operation name, required staff by specialization, and estimated duration for each new operation type.

#### Understanding:

* Admins need the ability to add, edit, and remove operation types. This helps ensure that the system reflects all available medical procedures and stays current with the hospital's needs.


#### Dependencies:

1. The backoffice module must handle user authentication and roles (Admins vs. other users).
2. Operation types must be integrated with the scheduling system to allow these types to be used in new requests.

#### Acceptance Criteria:

- The operation name must be unique.
- Admins must be able to specify required staff and estimated duration for the operation.
- New operation types should be available for scheduling immediately after creation.
- The system logs the creation of new operation types.
  
#### Input and Output Data

* Input: Operation name, required staff, estimated duration.
* Output: The newly created operation type appears in the system and can be used for future scheduling.


## 3. Analysis

* This user story focuses on creating flexible management of operations within the healthcare system, ensuring that the Admin can manage the types of procedures available.

## 4. Design

### 4.1. Realization (Sequence Diagram)

#### Level 1

![SequenceDiagramLv1](./Sequence%20Diagram/Level%201/svg/Level%201%20Sequence%20Diagram%20for%20US%205.1.svg)

#### Level 2

![SequenceDiagramLv2](./Sequence%20Diagram/Level%202/svg/Level%202%20Sequence%20Diagram%20for%20US%205.1.svg)

#### Level 3

![SequenceDiagramLv3](./Sequence%20Diagram/Level%203/svg/Level%203%20Sequence%20Diagram%20for%20US%205.1.svg)


### 4.2. Class Diagram

![Class Diagram](./Class%20Diagram/svg/class_diagram.svg)

### 4.3. Applied Patterns

- Repository Pattern: To interact with the database when adding a new operation type.
- MVC Pattern: To handle the user input and system response within the backoffice.

### 4.4. Tests

* Tests should verify:

- Operation names are unique.
- Required fields (name, staff, duration) are filled and non null.
- New operation types are correctly added to the system and available for use in future schedules.

## 5. Implementation

### Main classes created

* OperationType: Represents the medical procedure being added.
* OperationTypeRepository: Handles database interactions for storing operation types.

## 6. Integration/Demonstration

* Demonstrate how an Admin user adds a new operation type via the backoffice, and how it becomes immediately available for scheduling new operations.

## 7. Observations

* Ensure that the system is capable of handling any future updates or removals of operation types, and that historical records of procedures are maintained even if certain types are removed from active use.