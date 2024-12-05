# RGPD RULES

## Client Clarifications

**[9 de November de 2024]** 

* **Question:** When it comes to US6.6.2, are we supposed to functionally simulate dataleak detection on our projects or just document what would happen during a potential data breach?

  * **Answer:** You'll only have to explain during the pitch what would happen if a data breach was identified according to the US 6.6.2., that's all. 

You don't have to operationalize the solution, just explain it in the pitch.

### 1. **Patient**

- **Processed Data**: Name, date of birth, contact information (email and phone number), consultation and surgery history, medical conditions, emergency contacts.
- **Sensitive Data**:
  - Medical history (illnesses, conditions, treatments).
  - Information about allergies and medical conditions.
  - Emergency contact and contact information details.
- **Processing**:
  - Registration and profile updates.
  - Scheduling and managing appointments and operations.
  - Exercising the right to be forgotten (account deletion request).
- **RGPD Requirements**:
  - Explicit consent for processing personal data.
  - Right to access data.
  - Right to rectification and deletion of data (right to be forgotten).
  - Notification in case of changes to any sensitive data.

### 2. **Staff (Healthcare Professional)**

- **Processed Data**: Name, license number, specialization, availability, contact information (email and phone number).
- **Sensitive Data**:
  - License number (unique identifier for professionals).
  - Information on specializations and availability details.
  - Contact information details.
- **Processing**:
  - Registration and management of professional profiles.
  - Updating profile details and availability.
  - Participation in scheduling operations and consultations.
- **RGPD Requirements**:
  - Protection of personal data and role-based access control.
  - Logging of data handling activities (audit).
  - Notification in case of changes to any sensitive data.

### 3. **User (Generic User, such as Admins and Technicians)**

- **Processed Data**: Username, email, assigned role.
- **Sensitive Data**:
  - Emails and authentication information.
- **Processing**:
  - Management of user profiles and permission assignments.
  - Access to data related to patients and staff for backoffice operations.
- **RGPD Requirements**:
  - Strict access control to ensure data integrity and confidentiality.
  - Logging of access and modifications for auditing purposes.

