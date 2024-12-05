describe('Search Operation Request for Admins', () => {
    beforeEach(() => {
        // Intercept the login API call and return a mock JWT token for an Admin user
        cy.intercept('POST', '/api/auth/login-username', {
            statusCode: 200,
            body: {
                "token": "eyJraWQiOiJ4U1k1XC8wd3NTUmhDY0dpcW9hamNrSEs0YkMzSWFRQUpkTTlmMFNURU90RT0iLCJhbGciOiJSUzI1NiJ9.eyJzdWIiOiI3NGE4MzQyOC1lMDgxLTcwNGMtYzEyYS1lNjgxYTdmZTNkZjUiLCJjb2duaXRvOmdyb3VwcyI6WyJBZG1pbnMiXSwiZW1haWxfdmVyaWZpZWQiOnRydWUsImlzcyI6Imh0dHBzOlwvXC9jb2duaXRvLWlkcC51cy1lYXN0LTEuYW1hem9uYXdzLmNvbVwvdXMtZWFzdC0xX0hNTXdhSzlIUyIsImNvZ25pdG86dXNlcm5hbWUiOiI3NGE4MzQyOC1lMDgxLTcwNGMtYzEyYS1lNjgxYTdmZTNkZjUiLCJvcmlnaW5fanRpIjoiN2ZkODMzNmQtNzVmYi00ZDg2LTk0M2EtYmEyNzE0ZDIxMTliIiwiYXVkIjoiNDB0ZGg3Y2lmMm5yMm5qam43cjN2YXNrNHYiLCJldmVudF9pZCI6IjhmMzllYTU2LWE3ZTEtNGZlNy05N2ZlLTM0NzEwZGE4OWQ0OSIsInRva2VuX3VzZSI6ImlkIiwiYXV0aF90aW1lIjoxNzMyMzc5NTkxLCJuYW1lIjoiQWRtaW4iLCJleHAiOjE3MzIzODMxOTEsImlhdCI6MTczMjM3OTU5MSwianRpIjoiZGE1YWUwZGYtNDk5Ny00YTMxLTk3NGMtM2EwNDk2MTE0ZGI3IiwiZW1haWwiOiJqb3NlY2FybG9zLm1lbmRlcy4yMDAzQGdtYWlsLmNvbSJ9.EM8VExDBwFfbWzzGfL7Jr1tnfZI859oep9L6Z0Qyzqo6gU-JwbfchtWi87owN2oCWlgQb5eyqE8XNqT0V2VE_blk8Ls5-B-R7wGHp6QJQ2uGQpdk7dheBTmlhBV5QPIwtvnAOIUDEbuFISN8ROMUY3GxRTUCO9rTJ_y75N9l0KsDcmLgLCdMpvQAWD-Lx_oDNKa70cmFBY72qD451rcaPWFYRAC29XeRluK09RaYfYpJuPE2jTEZ4ysf4FcYv3gyEMUt60EUCqNcUcdhh17dG4JEghcDZgo5aXfz8vlJtkdC3K9u71QSBWDSBMfqqvUMDUy5PJAoOufEoPozT0o9CQ",
                "role": "Admins",
                "name": "Admin"
            }
        }).as('login');

        // Intercept the API call to fetch all operation requests
        cy.intercept('GET', '/api/operationrequests/all', {
            statusCode: 200,
            body: {
                operationRequests: [
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
                ]
            }
        }).as('getAllOperationRequests');
    });

    it('should search and display all operation requests for Admins', () => {
        // Visit the home page and log in as an Admin
        cy.visit('http://localhost:3000');
        cy.get('[type="email"]').clear().type('josecarlos.mendes.2003@gmail.com');
        cy.get('[type="password"]').clear().type('Jose@2024');
        cy.get('form > button').click();

        // Visit the search operation request page for Admins
        cy.get(':nth-child(4) > ul > :nth-child(1) > a').click();

        // Wait for the login to complete
        cy.wait('@login');

        // Verify the search results
        cy.wait('@getAllOperationRequests');

        // Toggle details of the first operation request
        cy.get(':nth-child(1) > .search-toggle-details-button > strong').click();
    });
});