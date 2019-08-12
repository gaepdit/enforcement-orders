# ENFO API Specification

## Address

* GET: `api/Addresses/{?params}`

    | parameter      | data type     |
    |----------------|---------------|
    | pageSize       | int           |
    | page           | int           |

* GET: `api/Addresses/{int:id}`

* *[Authorize]* POST: `api/Addresses/{?params}`

    | parameter      | data type     |
    |----------------|---------------|
    | pageSize       | int           |
    | page           | int           |

* *[Authorize]* PUT: `api/Addresses/{int:id}`

## County

* GET: `api/Counties/{?params}`

    | parameter      | data type     |
    |----------------|---------------|
    | pageSize       | int           |
    | page           | int           |

* GET: `api/Counties/{int:id}`

## Enforcement Order

* GET: `api/EnforcementOrders/{?params}`

    Only authorized users can request Orders with pubStatus other than "Published".

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
    | pageSize       | int              |
    | page           | int              |

* GET: `api/EnforcementOrders/{int:id}`

* *[Authorize]* GET: `api/EnforcementOrders/Detailed/{int:id}`

* GET: `api/EnforcementOrders/Count/{?params}`

    Only authorized users can request Orders with pubStatus other than "Published".

    | parameter      | data type        |
    |----------------|------------------|
    | facilityFilter | string           |
    | county         | string           |
    | fromDate       | DateTime?        |
    | tillDate       | DateTime?        |
    | status         | ActivityState    |
    | pubStatus      | PublicationState |
    | sortOrder      | SortOrder        |
    | orderNumber    | string           |

* GET: `api/EnforcementOrders/CurrentProposed/{?params}`

    Current Proposed are public proposed orders with comment close date in the future and publication date in the past

    | parameter      | data type     |
    |----------------|---------------|
    | pageSize       | int           |
    | page           | int           |

* GET: `api/EnforcementOrders/RecentlyExecuted/{?params}`

    Recently Executed are public executed orders with publication date within current week

    | parameter      | data type     |
    |----------------|---------------|
    | pageSize       | int           |
    | page           | int           |

* *[Authorize]* GET: `api/EnforcementOrders/Draft/{?params}`

    Draft are orders with publication status set to Draft

    | parameter      | data type     |
    |----------------|---------------|
    | pageSize       | int           |
    | page           | int           |

* *[Authorize]* GET: `api/EnforcementOrders/Pending/{?params}`

    Pending are public proposed or executed orders with publication date after the current week

    | parameter      | data type     |
    |----------------|---------------|
    | pageSize       | int           |
    | page           | int           |

* *[Authorize]* POST: `api/EnforcementOrders`

* *[Authorize]* PUT: `api/EnforcementOrders/{int:id}`

## EPD Contact

* GET: `api/EpdContacts/{?params}`

    | parameter      | data type     |
    |----------------|---------------|
    | pageSize       | int           |
    | page           | int           |

* GET: `api/EpdContacts/{int:id}`

* *[Authorize]* POST: `api/EpdContacts`

* *[Authorize]* PUT: `api/EpdContacts/{int:id}`

## Legal Authority

* GET: `api/LegalAuthorities/{?params}`

    | parameter      | data type     |
    |----------------|---------------|
    | pageSize       | int           |
    | page           | int           |

* GET: `api/LegalAuthorities/{int:id}`

* *[Authorize]* POST: `api/LegalAuthorities`

* *[Authorize]* PUT: `api/LegalAuthorities/{int:id}`
