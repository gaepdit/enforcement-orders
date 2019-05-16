# ENFO Data Models

* Enforcement Orders
    * Facility name (Required)
    * Facility location county (Required)
    * Legal authority (Required)
    * Nature of violation or cause of order (Required)
    * Requirements of order
    * Date notice posted
    * Publication status: draft/active
    * Activity status: proposed, archived, executed
    * Deleted (boolean)
    * Proposed orders only
        * Contact for public comments (Required)
        * Date comment period opens (Required)
        * Date comment period closes (Required)
        * Downloadable file
    * Executed orders only
        * Order Number (pre-filled template) (Required)
        * Date executed (Required)
        * Settlement amount (Required)
        * Downloadable file

* Public Comment Contacts
    * Name
    * Title
    * Organization
    * Address
    * Phone
    * Email
    * Active

* Address
    * Street
    * Street2
    * City
    * State
    * Postal Code
    * Country

* Legal authority
    * Code
    * Name
    * Order number template
    * Active

* County
    * Name
