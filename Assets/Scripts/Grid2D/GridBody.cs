using UnityEngine;

public class GridBody
{

    private RectInt area;
    private CardinalDirection direction;

    public GridBody(RectInt area, CardinalDirection direction)
    {
        this.area = area;
        this.direction = direction;
    }
    public RectInt GetArea()
    {
        return this.area;
    }
    public void SetArea(RectInt area)
    {
        this.area = area;
    }
    public CardinalDirection GetDirection()
    {
        return this.direction;
    }
    public void SetDirection(CardinalDirection direction)
    {
        this.direction = direction;
    }
}
