# PatientFrontnd Readme

## 1. Associated user stories

* **US 6.2.1** - As a Patient, I want to register for the healthcare application, so that I can create a user profile and book appointments online. (#31)

* **US 6.2.2** - As a Patient, I want to update my user profile, so that I can change my personal details and preferences. (#32)

* **US 6.2.3** - As a Patient, I want to delete my account and all associated data, so that I can exercise my right to be forgotten as per GDPR. (#33)

* **US 6.2.5** - As a Patient, I want to log in to the healthcare system, so that I can access my appointments, medical records, and other features securely. (#35)

* **US 6.2.6** - As an Admin, I want to create a new patient profile, so that I can register their personal details and medical history. (#36)

* **US 6.2.7** - As an Admin, I want to edit an existing patient profile, so that I can update their information when needed. (#37)

* **US 6.2.8** - As an Admin, I want to delete a patient profile, so that I can remove patients who are no longer under care. (#38)

* **US 6.2.9** - As an Admin, I want to list/search patient profiles by different attributes, so that I can view the details, edit, and remove patient profiles. (#39)

------------------------------------------------

## 2. Customer specifications and clarifications

**[Date]**

* **Question:** 
  * **Answer:** 

------------------------------------------------

## 3. Acceptance Criteria 

### **_6.2.1_**

- Patients can self-register using the external IAM system.
- During registration, patients provide personal details (e.g., name, email, phone) and create a profile.
- The system validates the email address by sending a verification email with a confirmation link.
- Patients cannot list their appointments without completing the registration process.

------------------------------------------------

### **_6.2.2_**

- Patients can log in and update their profile details (e.g., name, contact information, preferences).
- Changes to sensitive data, such as email, trigger an additional verification step (e.g., confirmation email).
- All profile updates are securely stored in the system.
- The system logs all changes made to the patient's profile for audit purposes.

------------------------------------------------

### **_6.2.3_**

- Patients can request to delete their account through the profile settings.
- The system sends a confirmation email to the patient before proceeding with account deletion.
- Upon confirmation, all personal data is permanently deleted from the system within the legally required time frame (e.g., 30 days).
- Patients are notified once the deletion is complete, and the system logs the action for GDPR compliance.
- Some anonymized data may be retained for legal or research purposes, but all identifiable information is erased.

------------------------------------------------

### **_6.2.5_**

- Patients log in via an external Identity and Access Management (IAM) provider (e.g., Google, Facebook, or hospital SSO).
- After successful authentication via the IAM, patients are redirected to the healthcare system with a valid session.
- Patients have access to their appointment history, medical records, and other features relevant to their profile.
- Sessions expire after a defined period of inactivity, requiring reauthentication.

------------------------------------------------

### **_6.2.6_**

- Admins can input patient details such as first name, last name, date of birth, contact information.
- A unique patient ID (Medical Record Number) is generated upon profile creation.
- The system validates that the patient’s email and phone number are unique.
- The profile is stored securely in the system, and access is governed by role-based permissions.

------------------------------------------------

### **_6.2.7_**

- Admins can search for and select a patient profile to edit.
- Editable fields include name, contact information, medical history, and allergies.
- Changes to sensitive data (e.g., contact information) trigger an email notification to the patient.
- The system logs all profile changes for auditing purposes.

------------------------------------------------

### **_6.2.8_**

- Admins can search for a patient profile and mark it for deletion.
- Before deletion, the system prompts the admin to confirm the action.
- Once deleted, all patient data is permanently removed from the system within a predefined time frame.
- The system logs the deletion for audit and GDPR compliance purposes.

------------------------------------------------

### **_6.2.9_**

- Admins can search patient profiles by various attributes, including name, email, date of birth, or medical record number.
- The system displays search results in a list view with key patient information (name, email, date of birth).
- Admins can select a profile from the list to view, edit, or delete the patient record.
- The search results are paginated, and filters are available to refine the search results.

------------------------------------------------

## 4. Input and Output Data

### **_6.2.1_**

* Input Data:
* Output Data:

------------------------------------------------

### **_6.2.2_**

* Input Data:
  * Updated patient information, including:
    * First, Last and Full Name
    * Date of Birth
    * Email
    * Contact information
* Output Data:
  * The updated patient profile
  * (In)Success of the operation

------------------------------------------------

### **_6.2.3_**

* Input Data:
  * Email: The Email of the Patient that requested the deletion.
  * Token: The Token sent to the Patient's Email.
* Output Data:
  * (In)Success of the operation.

------------------------------------------------

### **_6.2.5_**

* Input Data:
* Output Data:

------------------------------------------------

### **_6.2.6_**

* Input Data:
    * First Name 
    * Last Name
    * Email
    * Date of Birth
    * Gender
    * Contact Information

* Output data:
    * The patient profile including the Medical Record Number generated.
    * (In)Success of the operation.

------------------------------------------------

### **_6.2.7_**

* Input Data:
  * Patient's unique identifier (e.g., name or ID) for search. 
  * Updated patient information, including:
    * First, Last and Full Name 
    * Date of Birth
    * Email
    * Contact information
* Output Data:
  * The updated patient profile including the Medical Record Number generated.
  * (In)Success of the operation.

------------------------------------------------

### **_6.2.8_**

* Input Data:
  * Patient's unique identifier (e.g., name or ID).
* Output data:
  * (In)Success of the operation.

------------------------------------------------

### **_6.2.9_**

* Input Data:
  * The name, email, date of birth, medical record number or none depending on the search criteria selected.
* Output data:
  * (In)Success of the operation.
  * List of Patients.

------------------------------------------------

## 5. Use Cases


------------------------------------------------

## 6. General Information
**_Patient:_** Represents individuals receiving medical care.

* Attributes:
  * `First Name`
  * `Last Name`
  * `Full Name`
  * `Date of Birth`
  * `Gender`
  * `Medical Record Number` (unique identifier)
  * `Contact Information` (email, phone)
  * `Allergies/Medical Conditions` (optional)
  * `Emergency Contact`
  * `Appointment History` (list of previous and upcoming appointments)

* Rules:
  * A patient must be unique in terms of `Medical Record Number`, `Email` and `Phone`. 
  * Sensitive data (like medical history) must comply with GDPR, allowing patients to control their data access.

------------------------------------------------

## 7. Dependency

### **_6.2.1_**
- Depends on the external IAM system for user authentication.

### **_6.2.2_**
- Depends on the patient profile created in US 6.2.1.
- Depends on the email verification system.

### **_6.2.3_**
- Depends on the patient profile created in US 6.2.1.
- Depends on the email notification system.

### **_6.2.5_**
- Depends on the external IAM system for user authentication.
- Depends on the patient profile created in US 6.2.1.

### **_6.2.6_**
- Depends on the admin role and permissions.

### **_6.2.7_**
- Depends on the patient profile created in US 6.2.6.
- Depends on the admin role and permissions.

### **_6.2.8_**
- Depends on the patient profile created in US 6.2.6.
- Depends on the admin role and permissions.

### **_6.2.9_**
- Depends on the patient profiles created in US 6.2.6.
- Depends on the admin role and permissions.