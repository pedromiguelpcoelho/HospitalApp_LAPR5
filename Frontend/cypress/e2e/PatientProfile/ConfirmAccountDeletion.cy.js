describe('RequestAndConfirmAccountDeletionPage', () => {
    let deletionToken = '9d8bdbaf-882b-494b-8a87-88205c07f97a';

    beforeEach(() => {
        // Intercept the login API call and return a mock JWT token
        cy.intercept('POST', '/api/auth/login-username', {
            statusCode: 200,
            body: {
                "token": "mock-jwt-token",
                "role": "Patients",
                "name": "John Doe"
            }
        }).as('login');        

        // Intercept the API request for requesting account deletion
        cy.intercept('POST', '/api/PatientProfile/request-deletion', (req) => {
            req.reply({
                statusCode: 200,
                body: { message: 'A confirmation email has been sent to your email address.', token: deletionToken },
            });
        }).as('requestDeletion');

        // Intercept the API request for confirming account deletion (success)
        cy.intercept('POST', `/api/PatientProfile/confirm-deletion?token=${deletionToken}`, (req) => {
            req.reply({
                statusCode: 200,
                body: { message: 'Account deletion confirmed.' },
            });
        }).as('confirmDeletionSuccess');

        // Intercept the API request for confirming account deletion (failure)
        cy.intercept('POST', `/api/PatientProfile/confirm-deletion?token=${deletionToken}`, (req) => {
            req.reply({
                statusCode: 500,
                body: { message: 'Failed to confirm account deletion.' },
            });
        }).as('confirmDeletionFailure');

        // Visit the home page and log in
        cy.visit('http://localhost:3000');
        cy.get('[type="email"]').clear().type('john.doe@example.com');
        cy.get('[type="password"]').clear().type('StrongPassword123!');
        cy.get('form > button').click();
        cy.wait('@login');
    });

    it('requests and confirms the account deletion successfully', () => {
        // Request account deletion
        cy.get(':nth-child(1) > .patient-nav-item').click();
        cy.get('button[type="submit"]').click();
        cy.wait('@requestDeletion').its('response.statusCode').should('eq', 200);
        cy.contains('A confirmation email has been sent to your email address.', { timeout: 10000 }).should('be.visible');

        // Confirm account deletion
        cy.get(':nth-child(2) > .patient-nav-item').click();
        cy.get('input[type="text"]').type(deletionToken);
        cy.get('button[type="submit"]').click();
        cy.wait('@confirmDeletionSuccess', { timeout: 10000 }).its('response.statusCode').should('eq', 200);
        cy.contains('Account deletion confirmed.', { timeout: 10000 }).should('be.visible');
    });

    it('handles account deletion confirmation failure', () => {
        // Request account deletion
        cy.get(':nth-child(1) > .patient-nav-item').click();
        cy.get('button[type="submit"]').click();
        cy.wait('@requestDeletion').its('response.statusCode').should('eq', 200);
        cy.contains('A confirmation email has been sent to your email address.', { timeout: 10000 }).should('be.visible');

        // Confirm account deletion with failure
        cy.get(':nth-child(2) > .patient-nav-item').click();
        cy.get('input[type="text"]').type(deletionToken);
        cy.get('button[type="submit"]').click();
        cy.wait('@confirmDeletionFailure', { timeout: 10000 }).its('response.statusCode').should('eq', 500);
        cy.contains('Failed to confirm account deletion.', { timeout: 10000 }).should('be.visible');
    });
});