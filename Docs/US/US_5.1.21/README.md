# US 5.1. - As an Admin, I want to edit existing operation types, so that I can update or correct information about the procedure. (#21)

## 1. Context

### 1.1. Customer Specifications and Clarifications

* As an Admin, you should be able to update existing operation types to ensure the system stays up to date with the most accurate procedure details.

## 1.2. Explanation

* This user story focuses on providing admins the ability to correct or update information regarding operation types, such as names, required staff specializations, and estimated duration.

## 2. Requirements


#### Use Cases:

* An Admin searches for and selects an existing operation type.
* The Admin edits the operation name, required staff, and/or estimated duration.
* The updated operation type is reflected in future scheduling requests, but historical records remain unchanged.

#### Functionality:

* Provide an interface for admins to view and search for existing operation types.
* Allow editable fields such as operation name, required staff by specialization, and estimated duration.
* Reflect changes in the system immediately for future operation requests while maintaining historical records.


#### Understanding:

* This feature requires interaction between the backoffice module and scheduling system.
* Updates should be carefully handled to avoid impacting historical data and should only affect new scheduling operations.

#### Dependencies:

1. Operation types must already exist in the system.
2. Interaction with the scheduling module to ensure the updates are reflected in future requests.


#### Acceptance Criteria:

- Admins can search for and select an existing operation type.
- Editable fields include operation name, required staff, and estimated duration.
- Changes are immediately reflected for future operation requests, but historical data remains intact.
- 
#### Input and Output Data

* Input: Operation type attributes (name, required staff, estimated duration).
* Output: Updated operation type reflected in the system for future scheduling.


## 3. Analysis

* The system needs to maintain a history of operation type changes to ensure that older requests retain their original values, while new requests reflect the updated details.

## 4. Design

### 4.1. Realization (Sequence Diagram)

#### Level 1

![SequenceDiagramLv1](./Sequence%20Diagram/Level%201/svg/Level%201%20Sequence%20Diagram%20for%20US%205.1.svg)

#### Level 2

![SequenceDiagramLv2](./Sequence%20Diagram/Level%202/svg/Level%202%20Sequence%20Diagram%20for%20US%205.1.svg)

#### Level 3

![SequenceDiagramLv3](./Sequence%20Diagram/Level%203/svg/Level%203%20Sequence%20Diagram%20for%20US%205.1.svg)


### 4.2. Class Diagram

![ClassDiagram](./Class%20Diagram/svg/class_diagram.svg)

### 4.3. Applied Patterns

* MVC Pattern: To manage the user interface (search and update pages), business logic, and database interaction.
* Observer Pattern: Notify the scheduling system of updates to operation types.

### 4.4. Tests

- Test case to verify that operation types can be updated correctly.
- Test that historical data remains unchanged after updates.
- Test system behavior when incorrect or invalid data is entered during an update.

## 5. Implementation

### Main classes created

- OperationTypeService: Service responsible for handling the search, update, and validation of operation types.
- OperationTypeController: Handles admin requests to view and update operation types.

## 6. Integration/Demonstration

* MVC Pattern: To manage the user interface (search and update pages), business logic, and database interaction.
* Observer Pattern: Notify the scheduling system of updates to operation types.

## 7. Observations

* Be mindful of GDPR compliance, especially when dealing with changes to sensitive operation type information.
