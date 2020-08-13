context('Public search form', () => {
  it('can use default search form', () => {
    cy.visit('/')
    cy.contains('Search Orders').click()
    cy.url().should('eq', Cypress.config().baseUrl + '/Orders/Search')

    cy.get('tbody').should('exist')
    cy.get('tbody tr').should('have.length.greaterThan', 0)

    cy.get('#county').select('Fulton')
    cy.contains('input', 'Search').click()

    cy.get('tbody tr').should('have.length.greaterThan', 0)

    cy.get('tbody a').eq(0).click()
    cy.url().should('contain', Cypress.config().baseUrl + '/Orders/Details/')
    cy.get('h2').should('contain.text', 'Enforcement Order')
  })
})
