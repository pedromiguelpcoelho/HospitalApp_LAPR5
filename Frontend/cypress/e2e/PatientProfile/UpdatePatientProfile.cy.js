describe('UpdatePatientProfilePage', () => {
    it('should update an existing patient profile successfully by email', () => {

      cy.intercept('POST', '/api/PatientProfile', {
        statusCode: 201,
        body: {
          id: '12345',
          firstName: 'Luna',
          lastName: 'Silva',
          fullName: 'Luna Sofia Silva',
          dateOfBirth: '2004-02-10',
          email: 'luna.sp.silva@gmail.com',
          contactInformation: '935592162',
          message: 'Patient Profile created successfully',
        },
      }).as('createPatientProfile');
  
      // Visita a página inicial
      cy.visit('http://localhost:3000');
  
      // Preenche o formulário de login
      cy.get('[type="email"]').clear().type('josecarlos.mendes.2003@gmail.com');
      cy.get('[type="password"]').clear().type('Jose@2024');
      cy.get('form > button').click();
  
      // Navega para a página de adicionar perfil de paciente
      cy.get(':nth-child(2) > ul > :nth-child(1) > a').click();
  
      // Preenche o formulário de criação do PatientProfile (correto)
      cy.get('[placeholder="First Name"]').clear().type('Luna');
      cy.get('[placeholder="Last Name"]').clear().type('Silva');
      cy.get('[placeholder="Full Name"]').clear().type('Luna Sofia Silva');
      cy.get('[type="date"]').clear().type('2004-02-10');
      cy.get('[type="email"]').clear().type('luna.sp.silva@gmail.com');
      cy.get('[placeholder="Contact Information"]').clear().type('935592162');
  
      // Submete o formulário
      cy.get('#profile-form > button').click();
  
      // Verifica que a chamada à API foi feita com os dados corretos
      cy.wait('@createPatientProfile').its('request.body').should('include', {
        firstName: 'Luna',
        lastName: 'Silva',
        fullName: 'Luna Sofia Silva',
        dateOfBirth: '2004-02-10',
        email: 'luna.sp.silva@gmail.com',
        contactInformation: '935592162',
      });
  
      // Verifica se a mensagem de sucesso é exibida
      cy.contains('Patient profile created successfully!').should('be.visible');

      
      // Dados do perfil que queremos simular como existente
        cy.intercept('GET', '/api/PatientProfile/all?email=luna.sp.silva%40gmail.com', {
          statusCode: 200,
          body: [
            {
              id: '12345',
              firstName: 'Luna',
              lastName: 'Silva',
              fullName: 'Luna Sofia Silva',
              dateOfBirth: '2004-02-10',
              email: 'luna.sp.silva@gmail.com',
              contactInformation: '935592162',
              medicalRecordNumber: '12345',
            }
          ]
        }).as('getPatients');
    
    
        // Simula a navegação para editar o perfil
        cy.get(':nth-child(2) > ul > :nth-child(2) > a').click();


        cy.get('select').select('email');
        cy.get('[type="text"][name="email"]').clear().type('luna.sp.silva@gmail.com');
        cy.get('button').contains('Search').click();
            
        // Aguarda o carregamento do perfil simulado
        cy.wait('@getPatients');

    
        // Verifica que os dados simulados aparecem no formulário
        cy.get('[name="firstName"]').should('have.value', 'Luna');
        cy.get('[name="lastName"]').should('have.value', 'Silva');
        cy.get('[name="fullName"]').should('have.value', 'Luna Sofia Silva');
        cy.get('[name="dateOfBirth"]').should('have.value', '2004-02-10');
        cy.get('[name="email"]').should('have.value', 'luna.sp.silva@gmail.com');
        cy.get('[name="contactInformation"]').should('have.value', '935592162');
        cy.get('[name="medicalRecordNumber"]').should('have.value', '12345');

    
        // Atualiza os dados
        cy.get('[name="firstName"]').clear().type('Luna Atualizado');
        cy.get('[name="lastName"]').clear().type('Silva Atualizado');
        cy.get('[name="contactInformation"]').clear().type('935592163');

    // Dados do perfil que queremos simular como existente
        cy.intercept('PUT', '/api/PatientProfile/update?email=luna.sp.silva%40gmail.com', {
          statusCode: 200,
          body: [
            {
              id: '12345',
              firstName: 'Luna Atualizado',
              lastName: 'Silva Atualizado',
              fullName: 'Luna Sofia Silva',
              dateOfBirth: '2004-02-10',
              email: 'luna.sp.silva@gmail.com',
              contactInformation: '935592163',
              medicalRecordNumber: '12345',
            }
          ]
        }).as('updateProfile');

        // Submete o formulário
        cy.get('form > button').click();


        // Simula o `window.confirm` como se o user clicasse em "OK"
        cy.on('window:confirm', (message) => {
          expect(message).to.eq('Are you sure you want to update the profile of Luna Sofia Silva?');
          return true; // Simula clique em "OK"
        });    

        // Simula o `window.alert` e verifica a mensagem exibida
        cy.on('window:alert', (message) => {
          expect(message).to.eq('Patient profile updated successfully. The patient will be informed about this change.');
        });

        // Verifica que a atualização foi feita com sucesso
        cy.wait('@updateProfile');
          });
  });



