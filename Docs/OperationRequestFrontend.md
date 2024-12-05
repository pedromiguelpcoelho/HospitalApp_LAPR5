# Operation Request - Frontend README.md

## 1. Associated User Stories

- **6.2.14.** - As a Doctor, I want to request an operation, so that the Patient has access to the necessary healthcare. **(#44)**

- **6.2.15.** - As a Doctor, I want to update an operation requisition, so that the Patient has access to the necessary healthcare. **(#45)**

- **6.2.16.** - As a Doctor, I want to remove an operation requisition, so that the healthcare activities are provided as necessary. **(#46)**

- **6.2.17.** - As a Doctor, I want to list/search operation requisitions, so that I see the details, edit, and remove operation requisitions. **(#47)**

------------------------------------------------

## 2. Customer Specifications and Clarifications



------------------------------------------------

## 3. Acceptance Criteria

### **_6.2.14._**

- Doctors can create an operation request by selecting the patient, operation type, priority, and
  suggested deadline.
- The system validates that the operation type matches the doctor’s specialization.
- The operation request includes:
  - Patient ID
  - Doctor ID
  - Operation type
  - Deadline
  - Priority
- The system confirms successful submission of the operation request and logs the request in
  the patient’s medical history.

------------------------------------------------

### **_6.2.15._**

- Doctors can update operation requests they created (e.g., change the deadline or priority).
- The system checks that only the requesting doctor can update the operation request.
- The system logs all updates to the operation request (e.g., changes to priority or deadline).
- Updated requests are reflected immediately in the system and notify the Planning Module of
  any changes.

------------------------------------------------

### **_6.2.16._**

- Doctors can delete operation requests they created if the operation has not yet been
  scheduled.
- A confirmation prompt is displayed before deletion.
- Once deleted, the operation request is removed from the patient’s medical record and cannot
  be recovered.
- The system notifies the Planning Module and updates any schedules that were relying on this
  request.

------------------------------------------------

### **_6.2.17._**

- Doctors can search operation requests by patient name, operation type, priority, and status.
- The system displays a list of operation requests in a searchable and filterable view.
- Each entry in the list includes operation request details (e.g., patient name, operation type,
  status).
- Doctors can select an operation request to view, update, or delete it.

------------------------------------------------

## 4. Input and Output Data

### **_6.2.14._**

- **Input Data:**
  - Patient ID;
  - Doctor ID;
  - Operation Type ID;
  - Priority;
  - Suggested Deadline;
  
- **Output Data:**
  - Confirmation of the (in)successful submission of the Operation Request;
  - Operation Request Details logged in the Patient's Medical History;

------------------------------------------------

### **_6.2.15._**

- **Input Data:**
  - Operation Request ID;
  - Updated Priority (if applicable);
  - Updated Suggested Deadline (if applicable);
  - Doctor ID (to verify the requesting Doctor);

- **Output Data:**
  - Confirmation of the (in)successful update of the Operation Request;
  - Log of the Update in the Operation Request;

------------------------------------------------

### **_6.2.16._**

- **Input Data:**
  - Doctor ID (to verify the requesting Doctor);
  - Operation Request ID;
  
- **Output Data:**
  - Confirmation of the (in)successful update of the Operation Request;
  - Removal of the Operation Request from the Patient's Medical History;

------------------------------------------------

### **_6.2.17._**

- **Input Data:**
  - Patient Name (First and/or Last, if applicable);
  - Operation Type (if applicable);
  - Priority (if applicable);
  - They can be separately or combined to filter the Operation Requests;

- **Output Data:**
  - List of Operation Requests that match the search criteria;
  - Operation Request Details (Patient ID, Doctor ID, Operation Type ID, etc.);

------------------------------------------------

## 6. General Information

- **_Operation Request:_** The operation that is requested for later scheduling.

- **Attributes:**
  - `ID` - Unique Identifier;
  - `Patient ID` - Linked to a Specific Patient;
  - `Doctor ID` - Linked to a Doctor that requests it;
  - `Operation Type ID` - The type of the Operation to perform on the Patient;
  - `Deadline Date` - The Suggested Deadline to perform the Operation;
  - `Priority` - The Priority to perform the Operation;

------------------------------------------------

## 7. Dependency

### **_6.2.14_**
- Depends on the external IAM system for user authentication.
- Depends on the patient profile created in US 6.2.1.

### **_6.2.15_**
- Depends on the external IAM system for user authentication.
- Depends on the operation request created in US 6.2.14.

### **_6.2.16_**
- Depends on the external IAM system for user authentication.
- Depends on the operation request created in US 6.2.14.

### **_6.2.17_**
- Depends on the external IAM system for user authentication.
- Depends on the operation requests created in US 6.2.14, US 6.2.15, and US 6.2.16.