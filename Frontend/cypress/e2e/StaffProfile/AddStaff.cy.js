describe('AddStaffPage', () => {
    //-------------------------------------
    //--------ADD STAFF MEMBER-------------
    //-------------------------------------
  
    // Add a correct Staff Member
    it('should add a staff member successfully', () => {
      
      cy.intercept('POST', '/api/staff', {
        statusCode: 201,
        body: {
          firstName: 'Almerindo',
          lastName: 'Silva',
          email: 'example@gmail.com',
          phoneNumber: '935219216',
          role: 'Doctor',
          specialization: 'Orthopaedist',
          message: 'Staff member added successfully!',
        },
      }).as('addStaffMember');
  
      
      cy.visit('http://localhost:3000');
  
      
      cy.get('[type="email"]').clear().type('josecarlos.mendes.2003@gmail.com');
      cy.get('[type="password"]').clear().type('Jose@2024');
      cy.get('form > button').click();
  
      
      cy.get(':nth-child(3) > ul > :nth-child(1) > a').click();
  
      
      cy.get('[placeholder="First Name"]').clear().type('Almerindo');
      cy.get('[placeholder="Last Name"]').clear().type('Silva');
      cy.get('[type="email"]').clear().type('example@gmail.com');
      cy.get('[placeholder="Phone Number"]').clear().type('935219216');
      cy.get('select[name="role"]').select('Doctor');
      cy.get('select[name="specialization"]').select('Orthopaedist');
  
      
      cy.get('form > button').click();
  
      
      cy.wait('@addStaffMember').its('request.body').should('include', {
        firstName: 'Almerindo',
        lastName: 'Silva',
        email: 'example@gmail.com',
        phoneNumber: '935219216',
        role: 'Doctor',
        specialization: 'Orthopaedist',
      });
  
      
      cy.contains('Staff member added successfully!').should('be.visible');
    });
  

it('should display error for invalid email', () => {
    
    cy.visit('http://localhost:3000');

    
    cy.get('[type="email"]').clear().type('josecarlos.mendes.2003@gmail.com');
    cy.get('[type="password"]').clear().type('Jose@2024');
    cy.get('form > button').click();

    
    cy.get(':nth-child(3) > ul > :nth-child(1) > a').click();

    
    cy.get('[placeholder="First Name"]').clear().type('Manuel');
    cy.get('[placeholder="Last Name"]').clear().type('Silva');
    cy.get('[type="email"]').clear().type('invalid-email'); 
    cy.get('[placeholder="Phone Number"]').clear().type('937772162');
    cy.get('select[name="role"]').select('Nurse');
    cy.get('select[name="specialization"]').select('Instrumenting Nurse');

    
    cy.get('form > button').click();

    
    
    cy.contains('Staff member added successfully!').should('not.exist');
    
  });

  //-------------------------------------
  // Add a Staff Member with invalid phone number
  it('should display error for invalid phone number', () => {
    
    cy.visit('http://localhost:3000');

    
    cy.get('[type="email"]').clear().type('josecarlos.mendes.2003@gmail.com');
    cy.get('[type="password"]').clear().type('Jose@2024');
    cy.get('form > button').click();

    
    cy.get(':nth-child(3) > ul > :nth-child(1) > a').click();

   
    cy.get('[placeholder="First Name"]').clear().type('André');
    cy.get('[placeholder="Last Name"]').clear().type('Couto');
    cy.get('[type="email"]').clear().type('andrecouto@gmail.com');
    cy.get('[placeholder="Phone Number"]').clear().type('123'); 
    cy.get('select[name="role"]').select('Doctor');
    cy.get('select[name="specialization"]').select('Anaesthetist');

    
    cy.get('form > button').click();
    
    cy.contains('Phone number must have exactly 9 digits and start with 9.').should('be.visible');
  });

  //-------------------------------------
  // Add a Staff Member with existing email
  it('should display error for existing email', () => {
    
    cy.intercept('POST', '/api/staff', (req) => {
      if (req.body.email === 'luna.sp.silva@gmail.com') {
        req.reply({
          statusCode: 400,
          body: {
            message: 'A staff member with the same email already exists.',
          },
        });
      } else {
        req.reply({
          statusCode: 201,
          body: {
            message: 'Staff member added successfully!',
          },
        });
      }
    }).as('addStaffMember');

    
    cy.visit('http://localhost:3000');

    
    cy.get('[type="email"]').clear().type('josecarlos.mendes.2003@gmail.com');
    cy.get('[type="password"]').clear().type('Jose@2024');
    cy.get('form > button').click();

    
    cy.get(':nth-child(3) > ul > :nth-child(1) > a').click();

   
    cy.get('[placeholder="First Name"]').clear().type('André');
    cy.get('[placeholder="Last Name"]').clear().type('Couto');
    cy.get('[type="email"]').clear().type('luna.sp.silva@gmail.com'); 
    cy.get('[placeholder="Phone Number"]').clear().type('955555555');
    cy.get('select[name="role"]').select('Nurse');
    cy.get('select[name="specialization"]').select('Medical Action Assistant');

    
    cy.get('button[type="submit"]').click();

    
    cy.contains('A staff member with the same email already exists.').should('be.visible');
  });

  //-------------------------------------
  // Add a Staff Member with existing phone number
  it('should display error for existing phone number', () => {
   
    cy.intercept('POST', '/staff', (req) => {
      if (req.body.phoneNumber === '914939627') {
        req.reply({
          statusCode: 400,
          body: {
            message: 'The phone number is already in use by another staff member.',
          },
        });
      } else {
        req.reply({
          statusCode: 201,
          body: {
            message: 'Staff member added successfully!',
          },
        });
      }
    }).as('addStaffMember');

    
    cy.visit('http://localhost:3000');

    
    cy.get('[type="email"]').clear().type('josecarlos.mendes.2003@gmail.com');
    cy.get('[type="password"]').clear().type('Jose@2024');
    cy.get('form > button').click();

    
    cy.get(':nth-child(3) > ul > :nth-child(1) > a').click();

    
    cy.get('[placeholder="First Name"]').clear().type('Manuel');
    cy.get('[placeholder="Last Name"]').clear().type('Silva');
    cy.get('[type="email"]').clear().type('manuel123@gmeil.com');
    cy.get('[placeholder="Phone Number"]').clear().type('914939627'); 
    cy.get('select[name="role"]').select('Nurse');
    cy.get('select[name="specialization"]').select('Circulating Nurse');

    
    cy.get('button[type="submit"]').click();

    
    cy.contains('The phone number is already in use by another staff member.').should('be.visible');
  });
});

   

  