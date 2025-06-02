using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileNode 
{
    public Vector3Int cellPos;
    public Vector3 worldPos;
    public float moveCost;
    public bool walkable;

    public TileNode(Vector3Int cell, Vector3 world, float cost = 1f, bool walkable = true)
    {
        this.cellPos = cell;
        this.worldPos = world;
        this.moveCost = cost;
        this.walkable = walkable;
    }

    public override bool Equals(object obj)
    {
        return obj is TileNode node && cellPos.Equals(node.cellPos);
    }

    public override int GetHashCode()
    {
        return cellPos.GetHashCode();
    }
}
