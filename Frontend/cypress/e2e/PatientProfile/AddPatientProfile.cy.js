describe('AddPatientProfilePage', () => {
  //-------------------------------------
  //--------ADD PATIENT PROFILE----------
  //-------------------------------------

  // Add a correct Patient Profile
  it('passes', () => {
    // Intercepta o pedido à API de criação do PatientProfile
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
  });

  //-----------------------------------------------------------
  // Add a Patient Profile with invalid email

  it('should display error for invalid email', () => {
    // Visita a página inicial
    cy.visit('http://localhost:3000');

    // Preenche o formulário de login
    cy.get('[type="email"]').clear().type('josecarlos.mendes.2003@gmail.com');
    cy.get('[type="password"]').clear().type('Jose@2024');
    cy.get('form > button').click();

    // Navega para a página de adicionar perfil de paciente
    cy.get(':nth-child(2) > ul > :nth-child(1) > a').click();

    // Preencher o formulário com email inválido
    cy.get('[placeholder="First Name"]').clear().type('Manuel');
    cy.get('[placeholder="Last Name"]').clear().type('Silva');
    cy.get('[placeholder="Full Name"]').clear().type('Manuel Silva');
    cy.get('[type="date"]').clear().type('2004-03-10');
    cy.get('[type="email"]').clear().type('invalid-email'); // Email inválido
    cy.get('[placeholder="Contact Information"]').clear().type('937772162');

    // Submeter o formulário
    cy.get('#profile-form > button').click();

    // Verificar que o erro correto é exibido
    cy.contains("Email address must contain at least one '@' and one '.'.").should('be.visible');
  });

  //-------------------------------------
  // Add a Patient Profile with invalid phone number

  it('should display error for invalid phone number', () => {
    // Visita a página inicial
    cy.visit('http://localhost:3000');

    // Preenche o formulário de login
    cy.get('[type="email"]').clear().type('josecarlos.mendes.2003@gmail.com');
    cy.get('[type="password"]').clear().type('Jose@2024');
    cy.get('form > button').click();

    // Navega para a página de adicionar perfil de paciente
    cy.get(':nth-child(2) > ul > :nth-child(1) > a').click();

    // Preencher o formulário com número de telefone inválido
    cy.get('[placeholder="First Name"]').clear().type('André');
    cy.get('[placeholder="Last Name"]').clear().type('Couto');
    cy.get('[placeholder="Full Name"]').clear().type('André Silva Couto');
    cy.get('[type="date"]').clear().type('2004-05-10');
    cy.get('[type="email"]').clear().type('andrecouto@gmail.com');
    cy.get('[placeholder="Contact Information"]').clear().type('123'); // Número inválido

    // Submeter o formulário
    cy.get('#profile-form > button').click();

    // Verificar que o erro correto é exibido
    cy.contains('Phone number must have exactly 9 digits and start with 9.').should('be.visible');
  });

  //-------------------------------------
  // Add a Patient Profile same email

  it('should display error for existing email', () => {
    
    cy.intercept('POST', '/api/PatientProfile', (req) => {
      if (req.body.email === 'luna.sp.silva@gmail.com') {
        req.reply({
          statusCode: 400,
          body: {
            message: 'A profile with the same email already exists.',
          },
        });
      } else {
        req.reply({
          statusCode: 201,
          body: {
            id: '67890',
            message: 'Patient Profile created successfully',
          },
        });
      }
    }).as('createPatientProfile');

    // Visita a página inicial
    cy.visit('http://localhost:3000');

    // Preenche o formulário de login
    cy.get('[type="email"]').clear().type('josecarlos.mendes.2003@gmail.com');
    cy.get('[type="password"]').clear().type('Jose@2024');
    cy.get('form > button').click();

    // Navega para a página de adicionar perfil de paciente
    cy.get(':nth-child(2) > ul > :nth-child(1) > a').click();

    // Preencher o formulário com mesmo email
    cy.get('[placeholder="First Name"]').clear().type('André');
    cy.get('[placeholder="Last Name"]').clear().type('Couto');
    cy.get('[placeholder="Full Name"]').clear().type('André Silva Couto');
    cy.get('[type="date"]').clear().type('2004-05-10');
    cy.get('[type="email"]').clear().type('luna.sp.silva@gmail.com');
    cy.get('[placeholder="Contact Information"]').clear().type('955555555');

    // Submeter o formulário
    cy.get('#profile-form > button').click();

    // Verificar que o erro correto é exibido
    cy.contains('A profile with the same email already exists.').should('be.visible');
  });


  //-------------------------------------
  // Add a Patient Profile with same phone number
 
  it('should display error for existing phone number', () => {
    // Intercepta o pedido à API para simular erro de número de telefone já existente
    cy.intercept('POST', '/api/PatientProfile', (req) => {
      if (req.body.contactInformation === '935592162') {
        req.reply({
          statusCode: 400,
          body: {
            message: 'A profile with the same contact information already exists.',
          },
        });
      } else {
        req.reply({
          statusCode: 201,
          body: {
            id: '98765',
            message: 'Patient Profile created successfully',
          },
        });
      }
    }).as('createPatientProfile');

    cy.visit('http://localhost:3000');

    // Preenche o formulário de login
    cy.get('[type="email"]').clear().type('josecarlos.mendes.2003@gmail.com');
    cy.get('[type="password"]').clear().type('Jose@2024');
    cy.get('form > button').click();

    // Navega para a página de adicionar perfil de paciente
    cy.get(':nth-child(2) > ul > :nth-child(1) > a').click();

    // Preencher o formulário com email inválido
    cy.get('[placeholder="First Name"]').clear().type('Manuel');
    cy.get('[placeholder="Last Name"]').clear().type('Silva');
    cy.get('[placeholder="Full Name"]').clear().type('Manuel Silva');
    cy.get('[type="date"]').clear().type('2004-03-10');
    cy.get('[type="email"]').clear().type('manuel123@gmeil.com'); 
    cy.get('[placeholder="Contact Information"]').clear().type('935592162'); // Número já existente

    // Submeter o formulário
    cy.get('#profile-form > button').click();

    // Verificar que o erro correto é exibido
    cy.contains("A profile with the same contact information already exists.").should('be.visible');
  }); 



});
