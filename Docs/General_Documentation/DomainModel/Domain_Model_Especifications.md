# Domain Model Especifications

## Customer Specifications and Clarifications

[27-09-24]
* Question: O user apenas pode ser um staff ou patient ou poderá ser algo mais?
    * Answer: Os utilizadores do sistema são os administradores, as enfermeiras e os médicos, bem como os pacientes (com 
  limitações de funcionalidade)
--------------------
[28-09-24]
* Question: What is the difference between appointment, surgery, and operation?
    * Answer: Surgery is a medical procedure (e.g., hip surgery), while an operation request is when a doctor schedules 
  that surgery for a patient. An appointment is the scheduled date for the operation, determined by the planning module.
-----------------
[5-10-24]
* Question: Hello Mr. Client. The filters are And or OR. For example, if I filter for a Patient named John and Age 24, do
  you want every John who is 24 years old or every Patient who is called John or is 24 years old
  * Answer: if more than one search/filter parameter is used, the combination of filters should be considered as AND
----------------
[7-10-24]
* Question: How should the specialization be assigned to a staff?
  Should the admin write it like a first name? Or should the admin select the specialization?
    * Answer: The system has a list of specializations. Staff is assigned a specialization from that list.


[]
* Question:
    * Answer:

## Domain Model Level 1

High-Level Overview: This level provides a broad view of the system, showing the main entities and their relationships. 
It focuses on the primary components and their interactions without going into detailed attributes or methods. 
This level is useful for stakeholders to understand the overall architecture and major components of the system.

## Domain Model Level 2

Detailed View: This level delves deeper into the entities, showing their attributes and more specific relationships. 
It includes details such as the properties of each entity and the types of relationships (e.g., one-to-many, many-to-many). 
This level is useful for developers and designers to understand the data structure and how entities are connected.

## Domain Model Level 3

Aggregates and Value Objects: This level focuses on the domain aggregates, which are clusters of related entities 
and value objects that are treated as a single unit for data changes. It highlights the root entities and their associated 
value objects, providing a detailed view of the internal structure of each aggregate. This level is crucial for implementing 
domain-driven design (DDD) principles, ensuring consistency and integrity within the aggregates.