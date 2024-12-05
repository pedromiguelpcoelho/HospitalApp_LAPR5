describe('RequestAccountDeletionPage', () => {

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
                body: { message: 'A confirmation email has been sent to your email address.' },
            });
        }).as('requestDeletion');

        // Visit the home page and log in
        cy.visit('http://localhost:3000');
        cy.get('[type="email"]').clear().type('john.doe@example.com');
        cy.get('[type="password"]').clear().type('StrongPassword123!');
        cy.get('form > button').click();
        cy.wait('@login');

        // Navigate to the request account deletion page
        cy.get(':nth-child(1) > .patient-nav-item').click();
    });

    it('renders the form correctly', () => {
        cy.contains('Request Account Deletion').should('be.visible');
        cy.get('button[type="submit"]').should('contain', 'Request Deletion');
    });

    it('submits the form successfully', () => {
        cy.get('button[type="submit"]').click();
        cy.wait('@requestDeletion').its('response.statusCode').should('eq', 200);
        cy.contains('A confirmation email has been sent to your email address.', { timeout: 10000 }).should('be.visible');
    });

    it('handles form submission failure', () => {
        cy.intercept('POST', '/api/PatientProfile/request-deletion', {
            statusCode: 500,
            body: { message: 'Request failed' },
        }).as('requestDeletionFailure');

        cy.get('button[type="submit"]').click();
        cy.wait('@requestDeletionFailure').its('response.statusCode').should('eq', 500);
        cy.contains('Failed to request account deletion.', { timeout: 10000 }).should('be.visible');
    });
});