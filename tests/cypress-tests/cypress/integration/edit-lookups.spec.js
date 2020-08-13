if (Cypress.env('writesEnabled')) 
context('Test data editing', () => {
    beforeEach(() => {
      cy.login(Cypress.env('username'), Cypress.env('password'))
    })

    it('can create and edit a contact', () => {
      const newName = 'Name ' + Cypress._.random(0, 1e3)
      const newTitle = 'Title ' + Cypress._.random(0, 1e3)
      const newOrg = 'Org ' + Cypress._.random(0, 1e3)
      const newPhone =
        Cypress._.random(100, 1e3) +
        '-' +
        Cypress._.random(100, 1e3) +
        '-' +
        Cypress._.random(1000, 1e4)
      const newEmail = 'abc@example.net'

      cy.visit('/EpdContacts')
      cy.contains('Create New').click()
      cy.url().should('eq', Cypress.config().baseUrl + '/EpdContacts/Create')

      cy.get('#ContactName').type(newName)
      cy.get('#Title').type(newTitle)
      cy.get('#Organization').type(newOrg)
      cy.get('#AddressId').selectNth(1)
      cy.get('#Telephone').type(newPhone)
      cy.get('#Email').type(newEmail)
      cy.contains('Save').click()

      cy.url().should(
        'contain',
        Cypress.config().baseUrl + '/EpdContacts/Details'
      )
      cy.get('.usa-alert-text').should(
        'have.text',
        'New EPD Contact has been created.'
      )
      cy.get('table td').eq(0).should('have.text', newName)
      cy.get('table td').eq(1).should('have.text', newTitle)
      cy.get('table td').eq(2).should('have.text', newOrg)
      cy.get('table td').eq(3).should('have.text', newPhone)
      cy.get('table td').eq(4).should('have.text', newEmail)
      cy.get('table td').eq(6).should('contain.text', 'Yes')

      cy.contains('Edit').click()
      cy.get('#ContactName')
        .clear()
        .type('Edited ' + newName)
      cy.get('#Active').click()
      cy.contains('Save').click()

      cy.get('.usa-alert-text').should(
        'have.text',
        'The EPD Contact has been updated.'
      )
      cy.get('table td')
        .eq(0)
        .should('have.text', 'Edited ' + newName)
      cy.get('table td').eq(6).should('contain.text', 'No')
    })

    it('can create and edit an address', () => {
      const newStreet = Cypress._.random(0, 1e3) + ' Street'
      const newStreet2 = 'Apt. ' + Cypress._.random(0, 1e3)
      const newCity = 'City'
      const newZip = Cypress._.random(10000, 1e5)

      cy.visit('/Addresses')
      cy.contains('Create New').click()
      cy.url().should('eq', Cypress.config().baseUrl + '/Addresses/Create')

      cy.get('#Street').type(newStreet)
      cy.get('#City').type(newCity)
      cy.get('#PostalCode').type(newZip)
      cy.contains('input', 'Create').click()

      cy.url().should(
        'contain',
        Cypress.config().baseUrl + '/Addresses/Details'
      )
      cy.get('.usa-alert-text').should(
        'have.text',
        'The Address has been created.'
      )
      cy.get('table td').eq(0).should('have.text', newStreet)
      cy.get('table td').eq(2).should('have.text', newCity)
      cy.get('table td').eq(4).should('have.text', newZip.toString())
      cy.get('table td').eq(6).should('contain.text', 'Yes')

      cy.contains('Edit').click()
      cy.get('#Street2').type(newStreet2)
      cy.get('#PostalCode').clear().type(newZip + '-0000')
      cy.get('#Active').click()
      cy.contains('Save').click()

      cy.get('.usa-alert-text').should(
        'have.text',
        'The Address has been updated.'
      )
      cy.get('table td').eq(1).should('have.text', newStreet2)
      cy.get('table td').eq(4).should('have.text', newZip.toString() + '-0000')
      cy.get('table td').eq(6).should('contain.text', 'No')
    })

    it('can create and edit a legal authority', () => {
      const newAuthority = 'Authority ' + Cypress._.random(0, 1e3)
      const newTemplate = 'TEMP-'

      cy.visit('/LegalAuthorities')
      cy.contains('Create New').click()
      cy.url().should(
        'eq',
        Cypress.config().baseUrl + '/LegalAuthorities/Create'
      )

      cy.get('#AuthorityName').type(newAuthority)
      cy.get('#OrderNumberTemplate').type(newTemplate)
      cy.contains('input', 'Create').click()

      cy.url().should(
        'contain',
        Cypress.config().baseUrl + '/LegalAuthorities/Details'
      )
      cy.get('.usa-alert-text').should(
        'have.text',
        'The Legal Authority has been created.'
      )
      cy.get('table td').eq(0).should('have.text', newAuthority)
      cy.get('table td').eq(1).should('have.text', newTemplate)
      cy.get('table td').eq(2).should('contain.text', 'Yes')

      cy.contains('Edit').click()
      cy.get('#AuthorityName')
        .clear()
        .type('Edited ' + newAuthority)
      cy.get('#Active').click()
      cy.contains('Save').click()

      cy.get('.usa-alert-text').should(
        'have.text',
        'The Legal Authority has been updated.'
      )
      cy.get('table td')
        .eq(0)
        .should('have.text', 'Edited ' + newAuthority)
      cy.get('table td').eq(2).should('contain.text', 'No')
    })
  })
