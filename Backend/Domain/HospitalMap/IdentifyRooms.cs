using System;
using System.Collections.Generic;

namespace Backend.Domain.HospitalMap
{
    public class IdentifyRoomsFromMap
    {
        int[,] map = {
            {3, 2, 2, 2, 3, 5, 2, 3, 2, 2, 2, 1},
            {1, 0, 0, 0, 7, 0, 0, 8, 0, 0, 0, 1},
            {1, 0, 4, 0, 7, 0, 0, 8, 0, 4, 0, 1},
            {1, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 1},
            {3, 2, 2, 2, 1, 0, 0, 3, 2, 2, 2, 1},
            {1, 0, 0, 0, 6, 0, 0, 6, 0, 0, 0, 1},
            {1, 0, 9, 0, 6, 0, 0, 6, 0, 9, 0, 1},
            {1, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 1},
            {3, 2, 2, 2, 1, 0, 0, 3, 2, 2, 2, 1},
            {1, 0, 0, 0, 6, 0, 0, 8, 0, 0, 0, 1},
            {1, 0, 9, 0, 6, 0, 0, 8, 0, 4, 0, 1},
            {1, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 1},
            {2, 2, 2, 2, 2, 5, 2, 2, 2, 2, 2, 0}
        };

        // Dicionário para associar GUID a cada quarto
        Dictionary<Guid, (int roomNumber, int row, int col)> roomGuidMapping;

        public IdentifyRoomsFromMap(List<Guid> surgeryRooms)
        {
            roomGuidMapping = IdentifyRooms(map, surgeryRooms);  // Identifica os quartos e gera os GUIDs

            // Exibe os resultados
            foreach (var room in roomGuidMapping)
            {
                Console.WriteLine($"Room GUID: {room.Key}, Room Number: {room.Value.roomNumber}, Row: {room.Value.row}, Col: {room.Value.col}");
            }
        }

        public Dictionary<Guid, (int roomNumber, int row, int col)> IdentifyRooms(int[,] map, List<Guid> surgeryRooms)
        {
            Dictionary<Guid, (int, int, int)> rooms = new Dictionary<Guid, (int, int, int)>();
            int roomCount = 1;  // Inicia o contador de quartos
            int surgeryRoomIndex = 0;  // Para iterar sobre a lista de surgeryRooms

            for (int i = 1; i < map.GetLength(0) - 1; i++)  // Evita as bordas
            {
                for (int j = 1; j < map.GetLength(1) - 1; j++)
                {
                    // Verifica se o centro é 4 ou 9, que pode ser um quarto
                    if (map[i, j] == 4 || map[i, j] == 9)
                    {
                        // Verifica se a célula 4 está cercada por zeros
                        if (IsRoom(map, i, j))
                        {
                            // Atribui um GUID a partir da lista de cirurgia
                            Guid roomGuid = surgeryRoomIndex < surgeryRooms.Count ? surgeryRooms[surgeryRoomIndex++] : Guid.NewGuid();

                            // Adiciona o quarto ao dicionário com o GUID
                            rooms.Add(roomGuid, (roomCount++, i, j));

                            MarkRoom(map, i, j, roomCount - 1);  // Marca o quarto como visitado
                        }
                    }
                }
            }

            return rooms;
        }

        private bool IsRoom(int[,] map, int row, int col)
        {
            int[] surroundingCells = new int[]
            {
                map[row - 1, col - 1], map[row - 1, col], map[row - 1, col + 1],
                map[row, col - 1], map[row, col + 1],
                map[row + 1, col - 1], map[row + 1, col], map[row + 1, col + 1]
            };

            foreach (var cell in surroundingCells)
            {
                // Verifica se algum valor ao redor não é 0 ou 9
                if (cell != 0 && cell != 9)
                {
                    return false;
                }
            }

            return true;
        }

        private void MarkRoom(int[,] map, int row, int col, int roomNumber)
        {
            // Marca a célula central com o número do quarto
            map[row, col] = roomNumber;

            // Marca as células ao redor como parte do quarto
            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if (map[i, j] == 0)  // Marca apenas as células vazias
                    {
                        map[i, j] = roomNumber;
                    }
                }
            }
        }
    }
}
