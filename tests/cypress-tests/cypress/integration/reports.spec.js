context('Public reports', () => {
  it('can view reports', () => {
    cy.visit('/')

    cy.contains('h3', 'Proposed Orders')
      .parent()
      .contains('View Full Report')
      .click()
    cy.url().should('eq', Cypress.config().baseUrl + '/Orders/CurrentProposed')
    cy.get('h2').should('contain.text', 'Current Proposed Enforcement Orders')

    cy.contains('a', 'Current Orders').click()
    cy.url().should('eq', Cypress.config().baseUrl + '/')

    cy.contains('h3', 'Executed Orders')
      .parent()
      .contains('View Full Report')
      .click()
    cy.url().should('eq', Cypress.config().baseUrl + '/Orders/RecentExecuted')
    cy.get('h2').should('contain.text', 'Recently Executed Enforcement Orders')
  })
})
