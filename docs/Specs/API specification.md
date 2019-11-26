# ENFO API Specification

## Address

### GET: `api/Addresses`

| parameter       | data type |
|-----------------|-----------|
| pageSize        | int       |
| page            | int       |
| includeInactive | bool      |

### GET: `api/Addresses/{int:id}`

### POST: `[Authorize] api/Addresses`

| parameter | data type |
|-----------|-----------|
| pageSize  | int       |
| page      | int       |

### PUT: `[Authorize] api/Addresses/{int:id}`

## County

### GET: `api/Counties`

| parameter | data type |
|-----------|-----------|
| pageSize  | int       |
| page      | int       |

### GET: `api/Counties/{int:id}`

## Enforcement Order

### GET: `api/EnforcementOrders`

Only authorized users can request Orders with PublicationStatus other than "Published".

| parameter         | data type         |
|-------------------|-------------------|
| facilityFilter    | string            |
| county            | string            |
| legalAuth         | int               |
| fromDate          | DateTime?         |
| tillDate          | DateTime?         |
| status            | ActivityStatus    |
| publicationStatus | PublicationStatus |
| sortOrder         | SortOrder         |
| orderNumber       | string            |
| textContains      | string            |
| pageSize          | int               |
| page              | int               |

### GET: `api/EnforcementOrders/{int:id}`

### GET: `[Authorize] api/EnforcementOrders/Details/{int:id}`

### GET: `api/EnforcementOrders/Count`

Only authorized users can request Orders with PublicationStatus other than "Published".

| parameter         | data type         |
|-------------------|-------------------|
| facilityFilter    | string            |
| county            | string            |
| fromDate          | DateTime?         |
| tillDate          | DateTime?         |
| status            | ActivityStatus    |
| publicationStatus | PublicationStatus |
| sortOrder         | SortOrder         |
| orderNumber       | string            |

### GET: `api/EnforcementOrders/CurrentProposed`

Current Proposed are public proposed orders with comment close date in the future and publication date in the past

| parameter | data type |
|-----------|-----------|
| pageSize  | int       |
| page      | int       |

### GET: `api/EnforcementOrders/RecentlyExecuted`

Recently Executed are public executed orders with publication date within current week

| parameter | data type |
|-----------|-----------|
| pageSize  | int       |
| page      | int       |

### GET: `[Authorize] api/EnforcementOrders/Drafts`

Drafts are orders with publication status set to Draft

| parameter | data type |
|-----------|-----------|
| pageSize  | int       |
| page      | int       |

### GET: `[Authorize] api/EnforcementOrders/Pending`

Pending are public proposed or executed orders with publication date after the current week

| parameter | data type |
|-----------|-----------|
| pageSize  | int       |
| page      | int       |

### POST: `[Authorize] api/EnforcementOrders`

### PUT: `[Authorize] api/EnforcementOrders/{int:id}`

## EPD Contact

### GET: `api/EpdContacts`

| parameter       | data type |
|-----------------|-----------|
| pageSize        | int       |
| page            | int       |
| includeInactive | bool      |

### GET: `api/EpdContacts/{int:id}`

### POST: `[Authorize] api/EpdContacts`

### PUT: `[Authorize] api/EpdContacts/{int:id}`

## Legal Authority

### GET: `api/LegalAuthorities`

| parameter       | data type |
|-----------------|-----------|
| pageSize        | int       |
| page            | int       |
| includeInactive | bool      |

### GET: `api/LegalAuthorities/{int:id}`

### POST: `[Authorize] api/LegalAuthorities`

### PUT: `[Authorize] api/LegalAuthorities/{int:id}`
