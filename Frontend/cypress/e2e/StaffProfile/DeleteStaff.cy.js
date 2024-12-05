describe('Delete Staff', () => {


    let staffMembers;

    beforeEach(() => {
        
        cy.intercept('GET', '/api/staff/all', (req) => {
            req.reply((res) => {
                staffMembers = res.body; 
            });
        }).as('fetchStaff');

        
        cy.intercept('DELETE', '/api/staff/d2373348-e76f-4758-a9a0-99360ed9d21f', (req) => {
            const staffId = req.url.split('/').pop();
            staffMembers = staffMembers.filter(staff => staff.id !== staffId); 
            req.reply({
                statusCode: 200,
                body: {
                    message: 'Staff successfully deactivated.'
                }
            });
        }).as('handleDeactivate');
    });


    it('should delete a staff member successfully', () => {
        
        
        cy.visit('http://localhost:3000');
        
        
        cy.get('[type="email"]').clear().type('josecarlos.mendes.2003@gmail.com');
        cy.get('[type="password"]').clear().type('Jose@2024');
        cy.get('form > button').click();

        
        cy.get(':nth-child(3) > ul > :nth-child(3) > a').click();

        
        cy.wait('@fetchStaff');

        
        
      cy.get('.delete-staff-form-select').select('d2373348-e76f-4758-a9a0-99360ed9d21f');
  
        
        cy.get('.delete-staff-form-button').click();    
        cy.wait('@handleDeactivate');

       
        cy.contains('Staff successfully deactivated.').should('be.visible');

       
    });
});
