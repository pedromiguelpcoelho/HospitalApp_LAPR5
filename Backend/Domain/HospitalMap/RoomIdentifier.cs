using System.Collections.Generic;

namespace DDDSample1.Domain.HospitalMaps
{
    public class RoomIdentifier
    {
        private int[,] roomMap;
        private bool[,] visited;
        private int width, height;

        public RoomIdentifier(int[,] map)
        {
            roomMap = map;
            height = map.GetLength(0);
            width = map.GetLength(1);
            visited = new bool[height, width];
        }

        // Função para encontrar todos os quartos no mapa
        public List<List<(int, int)>> FindRooms(int roomType)
        {
            List<List<(int, int)>> rooms = new List<List<(int, int)>>();

            // Percorrer o mapa e buscar as células que ainda não foram visitadas
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    if (!visited[row, col] && roomMap[row, col] == roomType)
                    {
                        // Encontrou um novo quarto, então faz a busca por DFS/BFS
                        List<(int, int)> room = new List<(int, int)>();
                        DFS(row, col, roomType, room);
                        rooms.Add(room);
                    }
                }
            }
            return rooms;
        }

        // Função de busca em profundidade (DFS) para marcar todas as células de um quarto
        private void DFS(int row, int col, int roomType, List<(int, int)> room)
        {
            if (row < 0 || row >= height || col < 0 || col >= width)
                return;

            if (visited[row, col] || roomMap[row, col] != roomType)
                return;

            visited[row, col] = true;
            room.Add((row, col));

            // Verifica as células ao redor
            DFS(row - 1, col, roomType, room); // Cima
            DFS(row + 1, col, roomType, room); // Baixo
            DFS(row, col - 1, roomType, room); // Esquerda
            DFS(row, col + 1, roomType, room); // Direita
        }
    }
}
