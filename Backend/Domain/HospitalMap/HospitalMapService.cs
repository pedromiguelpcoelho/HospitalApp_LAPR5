using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DDDSample1.Domain.Appointments;
using DDDSample1.Domain.OperationRequests;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.SurgeryRooms;
using Backend.Domain.HospitalMap;

namespace DDDSample1.Domain.HospitalMaps
{
    public class HospitalMapService
    {
        private readonly string _hospitalMazePath = Path.Combine(Directory.GetCurrentDirectory(), "Domain/HospitalMap/HospitalMap.json");
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly ISurgeryRoomRepository _surgeryRoomRepository;
        private readonly IOperationTypeRepository _operationTypeRepository;
        private readonly IOperationRequestRepository _operationRequestRepository;
        private Dictionary<Guid, (int roomNumber, int row, int col)> roomGuidMapping;

        public HospitalMapService(
            IAppointmentRepository appointmentRepository,
            ISurgeryRoomRepository surgeryRoomRepository,
            IOperationTypeRepository operationTypeRepository,
            IOperationRequestRepository operationRequestRepository)
        {
            _appointmentRepository = appointmentRepository;
            _surgeryRoomRepository = surgeryRoomRepository;
            _operationTypeRepository = operationTypeRepository;
            _operationRequestRepository = operationRequestRepository;
        }

        public void MapSurgeryRoomsWithMaze()
        {

            // Lê o JSON do arquivo e deserializa
            var json = File.ReadAllText(_hospitalMazePath);
            var hospitalMap = JsonConvert.DeserializeObject<HospitalMap>(json);

            var surgeryRooms = _surgeryRoomRepository.GetAllAsync().Result;
            List<Guid> surgeryRoomsIds = new List<Guid>();
            foreach (var surgeryRoom in surgeryRooms)
            {
                surgeryRoomsIds.Add(surgeryRoom.Id.AsGuid());
            }
            
            IdentifyRoomsFromMap identifyRooms = new IdentifyRoomsFromMap(surgeryRoomsIds);
            roomGuidMapping = identifyRooms.IdentifyRooms(hospitalMap.Map, surgeryRoomsIds);
        }

        public async Task<HospitalMap> GetHospitalMapAsync()
        {
            if (!File.Exists(_hospitalMazePath))
                return null;

            // Lê o JSON do arquivo e deserializa
            var json = File.ReadAllText(_hospitalMazePath);
            var hospitalMap = JsonConvert.DeserializeObject<HospitalMap>(json);

            // Obter as salas ocupadas
            var appointments = await _appointmentRepository.GetAllAsync();
            var busyRooms = new List<Guid>();

            MapSurgeryRoomsWithMaze();

            foreach (var appointment in appointments)
            {
                if (appointment.Status != AppointmentStatus.Canceled && appointment.RoomId != null)
                {
                    var room = await _surgeryRoomRepository.GetByIdAsync(new SurgeryRoomId(appointment.RoomId.Value));
                    if (room != null && DateTime.Now >= appointment.StartTime && 
                        DateTime.Now <= appointment.StartTime.AddMinutes(appointment.Duration.TotalMinutes))
                    {
                        busyRooms.Add(room.Id.AsGuid());
                        Console.WriteLine("Room busy: " + room.RoomNumber);
                        Console.WriteLine("Busy Room GUID: " + room.Id.AsGuid());
                    }
                }
            } 

            // Atualiza o mapa com as salas ocupadas
            hospitalMap.UpdateMap(hospitalMap.Map, busyRooms, roomGuidMapping);

            // Salva o mapa atualizado no JSON
            //await SaveHospitalMapAsync(hospitalMap);

            //IdentifyRoomsFromMap identifyRooms = new IdentifyRoomsFromMap();
            //identifyRooms.IdentifyRooms(hospitalMap.Map);

            

            return hospitalMap;
        }

        public async Task SaveHospitalMapAsync(HospitalMap hospitalMap)
        {
            // Serializar o objeto atualizado para JSON
            string json = JsonConvert.SerializeObject(hospitalMap, Formatting.Indented);

            // Escrever no arquivo JSON
            await File.WriteAllTextAsync(_hospitalMazePath, json);
        }
    }
}
