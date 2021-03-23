# ENFO UI Specification

* Public dashboard `/`
* Admin dashboard `/Admin`
* Find `/Find/{string}`
* Enforcement orders search `/Search`
    * Public view `/Details/{id}`
    * Admin view `/Admin/Details/{id}`
    * Current proposed orders `/CurrentProposed`
    * Recently executed orders `/RecentExecuted`
    * Create `/Admin/Create`
    * Edit `/Admin/Edit/{id}`
    * Delete `/Admin/Delete/{id}`
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

* `/Orders/*` redirects to `/*`

## Menu

Public:

* 🏠 (Public dashboard)
* Search
* Admin

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