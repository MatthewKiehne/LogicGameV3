using UnityEngine;

public class LogicConnection : GridComponent
{

    private GridBody body;

    public GridComponentPlug InputPlug { get; set; }
    public GridComponentPlug OutputPlug { get; set; }

    public LogicConnection(RectInt rect, CardinalDirection direction)
    {
        this.body = new GridBody(rect, direction);
    }

    public void transferValue(int value)
    {
        if (this.InputPlug != null)
        {
            this.InputPlug.SetValue(value);
        }
    }

    public GridBody GetBody()
    {
        return this.body;
    }
}
