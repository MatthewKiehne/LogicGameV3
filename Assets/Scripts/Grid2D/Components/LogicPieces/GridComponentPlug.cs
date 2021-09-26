using UnityEngine;
using System;

public class GridComponentPlug : GridComponent
{
    public readonly ComponentPlugType PlugType;
    private readonly GridBody body;
    private LogicConnection connection;
    public readonly LogicComponent LogicComponent;
    private int value;
    private Action<int> onValueSet;

    public GridComponentPlug(int localX, int localY, CardinalDirection direction, ComponentPlugType plugType, LogicComponent logicComponent)
    {
        this.body = new GridBody(new RectInt(localX, localY, 1, 1), direction);
        this.PlugType = plugType;
        this.LogicComponent = logicComponent;
    }

    public GridBody GetBody()
    {
        return this.body;
    }

    public LogicConnection GetConnection()
    {
        return this.connection;
    }

    public void SetConnection(LogicConnection connection)
    {
        this.connection = connection;
    }

    public void SetValue(int value)
    {
        if (this.value != value)
        {
            this.value = value;
            if (this.onValueSet != null)
            {
                this.onValueSet.Invoke(this.value);
            }
        }
    }

    public int GetValue()
    {
        return this.value;
    }

    public void SubscribeOnValueChange(Action<int> action)
    {
        this.onValueSet += action;
    }

    public void UnsubscribeOnValueChange(Action<int> action)
    {
        this.onValueSet -= action;
    }

    public void ClearOnValueChange()
    {
        this.onValueSet = null;
    }


    // sets the current value of the plug equal to the next value
    public void transferValue()
    {
        if (this.PlugType == ComponentPlugType.OUTPUT && this.connection != null)
        {
            this.connection.transferValue(this.value);
        }
    }
}