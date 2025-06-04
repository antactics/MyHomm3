using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinder : MonoBehaviour
{
    //public Tilemap tilemap;

    //private readonly Vector3Int[] directions = new Vector3Int[]
    //{
    //    // 이동가능한 4방향 상하좌우
    //    new Vector3Int(0,1,0), new Vector3Int(0, -1, 0), new Vector3Int(-1,0,0), new Vector3Int(1,0,0),
    //    // 대각 4방향 좌상, 우상, 좌하, 우하
    //    new Vector3Int(-1, 1, 0), new Vector3Int(1, 1, 0), new Vector3Int(-1, -1, 0), new Vector3Int(1 , -1 ,0)
    //};

    //public List<Vector3> FindPath(Vector3 startWorldPos, Vector3 endWorldPos)
    //{
    //    Vector3Int startCell = tilemap.WorldToCell(startWorldPos);
    //    Vector3Int endCell = tilemap.WorldToCell(endWorldPos);

    //    var openSet = new List<TileNode>();
    //    var closedSet = new HashSet<TileNode>();
    //    var cameFrom = new Dictionary<TileNode, TileNode>();
    //    var gScore = new Dictionary<TileNode, float>();

    //    TileNode startNode = new TileNode(startCell, tilemap.GetCellCenterWorld(startCell));
    //    TileNode endNode = new TileNode(endCell, tilemap.GetCellCenterWorld(endCell));

    //    openSet.Add(startNode);
    //    gScore[startNode] = 0;

    //    while (openSet.Count > 0)
    //    {
    //        TileNode current = openSet[0];
    //        foreach(var node in openSet)
    //        {
    //            if (gScore.ContainsKey(node) && gScore[node] < gScore[current])
    //                current = node;
    //        }

    //        if (current.Equals(endNode))
    //            return ReconstructPath(cameFrom, current);

    //        openSet.Remove(current);
    //        closedSet.Add(current);

    //        foreach(var dir in directions)
    //        {
    //            Vector3Int neighborCell = current.cellPos + dir;

    //            if (!tilemap.HasTile(neighborCell))
    //                continue;

    //            TileNode neighbor = new TileNode(neighborCell, tilemap.GetCellCenterWorld(neighborCell));
    //            if (closedSet.Contains(neighbor))
    //                continue;

    //            float tentativeG = gScore[current] + ((dir.x != 0 && dir.y != 0) ? 1.4f : 1f); // 대각이동 1.4배

    //            if(!gScore.ContainsKey(neighbor) || tentativeG < gScore[neighbor])
    //            {
    //                cameFrom[neighbor] = current;
    //                gScore[neighbor] = tentativeG;
    //                if (!openSet.Contains(neighbor))
    //                    openSet.Add(neighbor);
    //            }
    //        }
    //    }

    //    return null;
    //}

    //private List<Vector3> ReconstructPath(Dictionary<TileNode, TileNode> camefrom, TileNode current)
    //{
    //    List<Vector3> path = new List<Vector3> { current.worldPos };
    //    while(camefrom.ContainsKey(current))
    //    {
    //        current = camefrom[current];
    //        path.Insert(0, current.worldPos);
    //    }
    //    return path;
    //}


    public static Pathfinder Instance;

    public Tilemap tilemap;
    public LayerMask obstacleMask;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public List<Vector3> FindPath(Vector3 startWorldPos, Vector3 targetWorldPos)
    {
        Vector3Int startCell = tilemap.WorldToCell(startWorldPos);
        Vector3Int targetCell = tilemap.WorldToCell(targetWorldPos);

        List<Vector3Int> openList = new List<Vector3Int>();
        HashSet<Vector3Int> closedSet = new HashSet<Vector3Int>();
        Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();

        Dictionary<Vector3Int, int> gCost = new Dictionary<Vector3Int, int>();
        Dictionary<Vector3Int, int> fCost = new Dictionary<Vector3Int, int>();

        openList.Add(startCell);
        gCost[startCell] = 0;
        fCost[startCell] = GetDistance(startCell, targetCell);

        while (openList.Count > 0)
        {
            // fCost 가장 낮은 노드 찾기
            Vector3Int current = openList[0];
            foreach (var pos in openList)
            {
                if (fCost.ContainsKey(pos) && fCost[pos] < fCost[current])
                {
                    current = pos;
                }
            }

            if (current == targetCell)
            {
                return ReconstructPath(cameFrom, current);
            }

            openList.Remove(current);
            closedSet.Add(current);

            foreach (Vector3Int neighbor in GetNeighbors(current))
            {
                if (closedSet.Contains(neighbor)) continue;

                if (!IsWalkable(neighbor)) continue;

                int tentativeG = gCost[current] + GetDistance(current, neighbor);

                if (!openList.Contains(neighbor))
                    openList.Add(neighbor);
                else if (tentativeG >= gCost[neighbor])
                    continue;

                cameFrom[neighbor] = current;
                gCost[neighbor] = tentativeG;
                fCost[neighbor] = gCost[neighbor] + GetDistance(neighbor, targetCell);
            }
        }

        return null; // 경로 없음
    }

    private List<Vector3> ReconstructPath(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int current)
    {
        List<Vector3> path = new List<Vector3>();
        while (cameFrom.ContainsKey(current))
        {
            path.Add(tilemap.GetCellCenterWorld(current));
            current = cameFrom[current];
        }
        path.Reverse();
        return path;
    }

    private int GetDistance(Vector3Int a, Vector3Int b)
    {
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);
        int diagonal = Mathf.Min(dx, dy);
        int straight = Mathf.Abs(dx - dy);
        return diagonal * 14 + straight * 10; // 대각선 1.4배로
    }

    private List<Vector3Int> GetNeighbors(Vector3Int cell)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();
        Vector3Int[] directions = new Vector3Int[]
        {
            new Vector3Int(1, 0, 0), new Vector3Int(-1, 0, 0),
            new Vector3Int(0, 1, 0), new Vector3Int(0, -1, 0),
            new Vector3Int(1, 1, 0), new Vector3Int(-1, 1, 0),
            new Vector3Int(1, -1, 0), new Vector3Int(-1, -1, 0),
        };

        foreach (Vector3Int dir in directions)
        {
            Vector3Int neighbor = cell + dir;
            neighbors.Add(neighbor);
        }

        return neighbors;
    }

    public bool IsWalkable(Vector3Int cell)
    {
        Vector3 world = tilemap.GetCellCenterWorld(cell);
        float checkRadius = 0.4f; 
        Collider2D hit = Physics2D.OverlapCircle(world, checkRadius, obstacleMask);
        return hit == null;
    }
}
