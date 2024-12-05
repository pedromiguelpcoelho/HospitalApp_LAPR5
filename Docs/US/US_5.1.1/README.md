# US 5.1.1 - Register new users in the Backoffice (Admin) #1

## 1. Context

### 1.1. Customer Specifications and Clarifications

[27-09-24]
* Question: What are the system's password requirements?
  * Answer: At least 10 characters long, at least a digit, a capital letter and a special character.

* Question: O user tem a contact information, email e phone, ambos são obrigatórios?
    * Answer: Sim.

* Question (OT): Can a user have both patient and healthcare staff profiles?
  * Answer: No, a user cannot have both profiles. Staff and patients have separate identifications. For example, a healthcare 
  worker will have their own identification, and if they visit a doctor as a patient, they will have a different identification.

* Question (OT): Do nurses have specializations like doctors?
  * Answer: Yes, nurses can have specializations, which are important for specific surgeries.

* Question (OT): How are duplicate patient profiles handled when registered by both the patient and admin?
  * Answer: An administrator must first create the patient's record. The patient can then create an online profile to access 
  the system. This will include two-factor authentication. The system will send an email to the registered email address with a 
  link to complete the registration.
---------
[28-09-24]
* Question(OT): Are healthcare staff IDs unique across roles?
  * Answer: Yes, staff IDs are unique and not role-specific (e.g., a doctor and nurse can share the same ID format).

* Question(OT): Can users hold multiple roles?
  * Answer: No, each user can have only one role.
---------
[5-10-24]
* Question: Can a doctor haver more than one Expertise?
  * Answer: No. Consider only one specialization per doctor

* Question: Can you clarify the username and email requirements?
  * Answer: The username is the "official" email address of the user. For backoffice users, this is the mechanographic number
    of the collaborator, e.g., D240003 or N190345, and the DNS domain of the system. For instance, Doctor Manuela Fernandes has
    email "D180023@myhospital.com". The system must allow for an easy configuration of the DNS domain (e.g., environment variable).
    For patients, the username is the email address provided in the patient record and used as identity in the external IAM. For
    instance patient Carlos Silva has provided his email csilva98@gmail.com the first time he entered the hospital. That email
    address will be his username when he self-registers in the system
-------
[6-10-24]
* Question: Chapter 3.2 says that "Backoffice users are registered by the admin in the IAM through an out-of-band process.", 
but US 5.1.1 says that "Backoffice users are registered by an Admin via an internal process, not via self-registration.".
Can you please clarify if backoffice users registration uses the IAM system? And if the IAM system is the out-of-band process?
  * Answer: What this means is that backoffice users can not self-register in the system like the patients do. The admin must 
  register the backoffice user. If you are using an external IAM (e.g., Google, Azzure, Linkedin, ...) the backoffice user 
  must first create their account in the IAM provider and then pass the credential info to the admin so that the user account 
  in the system is "linked" wit the external identity provider.
-------------

* Question:
  * Answer:

* Question:
  * Answer:

* Question:
  * Answer:

* Question:
  * Answer:


## 1.2. User Story Description

* As an Admin, I want to register new backoffice users (e.g., doctors, nurses,technicians, admins) via an out-of-band process, 
so that they can access the backoffice system with appropriate permissions.

## 2. Requirements

#### Acceptance Criteria:

- Backoffice users (e.g., doctors, nurses, technicians) are registered by an Admin via an internal process, not via self-registration.
- Admin assigns roles (e.g., Doctor, Nurse, Technician) during the registration process.
- Registered users receive a one-time setup link via email to set their password and activate their account.
- The system enforces strong password requirements for security.
- A confirmation email is sent to verify the user’s registration.

#### Use Cases:
![Use_Case_5.1.1-Use_Case_Diagram_5_1_1.svg](svg%2FUse_Case_5.1.1-Use_Case_Diagram_5_1_1.svg)
[Use_Case_5.1.1.puml](Use_Case_5.1.1.puml)

For this user story we can have 2 Use Cases:
* 1 - Register New User :
  * The Admin accesses the user registration section. 
  * The Admin enters the new user's details (name, email). 
  * The Admin selects the role (e.g., Staff, Doctor, Patient). 
  * The system validates the entered data. 
  * The Admin confirms the registration.

* 2 - Send Password Setup Link :
  * The system generates a unique password setup link. 
  * The system sends an email with the link to the new user.
  * The new user enters the provided link and sets a new password.
  * The system validates the password.
  * The system sends a confirmation email to the user.


#### Dependencies:

This user story has no dependencies on the other user stories in the project.

#### Input and Output Data

* Input Data:
  * Username 
  * Email
* Selection Data:
  * Role

* Output data:
  * (In)Success of the operation.
  * One-time setup link via email to set their password and activate their account.
  * Confirmation email to verify the user’s registration.


## 3. Analysis

*

## 4. Design

### 4.1. Realization (Sequence Diagram)

### 4.2. Class Diagram

### 4.3. Applied Patterns

### 4.4. Tests


## 5. Implementation

## 6. Integration/Demonstration

## 7. Observations