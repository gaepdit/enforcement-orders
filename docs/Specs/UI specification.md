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
    * EPD Addresses list `/Admin/Maintenance/Address`
        * Create `/Admin/Maintenance/Address/Create`
        * Edit `/Admin/Maintenance/Address/Edit/{id}`
    * EPD Contacts list `/Admin/Maintenance/Contact`
        * Create `/Admin/Maintenance/Contact/Create`
        * Edit `/Admin/Maintenance/Contact/Edit/{id}`
    * Legal Authorities list `/Admin/Maintenance/LegalAuthority`
        * Create `/Admin/Maintenance/LegalAuthority/Create`
        * Edit `/Admin/Maintenance/LegalAuthority/Edit/{id}`
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