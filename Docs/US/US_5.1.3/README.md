# US 5.1.3 - As a Patient, I want to register for the healthcare application, so that I can create a user profile and book appointments online. (#3)

## 1. Context

### 1.1. Customer Specifications and Clarifications

* The system must allow patients to self-register through an external IAM (Identity and Access Management) system, creating a profile with which they can book appointments and view appointment history. During registration, a verification email should be sent to validate the email address and ensure the registration process is completed correctly.
  
### 1.2 Forum Discussion

Question:
> In user story 5.1.3, there is an acceptance criterion that caused me some doubts: "The system validates the email address by sending a verification email with a confirmation link."

> I understand the relevance of this acceptance criterion when not using an external IAM (Identity and Access Management) system. It ensures that users can't claim someone else's email address, as they would need to access the email to confirm their registration (for example, by clicking a unique link sent to the email).

> However, there is another acceptance criterion stating: "Patients can self-register using the external IAM system." In this case, with an external IAM, wouldn't it be possible to bypass the step of sending a confirmation link to validate the email?

> Would the following approach be considered correct?

> An unauthenticated user tries to log in/access a patient area/register.
The unauthenticated user submits their credentials through the external IAM login (proving ownership of the email in the process).
The system receives this user's information (email), and if there is no corresponding user in the system, it asks for registration details (such as name and phone number).
The user submits the registration details, completing the registration as a patient in the system.
Advantages of this approach:

> Improved user experience: It simplifies the registration process by reducing steps, making it quicker and more convenient for users.
Efficiency: By relying on the external IAM for email validation, you avoid duplicating validation mechanisms and streamline the process.
This approach ensures that the email belongs to the patient without the need to send a confirmation email. Do you think this is a good solution, even though it doesn't comply with one of the acceptance criteria?

> NOTE: Google's IAM will be used for this process, which is reliable and also provides the email of the user who logged in (I don't know if this approach will be possible with other IAM system).

Answer:
> imagine the following scenario,
a patient is admitted to the hospital and a clerk registers their patient record with email abc@abc.com. that's the patient personal email.
afterwards, that same patient wants to self-register in the system.
the system use external IAM provider xyz
the patient will create an account in the IAM provider, e.g., abc12@xy2z.com and will use that identity to self-register as patient abc@abc.com
the system needs to send a verification email to abc@abc.com
when the patient follows the link sent to their personal email, the system will be able to "connect" the two emails as identifying the same person, and provide access to the system

## 1.2. Explanation

* This user story addresses the need for patients to create an account independently, allowing access to the application to book medical appointments. A verification email will be sent as part of the registration process, and the patient will only be able to view and book appointments after completing this verification step.

## 2. Requirements


#### Use Cases:

* Patients can self-register to create a profile.

#### Functionality:

* Patients can provide their name, email, and phone number to create a profile in the healthcare platform.
* The system must send a verification email to validate the address, allowing patients to view and book appointments only after verification.

#### Understanding:

* Self-registration is essential for patient convenience and appointment management. It’s also important that personal information is validated and confirmed before allowing access to booking features.

#### Dependencies:

1. External IAM: Required for authentication, authorization and patient identity validation.

#### Acceptance Criteria:

- Patients should be able to register by providing their name, email, and phone number.
- The system sends a verification email to confirm the email address.
- Patients can only access appointment booking features after email verification.
#### Input and Output Data

* Input: Name, email, phone.
* Output: Confirmation of registration via email and creation of a patient profile.


## 3. Analysis

* This user story is essential for the patient experience, ensuring that the booking process is secure and that the user profile is properly set up before allowing access to any features.

## 4. Design

### 4.1. Realization (Sequence Diagram)

#### Level 1

![SequenceDiagramLv1](./Sequence%20Diagram/Level%201/svg/Level%201%20Sequence%20Diagram%20for%20US%205.1.svg)

#### Level 2

![SequenceDiagramLv2](./Sequence%20Diagram/Level%202/svg/Level%202%20Sequence%20Diagram%20for%20US%205.1.svg)

#### Level 3

![SequenceDiagramLv3](./Sequence%20Diagram/Level%203/svg/Level%203%20Sequence%20Diagram%20for%20US%205.1.svg)


### 4.2. Applied Patterns

- MVC Pattern: To organize the patient registration interactions and personal data management.
- Repository Pattern: For interacting with the database when storing patient profile information.

### 4.3. Tests

- Required fields (name, email, phone) are correctly filled in.
- Verification email is sent upon registration.
- Access to booking is only enabled after email confirmation.

## 5. Implementation

### Main classes created

* AuthController: Responsible for handling patient registration and email verification.
* CoginitoAuthService: Service class for handling IAM interactions.

## 6. Integration/Demonstration

- Demonstrate the patient registration process via back office and the profile activation after email confirmation, enabling access to appointment scheduling.

## 7. Observations

The system must detect any issues in email sending or IAM integration and have backup processes to notify the patient and ensure data security.