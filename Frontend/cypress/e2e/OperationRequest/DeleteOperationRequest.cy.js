describe('Delete Operation Request', () => {
    let operationRequests;
  
    beforeEach(() => {
      // Intercept the login API call and return a mock JWT token
      cy.intercept('POST', '/api/auth/login-username', {
        statusCode: 200,
        body: {
          "token": "eyJraWQiOiJ4U1k1XC8wd3NTUmhDY0dpcW9hamNrSEs0YkMzSWFRQUpkTTlmMFNURU90RT0iLCJhbGciOiJSUzI1NiJ9.eyJzdWIiOiJjNDI4MDQ0OC0xMDQxLTcwN2ItNjI3Yy1lZTlkMjMwNzJiMzciLCJjb2duaXRvOmdyb3VwcyI6WyJEb2N0b3JzIl0sImVtYWlsX3ZlcmlmaWVkIjp0cnVlLCJpc3MiOiJodHRwczpcL1wvY29nbml0by1pZHAudXMtZWFzdC0xLmFtYXpvbmF3cy5jb21cL3VzLWVhc3QtMV9ITU13YUs5SFMiLCJjb2duaXRvOnVzZXJuYW1lIjoiYzQyODA0NDgtMTA0MS03MDdiLTYyN2MtZWU5ZDIzMDcyYjM3Iiwib3JpZ2luX2p0aSI6ImUzOWNjYWY3LTExMTItNDIyMi05NzAzLTUyNzRhZWY3NjE3YyIsImF1ZCI6IjQwdGRoN2NpZjJucjJuampuN3IzdmFzazR2IiwiZXZlbnRfaWQiOiJjZDdhYmU0NC05ZGVlLTRmZmMtYjgyZi05NDk4M2VkNTU1ZjAiLCJ0b2tlbl91c2UiOiJpZCIsImF1dGhfdGltZSI6MTczMjMyNDMyMSwibmFtZSI6Ik1pZ3VlbCBMaW1hIiwiZXhwIjoxNzMyMzI3OTIxLCJpYXQiOjE3MzIzMjQzMjEsImp0aSI6Ijg1MWRhMTc5LTQ1YzQtNDYxZS04M2Y4LTgzZmExN2ExY2I4NCIsImVtYWlsIjoiMTIyMDY4OEBpc2VwLmlwcC5wdCJ9.BdA__ASdvdZNrsTiJOFu8GRhYSyZB_cjQvo4RzBmqaRv1J0HML2swiAv1o28NZ8L2AIFIiPRor7O7i-OzN0MDdtnCNBWuixs5xxZA97U1_aNcnksqRDJLB1KYAiccFyHpPNwk2R2DlrbC0BUr2Gy_tGY77GC3Qgy7izP568bYYp8u6qeVKUropqKmr2N0nDKcTz5WKI3YcsrWMJVVOjmZAoyJbk_0jfPnkr5b54ZFDW54kGzZg780ed_WR2CsskhEucbIUF9gcWcj-Yy0o4URTAfCezcP7xvadAJTHGw8jmUevGUdZCWwqpGg84cCUX4ruLi4NA6FzGI89wmMl413g",
          "role": "Doctors",
          "name": "Miguel Lima"
        }
      }).as('login');
  
      // Intercept the API request for retrieving patients
      cy.intercept('GET', '/api/PatientProfile/all', {
        statusCode: 200,
        body: [
          {
            id: 'a193dd8f-2dfb-4a4f-a3a8-86546dc8b8a2',
            firstName: 'Pedro',
            lastName: 'Coelho',
            fullName: 'Pedro Coelho',
            dateOfBirth: '2004-01-02',
            email: 'pedro02012004@example.com',
            contactInformation: '961309771'
          }
        ]
      }).as('getPatients');
  
      // Intercept the API request for retrieving operation types
      cy.intercept('GET', '/api/OperationTypes/all', {
        statusCode: 200,
        body: [
          {
            id: '19d483bb-905b-421e-afb9-8f81f2e9f329',
            name: 'Hip Replacement',
            requiredStaff: ['39158f4f-04a1-4942-a678-7e73ac5ce5f2'],
            estimatedDuration: 240
          }
        ]
      }).as('getOperationTypes');
  
      // Intercept the API request for retrieving staff
      cy.intercept('GET', '/api/Staff/all', {
        statusCode: 200,
        body: [
          {
            id: '39158f4f-04a1-4942-a678-7e73ac5ce5f2',
            firstName: 'Miguel',
            lastName: 'Lima',
            name: 'Miguel Lima',
            email: 'miguellima@example.com',
            phoneNumber: '987654321',
            role: 'Doctor',
            specialization: 'Orthopaedist'
          }
        ]
      }).as('getStaff');
  
      // Initialize operation requests
      operationRequests = [
        {
          id: '399af060-99d1-4e80-87d1-f2f0767f3f21',
          patientName: 'Pedro Coelho',
          operationTypeName: 'Hip Replacement',
          doctorName: 'Miguel Lima',
          priority: 'Urgent',
          suggestedDeadline: '2024-12-01T12:00:00Z',
          requestDate: '2024-11-30T10:00:00Z'
        },
        {
          id: '399af060-99d1-4e80-87d1-f2f0767f3f22',
          patientName: 'John Doe',
          operationTypeName: 'Knee Replacement',
          doctorName: 'Sofia Paulo',
          priority: 'Elective',
          suggestedDeadline: '2024-12-02T12:00:00Z',
          requestDate: '2024-11-30T10:00:00Z'
        },
        {
          id: '399af060-99d1-4e80-87d1-f2f0767f3f23',
          patientName: 'Jane Doe',
          operationTypeName: 'Hip Replacement',
          doctorName: 'Miguel Lima',
          priority: 'Emergency',
          suggestedDeadline: '2024-12-03T12:00:00Z',
          requestDate: '2024-11-30T10:00:00Z'
        },
        {
          id: '399af060-99d1-4e80-87d1-f2f0767f3f24',
          patientName: 'Joe Doe',
          operationTypeName: 'Knee Replacement',
          doctorName: 'Sofia Paulo',
          priority: 'Urgent',
          suggestedDeadline: '2024-12-04T12:00:00Z',
          requestDate: '2024-11-30T10:00:00Z'
        }
      ];
    });
  
    it('should delete an existing operation request', () => {
  
      cy.intercept('POST', '/api/OperationRequests/add', {
        statusCode: 201,
        body: {
          id: '399af060-99d1-4e80-87d1-f2f0767f3f21',
          patientId: 'a193dd8f-2dfb-4a4f-a3a8-86546dc8b8a2',
          operationTypeId: '19d483bb-905b-421e-afb9-8f81f2e9f329',
          doctorId: '39158f4f-04a1-4942-a678-7e73ac5ce5f2',
          priority: 'Urgent',
          suggestedDeadline: '2024-12-01T12:00:00Z',
          message: 'Operation request created successfully',
        },
      }).as('createOperationRequest');
  
      // Intercept the API call to fetch operation requests by doctor
      cy.intercept('GET', '/api/operationrequests/searchByDoctor', (req) => {
        const token = req.headers.authorization.split(' ')[1];
        const decodedToken = JSON.parse(atob(token.split('.')[1]));
        const doctorName = decodedToken.name;
  
        const filteredRequests = operationRequests.filter(request => request.doctorName === doctorName);
        req.reply({
          statusCode: 200,
          body: {
            operationRequests: filteredRequests
          }
        });
      }).as('getOperationRequestsByDoctor');
  
      // Intercept the API call to delete an operation request
      cy.intercept('DELETE', '/api/operationrequests/delete/399af060-99d1-4e80-87d1-f2f0767f3f21', (req) => {
        operationRequests = operationRequests.filter(request => request.id !== '399af060-99d1-4e80-87d1-f2f0767f3f21');
        req.reply({
          statusCode: 200,
          body: {
            message: 'Operation request deleted successfully!'
          }
        });
      }).as('deleteOperationRequest');
  
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
  
      // Navigate to the delete operation request page
      cy.get(':nth-child(2) > ul > :nth-child(3) > .doctor-nav-item').click();
      cy.wait('@login');
      cy.wait('@getOperationRequestsByDoctor');
  
      // Find the operation request and select it
      cy.get('.delete-request-form-select').select('399af060-99d1-4e80-87d1-f2f0767f3f21');
  
      // Perform the deletion
      cy.get('.delete-request-form-button').click();
      cy.wait('@deleteOperationRequest');
  
      // Verify the deletion
      cy.contains('Operation request deleted successfully!').should('be.visible');
  
      // Verify the operation request is no longer in the search results
      cy.get(':nth-child(2) > ul > :nth-child(3) > .doctor-nav-item').click();
      cy.wait('@getOperationRequestsByDoctor');
      cy.get('.delete-request-form-select').should('not.contain', '399af060-99d1-4e80-87d1-f2f0767f3f21');
    });
  });