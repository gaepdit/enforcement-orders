context('Authentication', () => {
  beforeEach(() => {
    cy.visit('Account/Login')
  })

  it('fails if login does not include token', () => {
    cy.request({
      method: 'Post',
      url: 'Account/Login',
      form: true,
      followRedirect: false,
      failOnStatusCode: false,
      body: {
        EmailAddress: Cypress.env('username'),
        Password: Cypress.env('password'),
      },
    }).should((resp) => {
      expect(resp.status).to.eq(400)
    })
    cy.getCookie('.AspNetCore.EnfoCookie').should('not.exist')
  })

  it('sets identity cookie on successful login', () => {
    cy.login(Cypress.env('username'), Cypress.env('password'))
    cy.getCookie('.AspNetCore.EnfoCookie').should('exist')
  })

  it('can log in and log out', () => {
    cy.get('h2').should('contain.text', 'Sign in to your account')

    cy.get('#EmailAddress').type(Cypress.env('username'))
    cy.get('#Password').type(Cypress.env('password'), { log: false })
    cy.contains('Sign In').click()

    cy.url().should('eq', Cypress.config().baseUrl + '/')
    cy.get('h2').should('contain.text', 'Currently Published Orders')
    cy.getCookie('.AspNetCore.EnfoCookie').should('exist')

    cy.contains('Account').click()
    cy.contains('Sign Out').click()

    cy.url().should('eq', Cypress.config().baseUrl + '/Account/Login')
    cy.get('h2').should('contain.text', 'Sign in to your account')
    cy.getCookie('.AspNetCore.EnfoCookie').should('not.exist')
  })

  it('does not log in if password is incorrect', () => {
    cy.get('#EmailAddress').type(Cypress.env('username'))
    cy.get('#Password').type('123')
    cy.contains('Sign In').click()

    cy.url().should('eq', Cypress.config().baseUrl + '/Account/Login')
    cy.get('h2').should('contain.text', 'Sign in to your account')
    cy.getCookie('.AspNetCore.EnfoCookie').should('not.exist')

    cy.contains('Invalid login attempt')
  })

  it('does not log in if user does not exist', () => {
    cy.get('#EmailAddress').type('no-one@example.net')
    cy.get('#Password').type('123')
    cy.contains('Sign In').click()

    cy.url().should('eq', Cypress.config().baseUrl + '/Account/Login')
    cy.get('h2').should('contain.text', 'Sign in to your account')
    cy.getCookie('.AspNetCore.EnfoCookie').should('not.exist')

    cy.contains('Account does not exist')
  })
})
