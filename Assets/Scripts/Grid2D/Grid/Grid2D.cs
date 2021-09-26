using UnityEngine;

public interface Grid2D<T> where T : GridComponent
{
    int GetWidth();
    int GetHeight();
    bool AddComponent(T component);
    T[] GetGridComponents();
    T[] GetComponentsAt(Vector2Int position);
    T[] GetComponentsWithin(RectInt area);
    bool RemoveComponent(T component);
}
