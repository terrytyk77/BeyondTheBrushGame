using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;
using UnityEngine.Tilemaps;

public class Pathfinding
{
    float Heuristic(Vector3Int w1, Vector3Int w2)
    {
        return Vector3Int.Distance(w1, w2);
    }

    List<Vector3Int> ReconstructPath(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int current, Vector3Int start)
    {
        List<Vector3Int> finalPath = new List<Vector3Int>();
        finalPath.Add(current);
        while (current != start)
        {
            current = cameFrom[current];
            finalPath.Add(current);
        }
        finalPath.Reverse();
        return finalPath;
    }

    List<Vector3Int> GetNeighbors(Tilemap tilemap, Vector3Int node)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();
        Vector3 worldtile = tilemap.CellToWorld(node);
        Vector3Int rightNeighbor = tilemap.WorldToCell(new Vector3(worldtile.x + tilemap.cellSize.x, worldtile.y, worldtile.z));
        Vector3Int leftNeighbor = tilemap.WorldToCell(new Vector3(worldtile.x - tilemap.cellSize.x, worldtile.y, worldtile.z));
        Vector3Int topNeighbor = tilemap.WorldToCell(new Vector3(worldtile.x, worldtile.y + tilemap.cellSize.y, worldtile.z));
        Vector3Int bottomNeighbor = tilemap.WorldToCell(new Vector3(worldtile.x, worldtile.y - tilemap.cellSize.y, worldtile.z));
        Vector3Int diagonalNeighbor;

        if (tilemap.GetTile(rightNeighbor) == null)
            neighbors.Add(rightNeighbor);

        if (tilemap.GetTile(leftNeighbor) == null)
            neighbors.Add(leftNeighbor);

        if (tilemap.GetTile(topNeighbor) == null)
            neighbors.Add(topNeighbor);

        if (tilemap.GetTile(bottomNeighbor) == null)
            neighbors.Add(bottomNeighbor);

        diagonalNeighbor = tilemap.WorldToCell(new Vector3(worldtile.x + tilemap.cellSize.x, worldtile.y - tilemap.cellSize.y, worldtile.z));
        if (tilemap.GetTile(rightNeighbor) == null && tilemap.GetTile(bottomNeighbor) == null && tilemap.GetTile(diagonalNeighbor) == null)
            neighbors.Add(diagonalNeighbor);

        diagonalNeighbor = tilemap.WorldToCell(new Vector3(worldtile.x - tilemap.cellSize.x, worldtile.y - tilemap.cellSize.y, worldtile.z));
        if (tilemap.GetTile(leftNeighbor) == null && tilemap.GetTile(bottomNeighbor) == null &&  tilemap.GetTile(diagonalNeighbor) == null)
            neighbors.Add(diagonalNeighbor);

        diagonalNeighbor = tilemap.WorldToCell(new Vector3(worldtile.x - tilemap.cellSize.x, worldtile.y + tilemap.cellSize.y, worldtile.z));
        if (tilemap.GetTile(leftNeighbor) == null && tilemap.GetTile(topNeighbor) == null && tilemap.GetTile(diagonalNeighbor) == null)
            neighbors.Add(diagonalNeighbor);

        diagonalNeighbor = tilemap.WorldToCell(new Vector3(worldtile.x + tilemap.cellSize.x, worldtile.y + tilemap.cellSize.y, worldtile.z));
        if (tilemap.GetTile(rightNeighbor) == null && tilemap.GetTile(topNeighbor) == null && tilemap.GetTile(diagonalNeighbor) == null)
            neighbors.Add(diagonalNeighbor);

        return neighbors;
    }

    public List<Vector3Int> FindPath(Tilemap tilemap, Vector3Int start, Vector3Int goal)
    {
        List<Vector3Int> closedSet = new List<Vector3Int>();
        SimplePriorityQueue <Vector3Int> openSet = new SimplePriorityQueue<Vector3Int>();
        openSet.Enqueue(start, Heuristic(start, goal));

        Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();
        Dictionary<Vector3Int, float> gScore = new Dictionary<Vector3Int, float>();
        for (int n = tilemap.cellBounds.xMin; n < tilemap.cellBounds.xMax; n++)
        {
            for (int p = tilemap.cellBounds.yMin; p < tilemap.cellBounds.yMax; p++)
            {
                Vector3Int localPlace = new Vector3Int(n, p, (int)tilemap.transform.position.y);
                gScore.Add(localPlace, Mathf.Infinity);
            }
        }       
        gScore[start] = 0;
        while (openSet.Count > 0)
        {
            Vector3Int current = openSet.Dequeue();
            if (current == goal)
            {
                return ReconstructPath(cameFrom, current, start);
            }
            closedSet.Add(current);
            List<Vector3Int> neighbors = GetNeighbors(tilemap, current);
            foreach (Vector3Int neighbor in neighbors)
            {
                if (closedSet.Contains(neighbor))
                    continue;
                if (!openSet.Contains(neighbor))
                    openSet.Enqueue(neighbor, gScore[neighbor] + Heuristic(neighbor, goal));
                float tentative_score = gScore[current] + Heuristic(current, neighbor);
                if (tentative_score >= gScore[neighbor])
                    continue;
                cameFrom[neighbor] = current;
                gScore[neighbor] = tentative_score;
                openSet.UpdatePriority(neighbor, gScore[neighbor] + Heuristic(neighbor, goal));
            }
        }
        return new List<Vector3Int>();
    }
}
