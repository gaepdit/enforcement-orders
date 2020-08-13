context('Menu', () => {
  beforeEach(() => {
    cy.login(Cypress.env('username'), Cypress.env('password'))
  })

  it('has correct menu items', () => {
    cy.visit('/')

    // Dashboard
    cy.contains('a', 'Dashboard').should('have.attr', 'href', '/')

    // Orders
    cy.contains('button', 'Orders').click()
    cy.contains('a', 'Add New Order').should(
      'have.attr',
      'href',
      '/EnforcementOrders/Create'
    )
    cy.contains('a', 'Search Orders').should(
      'have.attr',
      'href',
      '/EnforcementOrders'
    )

    // Admin
    cy.contains('button', 'Admin').click()
    cy.contains('a', 'EPD Contacts').should('have.attr', 'href', '/EpdContacts')
    cy.contains('a', 'EPD Addresses').should('have.attr', 'href', '/Addresses')
    cy.contains('a', 'Legal Authorities').should(
      'have.attr',
      'href',
      '/LegalAuthorities'
    )
    cy.contains('a', 'Change Log').should('have.attr', 'href', '/Home/Changelog')

    // Account
    cy.contains('button', 'Account').click()
    cy.contains('a', 'View Profile').should(
      'have.attr',
      'href',
      '/EnfoUsers/Profile'
    )
    cy.contains('a', 'Sign Out').should('have.attr', 'href', '/Account/Logout')
    cy.contains('a', 'Register a New User').should(
      'have.attr',
      'href',
      '/Account/Register'
    )
  })
})
