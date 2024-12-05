describe('AddOperationRequestPage', () => {

    beforeEach(() => {
      // Intercept the API request for retrieving patients
      cy.intercept('GET', '/api/PatientProfile/all', {
        statusCode: 200,
        body: [
          {
            id: 'a193dd8f-2dfb-4a4f-a3a8-86546dc8b8a2',
            firstName: 'John',
            lastName: 'Doe',
            fullName: 'John Doe',
            dateOfBirth: '1990-01-01',
            email: 'john.doe@example.com',
            contactInformation: '923456789'
          }
        ]
      }).as('getPatients');
  
      // Intercept the API request for retrieving operation types
      cy.intercept('GET', '/api/OperationTypes/all', {
        statusCode: 200,
        body: [
          {
            id: '19d483bb-905b-421e-afb9-8f81f2e9f329',
            name: 'Knee Replacement',
            requiredStaff: ['39158f4f-04a1-4942-a678-7e73ac5ce5f2'],
            estimatedDuration: 120
          }
        ]
      }).as('getOperationTypes');
  
      // Intercept the API request for retrieving staff
      cy.intercept('GET', '/api/Staff/all', {
        statusCode: 200,
        body: [
          {
            id: '39158f4f-04a1-4942-a678-7e73ac5ce5f2',
            firstName: 'Dr.',
            lastName: 'Smith',
            name: 'Dr. Smith',
            email: 'dr.smith@example.com',
            phoneNumber: '987654321',
            role: 'Doctor',
            specialization: 'Orthopaedist'
          }
        ]
      }).as('getStaff');
    });
  
    // ---------------------------------------------------------- // 
  
    it('Successfully creates an Operation Request', () => {
      // Intercept the API request for creating an operation request
      cy.intercept('POST', '/api/OperationRequests/add', {
        statusCode: 201,
        body: {
          id: '399af060-99d1-4e80-87d1-f2f0767f3f21',
          patientId: 'a193dd8f-2dfb-4a4f-a3a8-86546dc8b8a2',
          operationTypeId: '19d483bb-905b-421e-afb9-8f81f2e9f329',
          doctorId: '39158f4f-04a1-4942-a678-7e73ac5ce5f2',
          priority: 'Urgent',
          suggestedDeadline: '2024-12-31T12:00:00Z',
          message: 'Operation request created successfully',
        },
      }).as('createOperationRequest');
  
      // Visit the home page and log in
      cy.visit('http://localhost:3000');
      cy.get('[type="email"]').clear().type('1220688@isep.ipp.pt');
      cy.get('[type="password"]').clear().type('Pedro@2024');
      cy.get('form > button').click();
  
      // Create an operation request
      cy.get(':nth-child(2) > ul > :nth-child(1) > .doctor-nav-item').click();
  
      // Wait for the intercepts to complete
      cy.wait('@getPatients');
      cy.wait('@getOperationTypes');
      cy.wait('@getStaff');
  
      cy.get('#add-request-form > :nth-child(2)').select('a193dd8f-2dfb-4a4f-a3a8-86546dc8b8a2');
      cy.get('#add-request-form > :nth-child(3)').select('19d483bb-905b-421e-afb9-8f81f2e9f329');
      cy.get('#add-request-form > :nth-child(4)').select('39158f4f-04a1-4942-a678-7e73ac5ce5f2');
      cy.get('#add-request-form > :nth-child(5)').select('Urgent');
      cy.get('input').click();
      cy.get('input').clear().type('2024-12-31T12:00');
      cy.get('#add-request-form > button').click();
      cy.wait('@createOperationRequest');
  
      // Verify the success message
      cy.contains('Operation request created successfully!').should('be.visible');
    });
  
    // ---------------------------------------------------------- // 
  
    it('Fails when creating an Operation Request because the Patient already has an Operation Request for the same Operation Types', () => {
      // Intercept the API request for creating an operation request
      cy.intercept('POST', '/api/OperationRequests/add', (req) =>{
        if (req.body.patientId === 'a193dd8f-2dfb-4a4f-a3a8-86546dc8b8a2' && req.body.operationTypeId === '19d483bb-905b-421e-afb9-8f81f2e9f329') {
          return {
            statusCode: 400,
            body: {
              message: 'An open request for the same patient and operation type already exists.',
            },
          };
        } else {
          return {
            statusCode: 201,
            body: {
              id: '399af060-99d1-4e80-87d1-f2f0767f3f21',
              patientId: 'a193dd8f-4a4f-a3a8-86546dc8b8a2',
              operationTypeId: '19d483bb-905b-421e-afb9-8f81f2e9f329',
              doctorId: '39158f4f-04a1-4942-a678-7e73ac5ce5f2',
              priority: 'Urgent',
              suggestedDeadline: '2024-12-31T12:00:00Z',
              message: 'Operation request created successfully',
            },
          };
        }
      }).as('createOperationRequest');
  
      // Visit the home page and log in
      cy.visit('http://localhost:3000');
      cy.get('[type="email"]').clear().type('1220688@isep.ipp.pt');
      cy.get('[type="password"]').clear().type('Pedro@2024');
      cy.get('form > button').click();
  
      // Create an operation request
      cy.get(':nth-child(2) > ul > :nth-child(1) > .doctor-nav-item').click();
  
      // Wait for the intercepts to complete
      cy.wait('@getPatients');
      cy.wait('@getOperationTypes');
      cy.wait('@getStaff');
  
      cy.get('#add-request-form > :nth-child(2)').select('a193dd8f-2dfb-4a4f-a3a8-86546dc8b8a2');
      cy.get('#add-request-form > :nth-child(3)').select('19d483bb-905b-421e-afb9-8f81f2e9f329');
      cy.get('#add-request-form > :nth-child(4)').select('39158f4f-04a1-4942-a678-7e73ac5ce5f2');
      cy.get('#add-request-form > :nth-child(5)').select('Urgent');
      cy.get('input').click();
      cy.get('input').clear().type('2024-12-31T12:00');
      cy.get('#add-request-form > button').click();
      cy.wait('@createOperationRequest');
  
      // Verify the error message
      cy.contains('An open request for the same patient and operation type already exists.').should('be.visible');
  
    });
  
    // ---------------------------------------------------------- // 
  
    it('Fails when creating an Operation Request with a past suggested deadline', () => {
      // Intercept the API request for creating an operation request
      cy.intercept('POST', '/api/OperationRequests/add', {
        statusCode: 400,
        body: {
          message: 'Suggested deadline must be in the future.',
        },
      }).as('createOperationRequest');
  
      // Visit the home page and log in
      cy.visit('http://localhost:3000');
      cy.get('[type="email"]').clear().type('1220688@isep.ipp.pt');
      cy.get('[type="password"]').clear().type('Pedro@2024');
      cy.get('form > button').click();
  
      // Create an operation request with a past deadline
      cy.get(':nth-child(2) > ul > :nth-child(1) > .doctor-nav-item').click();
  
      // Wait for the intercepts to complete
      cy.wait('@getPatients');
      cy.wait('@getOperationTypes');
      cy.wait('@getStaff');
  
      cy.get('#add-request-form > :nth-child(2)').select('a193dd8f-2dfb-4a4f-a3a8-86546dc8b8a2');
      cy.get('#add-request-form > :nth-child(3)').select('19d483bb-905b-421e-afb9-8f81f2e9f329');
      cy.get('#add-request-form > :nth-child(4)').select('39158f4f-04a1-4942-a678-7e73ac5ce5f2');
      cy.get('#add-request-form > :nth-child(5)').select('Urgent');
      cy.get('input').click();
      cy.get('input').clear().type('2020-12-31T12:00');
      cy.get('input').type('2020-12-31T12:00');
      cy.get('#add-request-form > button').click();
      cy.wait('@createOperationRequest');
  
      // Verify the error message
      cy.contains('Suggested deadline must be in the future.').should('be.visible');
    });
  
  });