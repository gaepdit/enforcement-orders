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
    * Add `/Admin/Add`
    * Edit `/Admin/Edit/{id}`
    * Delete `/Admin/Delete/{id}`
    * Restore `/Admin/Restore/{id}`
* Site maintenance `/Admin/Maintenance`
    * EPD Addresses list `/Admin/Maintenance/Address`
        * Add `/Admin/Maintenance/Addresses/Add`
        * Edit `/Admin/Maintenance/Address/Edit/{id}`
    * EPD Contacts list `/Admin/Maintenance/Contact`
        * Add `/Admin/Maintenance/Contacts/Add`
        * Edit `/Admin/Maintenance/Contact/Edit/{id}`
    * Legal Authorities list `/Admin/Maintenance/LegalAuthority`
        * Add `/Admin/Maintenance/LegalAuthorities/Add`
        * Edit `/Admin/Maintenance/LegalAuthority/Edit/{id}`
* Account profile `/Account`
    * Account Login `/Account/Login`
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