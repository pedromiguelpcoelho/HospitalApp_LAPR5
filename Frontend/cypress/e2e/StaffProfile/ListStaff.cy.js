    //-------------------------------------
    //--------LIST STAFF PROFILES BY SEARCH PARAMS----------
    //-------------------------------------

describe('ListStaffPage', () => {

    let staffMembers;

    beforeEach(() => {
        
        cy.intercept('GET', '/api/staff/search', (req) => {
            req.reply((res) => {
                staffMembers = res.body;
            });
        }).as('fetchStaffProfiles');
    });

    it('should list staff profiles based on search parameters successfully', () => {

       
        cy.visit('http://localhost:3000');
        
        cy.get('[type="email"]').clear().type('josecarlos.mendes.2003@gmail.com');
        cy.get('[type="password"]').clear().type('Jose@2024');
        cy.get('form > button').click();

        cy.get(':nth-child(3) > ul > :nth-child(4) > a').click();  
        
        cy.wait('@fetchStaffProfiles');
      
        cy.contains('Petr Yan').should('be.visible');
        cy.contains('Deiverson Figueiredo').should('be.visible');
        cy.contains('Almeida Cristalino').should('be.visible');
        
        cy.get('select').select('specialization');
        cy.get('[name="specialization"]').clear().type('Orthopaedist');        
        
        cy.contains('Petr Yan').should('be.visible');
        cy.contains('Deiverson Figueiredo').should('not.exist'); 
       
        cy.get('select').select('name');
        cy.get('[name="name"]').clear().type('Petr Yan');
                
        cy.contains('Petr Yan').should('be.visible');
        
    });

});

