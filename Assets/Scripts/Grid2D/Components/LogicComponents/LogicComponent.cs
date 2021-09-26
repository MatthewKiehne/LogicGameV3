using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LogicComponent : GridComponent
{
    private OverlapGrid<GridComponentPlug> plugGrid;
    private List<GridComponentPlug> incomingPlugs;
    private List<GridComponentPlug> outgoingPlugs;
    protected GridBody body;
    private bool flipped;
    public LogicComponentType GateType { get; }
    private LogicGate gate;
    private bool clearAfterUpdate;


    public LogicComponent(RectInt rect, CardinalDirection direction, bool flipped, LogicComponentType type, LogicGate gate, bool clearAfterUpdate)
    {
        this.plugGrid = new OverlapGrid<GridComponentPlug>(rect.width, rect.height);
        this.body = new GridBody(rect, direction);
        this.incomingPlugs = new List<GridComponentPlug>();
        this.outgoingPlugs = new List<GridComponentPlug>();
        this.flipped = flipped;
        this.GateType = type;
        this.gate = gate;
        this.clearAfterUpdate = clearAfterUpdate;
    }
    public void TransmitOutputValue()
    {
        foreach (GridComponentPlug plug in this.GetOutgoingPlugs())
        {
            plug.transferValue();
        }
    }

    public void UpdateValue()
    {
        if (this.gate != null)
        {
            this.gate.UpdateOutgoingPlugValues(this.GetIncomingPlugs(), this.GetOutgoingPlugs());
        }

        if (this.clearAfterUpdate)
        {
            foreach (GridComponentPlug plug in this.GetIncomingPlugs())
            {
                plug.SetValue(0);
            }
        }
    }

    public void AddPlug(Vector2Int localPosision, CardinalDirection direction, ComponentPlugType type)
    {
        GridComponentPlug plug = new GridComponentPlug(localPosision.x, localPosision.y, direction, type, this);
        if (type == ComponentPlugType.INPUT)
        {
            this.incomingPlugs.Add(plug);
        }
        else if (type == ComponentPlugType.OUTPUT)
        {
            this.outgoingPlugs.Add(plug);
        }
        else
        {
            throw new Exception("Type: " + type + " was not expected");
        }
        this.plugGrid.AddComponent(plug);
    }

    public GridBody GetBody()
    {
        return this.body;
    }
    public GridComponentPlug[] GetIncomingPlugs()
    {
        return this.incomingPlugs.ToArray();
    }
    public GridComponentPlug[] GetOutgoingPlugs()
    {
        return this.outgoingPlugs.ToArray();
    }
}
