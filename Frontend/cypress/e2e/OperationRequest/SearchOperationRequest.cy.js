describe('Search Operation Request', () => {
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
        
        // Intercept the API call to fetch operation requests by doctor
        cy.intercept('GET', '/api/operationrequests/searchByDoctor', (req) => {
            const token = req.headers.authorization.split(' ')[1];
            const decodedToken = JSON.parse(atob(token.split('.')[1]));
            const doctorName = decodedToken.name;

            const operationRequests = [
                {
                    id: '399af060-99d1-4e80-87d1-f2f0767f3f21',
                    patientName: 'John Doe',
                    operationTypeName: 'Knee Replacement',
                    doctorName: 'Miguel Lima',
                    priority: 'Urgent',
                    suggestedDeadline: '2024-12-01T12:00:00Z',
                    requestDate: '2024-11-30T10:00:00Z'
                },
                {
                    id: '399af060-99d1-4e80-87d1-f2f0767f3f22',
                    patientName: 'Jane Doe',
                    operationTypeName: 'Hip Replacement',
                    doctorName: 'Sofia Paulo',
                    priority: 'Elective',
                    suggestedDeadline: '2024-12-02T12:00:00Z',
                    requestDate: '2024-11-30T10:00:00Z'
                },
                {
                    id: '399af060-99d1-4e80-87d1-f2f0767f3f23',
                    patientName: 'Joe Doe',
                    operationTypeName: 'Hip Replacement',
                    doctorName: 'Miguel Lima',
                    priority: 'Emergency',
                    suggestedDeadline: '2024-12-03T12:00:00Z',
                    requestDate: '2024-11-30T10:00:00Z'
                },
                {
                    id: '399af060-99d1-4e80-87d1-f2f0767f3f24',
                    patientName: 'Joanne Doe',
                    operationTypeName: 'Knee Replacement',
                    doctorName: 'Sofia Paulo',
                    priority: 'Urgent',
                    suggestedDeadline: '2024-12-04T12:00:00Z',
                    requestDate: '2024-11-30T10:00:00Z'
                }
            ];

            const filteredRequests = operationRequests.filter(request => request.doctorName === doctorName);
            req.reply({
                statusCode: 200,
                body: {
                    operationRequests: filteredRequests
                }
            });
        }).as('getOperationRequestsByDoctor');
    });

    it('should search and display operation requests', () => {
        // Visit the home page and log in
        cy.visit('http://localhost:3000');
        cy.get('[type="email"]').clear().type('1220688@isep.ipp.pt');
        cy.get('[type="password"]').clear().type('Pedro@2024');
        cy.get('form > button').click();

        // Visit the search operation request page
        cy.get(':nth-child(4) > .doctor-nav-item').click();

        // Wait for the login to complete
        cy.wait('@login');

        // Verify the search results
        cy.wait('@getOperationRequestsByDoctor');

        // Toggle details of the first operation request
        cy.get(':nth-child(1) > .search-toggle-details-button > strong').click();
    });
});