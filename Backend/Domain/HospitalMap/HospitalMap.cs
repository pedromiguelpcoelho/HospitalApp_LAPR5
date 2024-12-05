using System;
using System.Collections.Generic;
using System.Linq;

public class HospitalMap
{
    public string GroundTextureUrl { get; set; }
    public string WallTextureUrl { get; set; }
    public string DoorTextureUrl { get; set; }
    public string SurgeryRoomDoorTextureUrl { get; set; }
    public string BedTextureUrl { get; set; }
    public string AderecoTextureUrl { get; set; }
    public string PatientTextureUrl { get; set; }
    public string PatientModelOBJUrl { get; set; }
    public string SurgeonModelOBJUrl { get; set; }
    public string SurgeonTextureUrl { get; set; }
    public string TroleyModelOBJUrl { get; set; }
    public string TroleyTextureUrl { get; set; }
    public string TweezerTextureUrl { get; set; }
    public string SyringeTextureUrl { get; set; }

    public Size Size { get; set; }
    public int[,] Map { get; set; }
    public List<int> BusyRooms { get; set; }

    public HospitalMap()
    {
        BusyRooms = new List<int>();
    }




    // Updates the map with current busy rooms and door status
    public void UpdateMap(int[,] map, List<Guid> busyRooms, Dictionary<Guid, (int roomNumber, int row, int col)> roomGuidMapping)
{
    // Itera sobre a lista de quartos ocupados (busyRooms)
    foreach (var roomGuid in busyRooms)
    {
        Console.WriteLine($"Room GUID OF HOSPITAL MAP DOMAIN: {roomGuid}");
        // Verifica se o GUID do quarto existe no dicionário
        if (roomGuidMapping.TryGetValue(roomGuid, out var roomInfo))
        {
            // Obtém as coordenadas da sala (linha e coluna)
            int row = roomInfo.row;
            int col = roomInfo.col;

            Console.WriteLine($"Room Number to change: {roomInfo.roomNumber}, Row: {row}, Col: {col}");

            // Verifica se a célula no mapa corresponde ao centro da sala (4) e se deve ser marcada como ocupada
            if (map[row, col] == 4) // Se o valor atual da célula for 4, que indica o centro da sala
            {
                // Marca a sala como ocupada (valor 9)
                map[row, col] = 9;
            }
        }
    }

    // Após iterar sobre os quartos ocupados, percorre todo o dicionário para garantir que os quartos não ocupados sejam definidos como 4
    foreach (var roomInfo in roomGuidMapping.Values)
    {
        int row = roomInfo.row;
        int col = roomInfo.col;

        // Se a célula não foi modificada (não está 9) e o quarto não está ocupado, defina como 4
        if (map[row, col] != 9)
        {
            map[row, col] = 4;
        }
    }
}


}
