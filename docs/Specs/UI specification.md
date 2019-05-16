# ENFO UI Specification

## Admin UI

* Dashboard [Index]
    - Current proposed orders [#proposed]
    - Current executed orders [#executed]
* Enforcement Orders [Orders]
    - View all
        - Filters
            - Facility name
            - Authority
            - County
            - Date range
            - Executed/Proposed/All
            - Active/Deleted/All
        - Results
        - Pagination options
    - Create new (form) [Orders/Add]
    - View single [Orders/{id}]
    - Edit single [Orders/{id}/Edit]
* Admin lists
    - Public Comment Contacts
    - Counties
    - Legal Authorities
* User Admin [Users]
    - List all users
    - View user [Users/{id}]
    - Edit user permissions [Users/{id}/Edit]

## Public UI

* Search [Index]
    - Form 
        - Facility name
        - Authority
        - County
        - Date range
        - Status (Executed/Proposed/All)
    - Results
    - Pagination options
* Details [Details/{id}]
* Current proposed orders [Proposed]
* Current executed orders [Executed]
* RSS feed
