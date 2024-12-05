//-------------------------------------
//--------UPDATE STAFF PROFILE E2E TESTING----------
//-------------------------------------

describe('UpdateStaffPage', () => {
    let staffMembers;
  
    beforeEach(() => {
      // Interceptando a chamada GET para obter os perfis de staff
      cy.intercept('GET', '/api/staff/search', (req) => {
        req.reply((res) => {
          staffMembers = res.body;
        });
      }).as('fetchStaffProfiles');
  
      // Interceptando a chamada PUT para atualizar o perfil de staff
      cy.intercept('PUT', '/api/staff/*', (req) => {
        req.reply({
          statusCode: 200,
          body: req.body, // Simulando a resposta com os dados do corpo enviado
        });
      }).as('updateStaffProfile');
    });
  
    it('should update a staff profile successfully', () => {
      
      cy.visit('http://localhost:3000');
  
      
      cy.get('[type="email"]').clear().type('josecarlos.mendes.2003@gmail.com');
      cy.get('[type="password"]').clear().type('Jose@2024');
      cy.get('form > button').click();
  
      
      cy.get(':nth-child(3) > ul > :nth-child(2) > a').click();
  
     
      cy.wait('@fetchStaffProfiles');
  
      
      cy.contains('Petr Yan').should('be.visible');
      cy.contains('Deiverson Figueiredo').should('be.visible');
      cy.contains('Almeida Cristalino').should('be.visible');
  
      
      cy.contains('Petr Yan').click();
  
      
      cy.contains('Staff Details').should('be.visible');
      cy.contains('Name: Petr Yan').should('be.visible');
      cy.contains('Specialization: Orthopaedist').should('be.visible');
  
      
      cy.get('button').contains('Update Profile').click();
  
      
      cy.get('[name="firstName"]').clear().type('Petr');
      cy.get('[name="lastName"]').clear().type('Yan Updated');
      cy.get('[name="email"]').clear().type('petr.yan.updated@example.com');
      cy.get('[name="specialization"]').clear().type('Nurse Anaesthetist');
      cy.get('[name="role"]').clear().type('Nurse');
      cy.get('[name="phoneNumber"]').clear().type('9876543210');
  
      
      cy.get('button').contains('Update').click();
  
      
      cy.wait('@updateStaffProfile').its('request.body').should('include', {
        firstName: 'Petr',
        lastName: 'Yan Updated',
        email: 'petr.yan.updated@example.com',
        specialization: 'Nurse Anaesthetist',
        role: 'Nurse',
        phoneNumber: '9876543210',
      });
  
      
     
    });
  
  });
  