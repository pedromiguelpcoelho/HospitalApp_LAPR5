describe('UpdateMyOwnPatientProfilePage', () => {
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

        // Intercept the API request for updating the patient profile
        cy.intercept('PUT', '/api/PatientProfile/updatePatient', (req) => {
            req.reply({
                statusCode: 200,
                body: { message: 'Profile updated successfully' },
            });
        }).as('updateProfile');

        // Visit the home page and log in
        cy.visit('http://localhost:3000');
        cy.get('[type="email"]').clear().type('john.doe@example.com');
        cy.get('[type="password"]').clear().type('StrongPassword123!');
        cy.get('form > button').click();
        cy.wait('@login');

        // Navigate to the update profile page
        cy.get(':nth-child(3) > .patient-nav-item').click();
    });

    it('renders the form correctly', () => {
        cy.contains('Update My Profile').should('be.visible');
        cy.get('form').within(() => {
            cy.get('input[name="firstName"]').should('be.visible');
            cy.get('input[name="lastName"]').should('be.visible');
            cy.get('input[name="fullName"]').should('be.visible');
            cy.get('input[name="dateOfBirth"]').should('be.visible');
            cy.get('input[name="email"]').should('be.visible');
            cy.get('input[name="contactInformation"]').should('be.visible');
            cy.get('button[type="submit"]').should('contain', 'Update Profile');
        });
    });

    it('updates the profile successfully', () => {
        // Handle the alert
        cy.on('window:alert', (str) => {
            expect(str).to.equal('Profile updated successfully');
        });

        // Fill out the form with new profile data
        cy.get('input[name="firstName"]').clear().type('Jane');
        cy.get('input[name="lastName"]').clear().type('Doe');
        cy.get('input[name="fullName"]').clear().type('Jane Doe');
        cy.get('input[name="dateOfBirth"]').clear().type('1990-01-01');
        cy.get('input[name="email"]').clear().type('jane.doe@example.com');
        cy.get('input[name="contactInformation"]').clear().type('987654321');

        // Submit the form
        cy.get('button[type="submit"]').click();
        cy.wait('@updateProfile').its('response.statusCode').should('eq', 200);
    });

    it('handles profile update failure', () => {
        // Intercept the API request for updating the patient profile with failure
        cy.intercept('PUT', '/api/PatientProfile/updatePatient', (req) => {
            req.reply({
                statusCode: 500,
                body: { message: 'Failed to update profile' },
            });
        }).as('updateProfileFailure');

        // Handle the alert
        cy.on('window:alert', (str) => {
            expect(str).to.equal('Failed to update profile');
        });

        // Fill out the form with new profile data
        cy.get('input[name="firstName"]').clear().type('Jane');
        cy.get('input[name="lastName"]').clear().type('Doe');
        cy.get('input[name="fullName"]').clear().type('Jane Doe');
        cy.get('input[name="dateOfBirth"]').clear().type('1990-01-01');
        cy.get('input[name="email"]').clear().type('jane.doe@example.com');
        cy.get('input[name="contactInformation"]').clear().type('987654321');

        // Submit the form
        cy.get('button[type="submit"]').click();
        cy.wait('@updateProfileFailure').its('response.statusCode').should('eq', 500);
    });
});