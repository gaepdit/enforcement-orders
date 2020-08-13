context('Search form', () => {
  beforeEach(() => {
    cy.login(Cypress.env('username'), Cypress.env('password'))
  })

  it('can use search form', () => {
    cy.visit('/')
    cy.contains('main a', 'Search Orders').click()
    cy.url().should('eq', Cypress.config().baseUrl + '/EnforcementOrders')

    cy.get('tbody').should('exist')
    cy.get('tbody tr').should('have.length.greaterThan', 0)

    cy.get('#county').select('Fulton')
    cy.contains('input', 'Search').click()

    cy.get('tbody tr').should('have.length.greaterThan', 0)

    cy.get('tbody a').eq(0).click()
    cy.url().should(
      'contain',
      Cypress.config().baseUrl + '/EnforcementOrders/Details/'
    )
    cy.get('h2').should('contain.text', 'Enforcement Order')
  })

  it('can use find box', () => {
    cy.visit('/')
    cy.get('#OrderFilter').type('EPD-WP-8734')
    cy.contains('button', 'Find').click()

    cy.url().should(
      'contain',
      Cypress.config().baseUrl + '/EnforcementOrders/Details/'
    )
    cy.get('h2').should('contain.text', 'Enforcement Order EPD-WP-8734')
  })
})
