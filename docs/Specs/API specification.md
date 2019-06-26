# ENFO API Specification

## Address

* GET: `api/Addresses`

* GET: `api/Addresses/{int:id}`

* *[Authorize]* POST: `api/Addresses`

* *[Authorize]* PUT: `api/Addresses/{int:id}`

## County

* GET: `api/Counties`

* GET: `api/Counties/{int:id}`

## Enforcement Order

* GET: `api/Orders/{?params}`

    | parameter      | data type        |
    |----------------|------------------|
    | facilityFilter | string           |
    | county         | string           |
    | legalAuth      | int              |
    | fromDate       | DateTime?        |
    | tillDate       | DateTime?        |
    | status         | ActivityState    |
    | pubStatus      | PublicationState |
    | sortOrder      | SortOrder        |
    | orderNumber    | string           |
    | textContains   | string           |
    | page           | int              |
    | pageSize       | int              |

* GET: `api/Orders/{int:id}`

* *[Authorize]* GET: `api/Orders/Detailed/{int:id}`

* GET: `api/Orders/CurrentProposed/{?params}`

    Current Proposed are public proposed orders with comment close date in the future and publication date in the past

    | parameter      | data type     |
    |----------------|---------------|
    | page           | int           |
    | pageSize       | int           |

* GET: `api/Orders/RecentlyExecuted/{?params}`

    Recently Executed are public executed orders with publication date within current week

    | parameter      | data type     |
    |----------------|---------------|
    | page           | int           |
    | pageSize       | int           |

* *[Authorize]* GET: `api/Orders/Draft/{?params}`

    Draft are orders with publication status set to Draft

    | parameter      | data type     |
    |----------------|---------------|
    | page           | int           |
    | pageSize       | int           |

* *[Authorize]* GET: `api/Orders/Pending/{?params}`

    Pending are public proposed or executed orders with publication date after the current week

    | parameter      | data type     |
    |----------------|---------------|
    | page           | int           |
    | pageSize       | int           |

* *[Authorize]* POST: `api/Orders`

* *[Authorize]* PUT: `api/Orders/{int:id}`

## EPD Contact

* GET: `api/EpdContacts`

* GET: `api/EpdContacts/{int:id}`

* *[Authorize]* POST: `api/EpdContacts`

* *[Authorize]* PUT: `api/EpdContacts/{int:id}`

## Legal Authority

* GET: `api/LegalAuthorities`

* GET: `api/LegalAuthorities/{int:id}`

* *[Authorize]* POST: `api/LegalAuthorities`

* *[Authorize]* PUT: `api/LegalAuthorities/{int:id}`
