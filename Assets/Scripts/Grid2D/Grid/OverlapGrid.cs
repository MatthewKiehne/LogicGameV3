using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OverlapGrid<T> : Grid2D<T> where T : GridComponent
{

    private int width;
    private int height;
    private List<T> gridComponents = new List<T>();

    public OverlapGrid(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public int GetWidth()
    {
        return this.width;
    }
    public int GetHeight()
    {
        return this.height;
    }
    public bool AddComponent(T component)
    {
        if (this.canPlaceComponent(component))
        {
            this.gridComponents.Add(component);
            return true;
        }
        return false;
    }

    public bool canPlaceComponent(T component)
    {

        RectInt compArea = component.GetBody().GetArea();
        if (compArea.x >= 0 && compArea.x + compArea.width <= this.GetWidth() && compArea.y >= 0 && compArea.y + compArea.height <= this.GetHeight())
        {
            return true;
        }
        return false;
    }

    public T[] GetComponentsAt(Vector2Int position)
    {
        return this.gridComponents.FindAll((T comp) =>
        {
            return comp.GetBody().GetArea().Contains(position);
        }).ToArray();
    }

    public T[] GetGridComponents()
    {
        return this.gridComponents.ToArray();
    }

    public T[] GetComponentsWithin(RectInt area)
    {
        return this.gridComponents.FindAll((T comp) =>
       {
           return comp.GetBody().GetArea().Overlaps(area);
       }).ToArray();
    }

    public bool RemoveComponent(T component)
    {
        return this.gridComponents.Remove(component);
    }
}
