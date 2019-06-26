# ENFO UI Specification

If the label ***(Admin)*** is applied to a page or section, then it applies to all sub-pages as well.

* Dashboard `/`
    - Current orders
        - Current proposed orders
        - Current executed orders
    - ***(Admin)*** Unpublished orders
        - Pending orders
        - Draft orders
* Search `/Orders`
    - View (limited details) `/Orders/Details/{id}`
    - Current proposed orders `/Orders/CurrentProposed`
    - Recently executed orders `/Orders/RecentExecuted`
* ***(Admin)*** Search (allows additional fields) `/OrdersAdmin`
    - View (full details) `/OrdersAdmin/Details/{id}`
    - Create `/OrdersAdmin/Create`
    - Edit `/OrdersAdmin/Edit/{id}`
    - Delete `/OrdersAdmin/Delete/{id}`
* ***(Admin)*** Maintenance `/Maintenance`
    - EPD Contacts `/Maintenance/EpdContacts`
    - Addresses `/Maintenance/Addresses`
    - Legal Authorities `/Maintenance/LegalAuthorities`
* ***(Admin)*** Profile `/Account`
    - Log in `/Account/Login`
    - Edit profile `/Account/Edit`
    - Change password `/Account/ChangePassword`
    - Reset password `/Account/ForgotPassword`
* ***(Admin)*** Users `/Users`
    - Register new user `/Users/Register`
    - View user `/Users/Details/{id}`
    - Edit user profile `/Users/Edit/{id}`
* Change log `/Changelog`
* API documentation `/api-docs/`
