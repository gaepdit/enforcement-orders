# ENFO UI Specification

* Public dashboard `/EnforcementOrders`
* Admin dashboard `/Admin`
* Find `/Find/{string}`
* Enforcement orders search `/EnforcementOrders/Search`
    * Public view `/EnforcementOrders/Details/{id}`
    * Admin view `/EnforcementOrders/Admin/{id}`
    * Current proposed orders `/EnforcementOrders/CurrentProposed`
    * Recently executed orders `/EnforcementOrders/RecentExecuted`
    * Create `/EnforcementOrders/Create`
    * Edit `/EnforcementOrders/Edit/{id}`
    * Delete `/EnforcementOrders/Delete/{id}`
* Site maintenance `/Maintenance`
    * Addresses list `/Maintenance/Addresses`
        * Create `/Maintenance/Addresses/Create`
        * Edit `/Maintenance/Addresses/Edit/{id}`
    * EPD Contacts list `/Maintenance/EpdContacts`
        * Create `/Maintenance/EpdContacts/Create`
        * Edit `/Maintenance/EpdContacts/Edit/{id}`
    * Legal Authorities list `/Maintenance/LegalAuthorities`
        * Create `/Maintenance/LegalAuthorities/Create`
        * Edit `/Maintenance/LegalAuthorities/Edit/{id}`
* Account profile `/Account`
* Users list `/Users`
    * View user profile `/Users/Details/{id}`
    * Edit user profile `/Users/Edit/{id}`

## Redirects

* `/` redirects to `/EnforcementOrders` if not logged in
* `/` redirects to `/Dashboard` if logged in
* `/Orders/*` redirects to `/EnforcementOrders/*`

## Menu

Public:

* 🏠 (Public dashboard)
* Search

User:

* 🏠 (Admin dashboard)
* +&nbsp;New Order
* Search
* Admin
    * Site Maintenance
    * ENFO Users
    * Public dashboard
* Account
    * View Profile
    * Sign out