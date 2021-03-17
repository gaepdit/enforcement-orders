# Data Models

## Enforcement Order

* Facility name *(required)*
* Facility location county *(required)*
* **Legal authority** *(required)*
* Nature of violation or cause of order *(required)*
* Requirements of order *(required)*
* Settlement amount *(required if executed)*
* Publication status *(enum): Draft/Published *(required)*
* Order Number *(required)*
* Is proposed *(bool):*
    * Date comment period closes *(required)*
    * **Contact** for public comments *(required)*
    * Date proposed order posted *(required)*
* Is executed *(bool):*
    * Date executed *(required)*
    * Date executed order posted *(required)*
* Is hearing scheduled *(bool):*
    * Date of hearing *(required)*
    * Hearing location *(required)*
    * Date hearing-related comment period closes *(required)*
    * **Contact** for hearing-related comments *(required)*
* Deleted *(bool)*

## EPD Contact

* Name *(required)*
* Title *(required)*
* Organization *(required)*
* **Address** *(required)*
* Phone
* Email
* Active *(bool)*

## Address

* Street *(required)*
* Street2 *(required)*
* City *(required)*
* State *(required)*
* Postal Code
* Active *(bool)*

## Legal authority

* Name *(required)*
* Active *(bool)*

## County

* Name *(required)*
