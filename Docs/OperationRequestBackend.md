# Operation Request - README.md

## 1. Associated User Stories

- **5.1.16.** - As a Doctor, I want to request an operation, so that the Patient has access to the
  necessary healthcare. **(#16)**

- **5.1.17.** - As a Doctor, I want to update an operation requisition, so that the Patient has
  access to the necessary healthcare. **(#17)**

- **5.1.18.** - As a Doctor, I want to remove an operation requisition, so that the healthcare
  activities are provided as necessary. **(#18)**

- **5.1.19.** - As a Doctor, I want to list/search operation requisitions, so that I see the details,
  edit, and remove operation requisitions. **(#19)**

------------------------------------------------

## 2. Customer Specifications and Clarifications

**[04-10-2024]**

- **Question:** 
  - A Operation Request tem o campo Priority. Que priorities existem?
- **Answer:** 
  - **Elective Surgery:** A planned procedure that is not life-threatening and can be scheduled at a convenient time (e.g., joint replacement, cataract surgery).
  - **Urgent Surgery:** Needs to be done sooner but is not an immediate emergency. Typically within days (e.g., certain types of cancer surgeries).
  - **Emergency Surgery:** Needs immediate intervention to save life, limb, or function. Typically performed within hours (e.g., ruptured aneurysm, trauma).


- **Question:** 
  - When does an Operation Request become an Appointment?
- **Answer:** 
  - When it is scheduled by the Planning/Scheduling Module.


- **Question:** 
  - What does “status” refer to in the context of searching for operating requisitions?
- **Answer:** 
  - Status refers to whether the operation is planned or requested.

------------------------------------------------

**[07-10-2024]**

- **Question:** 
  - When listing Operation Requests, should only the Operation Requests associated to the logged-in doctor be displayed?
- **Answer:** 
  - A doctor can see the Operation Requests they have submitted as well as the Operation Requests of a certain patient. 
  - An Admin will be able to list all Operation Requests and filter by doctor. 
  - It should be possible to filter by date of request, priority and expected due date.

------------------------------------------------

**[10-10-2024]**

- **Question:** 
  - Hello Mr. Client, you want to log all updates to the operation request. Do you plan to have this info available in the app or is this just for audit purposes ?
- **Answer:** 
  - The history of the operation type definition is part of the application's data. 
  - If the user needs to view the details of an operation that was performed last year, they need to be able to see the operation configuration that was in place at that time.

------------------------------------------------

**[12-10-2024]**

- **Question:** 
  - What is the process for handling the editing of operations, specifically regarding their type and history?
- **Answer:** 
  - When editing an operation, the system needs to maintain the history of the original operation type. 
  - The challenge is that operation names must be unique, and you may want to track versions of operations over time. 
  - One approach is to use an auxiliary table to store operation history, ensuring you can track changes and retrieve past data, much like how VAT changes in invoices are handled.

------------------------------------------------

**[21-10-2024]**

- **Question:** 
  - There was a previous question - "What information can physicians update on an operating requisition?", with the following answer, "Physicians can update the operating time, priority, and description text, but not change the patient.". 
  - However, half of this answer applies to the Operation Type, instead of the Operation Request. 
  - Operation Requests have, at least, an ID, a Patient, an Operation Type, a Doctor, a Deadline Date, and a Priority. 
  - Considering the previous answer, the doctor cannot change the Patient ID but can change the Priority. 
  - Besides the Priority, could the doctor also update the Deadline Date or Operation Type?
- **Answer:**
  - The answer was about operation requests, not operation types. I believe the term "operation time" in the original answer was the reason for this misunderstanding, as it means the expected deadline for the request, not the duration. 
  - Thus, the doctor can change the deadline, the priority, and the description. The doctor cannot change the operation type nor the patient.


- **Question:** 
  - Can a doctor make more than one operation request for the same patient? 
  - If so, is there any limit or rules to follow? 
  - For example, doctors can make another operation request for the same patient as long as it's not the same operation type?
- **Answer:**
  - It should not be possible to have more than one "open" surgery request (that is, a surgery that is requested or scheduled but not yet performed) for the same patient and operation type. 

------------------------------------------------

**[22-10-2024]**

- **Question:** 
  - Does the system adds automatically the operation request to the medical history of the patient?
- **Answer:**
  - No need. It will be the doctor's responsibility to add it.


- **Question:** 
  - Já referiu que os registros do Medical Record são adicionados manualmente. Apagar esses registos também o deverá ser? 
  - Mais especificamente na US 5.1.18 diz "Once deleted, the operation request is removed from the Patient’s Medical Record and cannot be recovered". 
  - Este DELETE do Medical Record deverá ser manual ou deverá acontecer ao mesmo tempo do delete do Operation Request?
- **Answer:**
  - Ver https://moodle.isep.ipp.pt/mod/forum/discuss.php?d=31956#p40557.
  - O sistema nao faz a ligação automatica entre medical history e operation request.

------------------------------------------------

## 3. Acceptance Criteria

### **_5.1.16._**

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

### **_5.1.17._**

- Doctors can update operation requests they created (e.g., change the deadline or priority).
- The system checks that only the requesting doctor can update the operation request.
- The system logs all updates to the operation request (e.g., changes to priority or deadline).
- Updated requests are reflected immediately in the system and notify the Planning Module of
  any changes.

------------------------------------------------

### **_5.1.18._**

- Doctors can delete operation requests they created if the operation has not yet been
  scheduled.
- A confirmation prompt is displayed before deletion.
- Once deleted, the operation request is removed from the patient’s medical record and cannot
  be recovered.
- The system notifies the Planning Module and updates any schedules that were relying on this
  request.

------------------------------------------------

### **_5.1.19._**

- Doctors can search operation requests by patient name, operation type, priority, and status.
- The system displays a list of operation requests in a searchable and filterable view.
- Each entry in the list includes operation request details (e.g., patient name, operation type,
  status).
- Doctors can select an operation request to view, update, or delete it.

------------------------------------------------

## 4. Input and Output Data

### **_5.1.16._**

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

### **_5.1.17._**

- **Input Data:**
  - Operation Request ID;
  - Updated Priority (if applicable);
  - Updated Suggested Deadline (if applicable);
  - Doctor ID (to verify the requesting Doctor);

- **Output Data:**
  - Confirmation of the (in)successful update of the Operation Request;
  - Log of the Update in the Operation Request;

------------------------------------------------

### **_5.1.18._**

- **Input Data:**
  - Doctor ID (to verify the requesting Doctor);
  - Operation Request ID;
  
- **Output Data:**
  - Confirmation of the (in)successful update of the Operation Request;
  - Removal of the Operation Request from the Patient's Medical History;

------------------------------------------------

### **_5.1.19._**

- **Input Data:**
  - Patient Name (First and/or Last, if applicable);
  - Operation Type (if applicable);
  - Priority (if applicable);
  - They can be separately or combined to filter the Operation Requests;

- **Output Data:**
  - List of Operation Requests that match the search criteria;
  - Operation Request Details (Patient ID, Doctor ID, Operation Type ID, etc.);

------------------------------------------------

## 5. Use Cases



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

### **_5.1.16._**
- Depends on the external IAM system for user authentication.
- Depends on the patient profile created in US 6.2.1.

### **_5.1.17._**
- Depends on the external IAM system for user authentication.
- Depends on the operation request created in US 6.2.14.

### **_5.1.18._**
- Depends on the external IAM system for user authentication.
- Depends on the operation request created in US 6.2.14.

### **_5.1.19._**
- Depends on the external IAM system for user authentication.
- Depends on the operation requests created in US 6.2.14, US 6.2.15, and US 6.2.16.