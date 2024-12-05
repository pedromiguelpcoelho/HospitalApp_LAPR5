# SurguicalRoom Readme

 **_SurgicalRoom:_** Represents the the surgical rooms of the hospital.

* Attributes:
    * `Room Number` (unique identifier)
    * `Type` (e.g., operating room, consultation room, ICU)
    * `Capacity` (maximum number of patients or staff)
    * `Assigned Equipment` (list of equipment in the room)
    * `Current Status` (available, occupied, under maintenance)
    * Maintenance slots (the slots defined for room maintenance)

* Rules:
    * Each room can host only one event at a time (either an appointment, surgery,
or meeting).
    * The room’s schedule must be managed based on doctor and equipment
availability.
    * Assume that all rooms are fully equipped with the necessary equipment for
every type of operation.