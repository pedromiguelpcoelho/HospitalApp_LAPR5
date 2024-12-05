# Appointment Readme

## Appointment
Represents scheduled operation of a patient by a set of staff occurring in a room in a time slot.
• Attributes:
- `ID` (unique identifier)
- ‘Request ID’ (linked to the request that gave rise to this appointment)
- `Room ID` (linked to a specific room)
- `Date and Time` (of the operation)
- `Status` (scheduled, completed, canceled)
• Rules:
- Operations appointments must be assigned to a set of staff, a room in a time
slot.
- Operations cannot exceed the estimated time unless rescheduled.
- An appointment cannot be scheduled if the staff or room is unavailable at that
time.
- The appointment type should match the staff’s specializations and room
availability.

## Customer specifications and clarifications

**[4 de November de 2024]** 

* **Question:** Em várias US do projeto temos a informação de que o Patient "book appointments". De acordo com o projeto, o conceito Appointment possui um "Request ID", que é o Operation Request associado, que é criado por um Staff, e também possui informação como "Room ID" que não é conhecida pelo Patient.

Sendo assim, quem é que requisita o Appointment, tendo em conta que o Operation Request já tem de estar associado?

  * **Answer:** no âmbito atual do sistema o paciente não faz marcaºões. as unicas marcações existentes são as cirurgias escalonadas pelo módulo de planeamento. o paciente pode consultar as cirurgias requisitadas/escalonadas para ele


**[data]**

* **Question:** 

  * **Answer:** 

**[data]**

* **Question:** 

  * **Answer:** 
