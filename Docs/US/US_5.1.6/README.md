# US 5.1.6 - As a back-office user, I want my account to be temporarily locked after multiple failed login attempts to protect my account’s security.

## 1. Context

### 1.1. Customer Specifications and Clarifications

Purpose of Temporary Lock: Temporary account locking is a security measure to prevent unauthorized access after several failed login attempts. This functionality is crucial to safeguard sensitive data within the system, especially given access to patient information and surgical operations.

Unlocking Criteria:

Accounts are locked after five consecutive failed login attempts.
Unlocking can occur in two ways: through administrative intervention (Admin) or via a password reset process.
Sending an email confirmation link to unlock the account may be implemented to ensure that the person requesting the unlock is indeed the account owner.


Customer Specifications:

Primary Focus: To prevent unauthorized access after repeated login attempts, while minimizing the need for frequent administrative intervention.
Implementation Alternatives: The focus should be on a straightforward approach to authentication without complicating the process unnecessarily, as emphasized by the customer.


## 2. Requirements


#### Use Cases:

For this User Story, the following use cases apply:

1. Acess Features

The user enters their username and password on the login page.

The system checks if the credentials match an active account.

If the credentials are correct and the account is active, the system grants access to the user’s designated back-office features,if not after five consecutive incorrect attempts, the system temporarily locks the account and displays a message informing the user of the lock..

The user is redirected to the back-office dashboard where they can access features according to their role (e.g., view scheduled operations, manage patient records).

The system begins tracking the user session to maintain access continuity and security during use.

#### Functionality:

* The functionality involves securely managing login attempts to prevent unauthorized access through temporary account locking. Each failed login attempt increments an internal counter, and after five consecutive failed attempts, the account is automatically locked. The system then notifies the user of the lock through a displayed message and sends an email to the registered email address with information about the lock and instructions for unlocking. The unlock process can be handled manually by an administrator or through a password reset link.

#### Understanding:

* The temporary lock mechanism is intended to prevent unauthorized access by prompting users to verify their identity if they exceed the allowed number of failed login attempts. While the lock occurs automatically after five failed attempts, unlocking may either be managed by an administrator or initiated by the user through a password reset link, depending on the chosen system design. The approach should remain simple and focused on essential security features.

#### Dependencies:

* 5.1.1 As an Admin, I want to register new backoffice users (e.g., doctors, nurses,
technicians, admins) via an out-of-band process, so that they can access the
backoffice system with appropriate permissions.


#### Acceptance Criteria:

- Backoffice users log in using their username and password.

- Role-based access control ensures that users only have access to features appropriate to their
role (e.g., doctors can manage appointments, admins can manage users and settings).

- After five failed login attempts, the user account is temporarily locked, and a notification is
sent to the admin.

- Login sessions expire after a period of inactivity to ensure security.

#### Input and Output Data

* Input Data:
        * Username or email
        * Password (incorrect attempts are logged)

* Output Data:
        * Login error message (for each incorrect attempt)
        * Temporary lock notification after five failed attempts
        * Lock notification email with unlock instructions


