# ENFO UI Specification

* Public dashboard `/`
* Enforcement orders search `/Search`
* View `/Details/{id}`
* Current proposed orders `/CurrentProposed`
* Recently executed orders `/RecentExecuted`
* Admin dashboard `/Admin`
    * Admin quick search `/Admin {find}`
    * Admin search `/Admin/Search`
    * Admin view `/Admin/Details/{id}`
    * Create `/Admin/Create`
    * Edit `/Admin/Edit/{id}`
    * Delete `/Admin/Delete/{id}`
    * Restore `/Admin/Restore/{id}`
* Site maintenance `/Admin/Maintenance`
    * Addresses list `/Admin/Maintenance/Addresses`
        * Create `/Admin/Maintenance/Addresses/Create`
        * Edit `/Admin/Maintenance/Addresses/Edit/{id}`
    * EPD Contacts list `/Admin/Maintenance/EpdContacts`
        * Create `/Admin/Maintenance/EpdContacts/Create`
        * Edit `/Admin/Maintenance/EpdContacts/Edit/{id}`
    * Legal Authorities list `/Admin/Maintenance/LegalAuthorities`
        * Create `/Admin/Maintenance/LegalAuthorities/Create`
        * Edit `/Admin/Maintenance/LegalAuthorities/Edit/{id}`
* Account profile `/Account`
* Users list `/Admin/Users`
    * View user profile `/Admin/Users/Details/{id}`
    * Edit user profile `/Admin/Users/Edit/{id}`

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