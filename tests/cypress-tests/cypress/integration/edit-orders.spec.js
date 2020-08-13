if (Cypress.env('writesEnabled'))
  context('Orders', () => {
    beforeEach(() => {
      cy.login(Cypress.env('username'), Cypress.env('password'))
    })

    it('can create and delete an order', () => {
      cy.visit('/EnforcementOrders/Create')
      const newFacility = Cypress._.random(0, 1e6)
      const newOrder = 'EPD-TEST-' + Cypress._.random(1, 1e3)
      const newAmount = Cypress._.random(1000, 1e7)

      cy.get('#FacilityName').type(newFacility)
      cy.get('#County').select('Fulton')
      cy.get('#LegalAuthorityId').select('Air Quality Act')
      cy.get('#OrderNumber').type(newOrder)
      cy.get('#County').select('Fulton')
      cy.get('#SettlementAmount').type(newAmount)
      cy.get('#CommentContactId').selectNth(1)
      cy.get('#CommentPeriodClosesDate').invoke('val', '2020-01-01')
      cy.contains('input', 'Save').click()

      cy.get('.usa-alert-heading').should('have.text', 'Success')
      cy.get('.usa-alert-text').should(
        'have.text',
        'The Enforcement Order has been created.'
      )

      cy.get('table td').eq(0).should('have.text', 'Published')
      cy.get('table td').eq(1).should('have.text', newFacility.toString())
      cy.get('table td')
        .eq(5)
        .should('contain.text', newAmount.toLocaleString())
      cy.get('table td').eq(8).should('have.text', '1/1/2020')

      var confirmMessage = false
      cy.on('window:confirm', (msg) => (confirmMessage = msg))

      cy.contains('a', 'Delete')
        .click()
        .then(() =>
          expect(confirmMessage).to.eq(
            'Are you sure you want to delete this order?'
          )
        )

      cy.get('.usa-alert-text')
        .eq(0)
        .should('have.text', 'The Enforcement Order has been deleted.')
      cy.get('.usa-alert-text')
        .eq(1)
        .should(
          'have.text',
          'This enforcement order has been deleted. It will not be displayed to the public.'
        )
    })

    it('can edit an order', () => {
      cy.visit('/EnforcementOrders')
      cy.get('tbody a').eq(1).click()

      cy.get('h2').should('contain.text', 'Edit Enforcement Order')

      const newFacility = Cypress._.random(0, 1e6)
      const newAmount = Cypress._.random(1, 1e6)

      cy.get('#FacilityName').clear().type(newFacility)
      cy.get('#SettlementAmount').clear().type(newAmount)
      cy.contains('input', 'Save').click()
      cy.get('table td').eq(1).should('have.text', newFacility.toString())
      cy.get('table td')
        .eq(5)
        .should('contain.text', newAmount.toLocaleString())
    })
  })
