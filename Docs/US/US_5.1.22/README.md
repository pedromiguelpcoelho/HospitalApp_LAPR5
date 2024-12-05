# US 5.1.22 - As an Admin, I want to remove obsolete or no longer performed operation types, so that the system stays current with hospital practices. (#22)

## 1. Context

### 1.1. Customer Specifications and Clarifications

* The system should allow administrators to remove outdated or no longer necessary operation types from future scheduling options, while maintaining historical records for data integrity.

## 1.2. Explanation

* To ensure that the hospital's operation records remain accurate and reflect current practices, the system must allow admins to mark operation types as inactive without fully deleting them, preserving historical data while preventing their use in future scheduling.

## 2. Requirements


#### Use Cases:

* Admins should be able to search for and select operation types to deactivate.
* The system must provide a confirmation prompt before marking an operation type as inactive.
* Operation types marked as inactive must remain in historical records but should no longer be available for new scheduling requests.

#### Functionality:

* Search and filter operation types by name, specialization, or active/inactive status.
* Display operation details such as name, required staff, and estimated duration.

#### Understanding:

* The ability to deactivate operation types ensures the system remains aligned with the hospital's up-to-date procedures while preserving the integrity of historical records for audits and analysis.

#### Dependencies:

1. Operation scheduling and historical records systems.
2. User permissions (only Admins can perform this action).

#### Acceptance Criteria:

- Admins can mark operation types as inactive.
- Inactive operation types are unavailable for future scheduling but remain in historical data.
- Confirmation prompts are displayed before deactivating an operation type.

#### Input and Output Data

* Input: selected OperationType
* Output: Confirmation message, updated status of the operation type (active/inactive).


## 3. Analysis

* The feature needs to interact with operation type records.

## 4. Design

### 4.1. Realization (Sequence Diagram)

#### Level 1

![SequenceDiagramLv1](./Sequence%20Diagram/Level%201/svg/SequenceDiagramLevel1.svg)

#### Level 2

![SequenceDiagramLv2](./Sequence%20Diagram/Level%202/svg/sd_lv2.svg)

#### Level 3

![SequenceDiagramLv3](./Sequence%20Diagram/Level%203/svg/sd_lv3.svg)


### 4.2. Class Diagram

![ClassDiagram](./Class%20Diagram/svg/class_diagram.svg)


### 4.3. Applied Patterns

- Repository Pattern: To interact with the database when adding a new operation type.
- MVC Pattern: To handle the user input and system response within the backoffice.

### 4.4. Tests

Tests should confirm:

- Deactivation flow works correctly.
- Deactivated operation types are removed from available operation types but visible in historical records.


## 5. Implementation

### Main classes created

* OperationType: Represents the medical procedure.
* OperationTypeRepository: Handles database interactions for storing operation types.
* OperationTypeController: Manages admin requests to deactivate operation types.
* OperationTypeService: Contains the business logic for deactivating operation types.

## 6. Integration/Demonstration

- Verify the functionality through admin access, searching and deactivating an operation type, and ensuring it remains in historical data but is excluded from future scheduling.

## 7. Observations

- Ensure proper logging and auditing features are in place to track changes to operation type statuses.