using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LogicComponentFactory
{

    private Dictionary<LogicComponentType, Vector2Int> dimensions = new Dictionary<LogicComponentType, Vector2Int>();
    private Dictionary<LogicComponentType, LogicGate> logicGate = new Dictionary<LogicComponentType, LogicGate>();
    private Dictionary<LogicComponentType, bool> clearAfterUpdate = new Dictionary<LogicComponentType, bool>();
    private Dictionary<LogicComponentType, GridComponentPlug[]> defaultPlugs = new Dictionary<LogicComponentType, GridComponentPlug[]>();
    private Dictionary<LogicComponentType, Action<int>> onInputValueChange = new Dictionary<LogicComponentType, Action<int>>();

    public void RegisterLogicComponent(LogicComponentType type, Vector2Int dimension, LogicGate gate,
        bool clearAfterUpdate, GridComponentPlug[] defaultPlugs, Action<int> onInputValueChange)
    {
        this.dimensions.Add(type, dimension);
        this.logicGate.Add(type, gate);
        this.clearAfterUpdate.Add(type, clearAfterUpdate);
        this.defaultPlugs.Add(type, defaultPlugs);
        this.onInputValueChange.Add(type, onInputValueChange);
    }

    public LogicComponent CreateLogicComponent(LogicComponentType type, int x, int y, CardinalDirection direction, bool flipped)
    {
        LogicComponent logicComponent = this.CreateBaseLogicComponent(type, x, y, direction, flipped);
        this.AddPlugsToLogicComponent(logicComponent, direction, flipped);
        this.AddOnValueChange(logicComponent);
        return logicComponent;
    }

    private LogicComponent CreateBaseLogicComponent(LogicComponentType type, int x, int y, CardinalDirection direction, bool flipped)
    {
        Vector2Int defaultDimensions = this.dimensions[type];
        LogicGate gate = this.logicGate[type];
        RectInt logicComponentArea = new RectInt(x, y, defaultDimensions.x, defaultDimensions.y);
        bool clear = this.clearAfterUpdate[type];

        int clockwiseSteps = CardinalDirectionHelper.ClockwiseStepDifference(CardinalDirection.NORTH, direction);
        if (clockwiseSteps % 2 == 1)
        {
            int priorWidth = logicComponentArea.width;
            logicComponentArea.width = logicComponentArea.height;
            logicComponentArea.height = priorWidth;
        }

        return new LogicComponent(logicComponentArea, direction, flipped, type, gate, clear);
    }

    private void AddPlugsToLogicComponent(LogicComponent component, CardinalDirection direction, bool flipped)
    {
        GridComponentPlug[] placeholder = this.defaultPlugs[component.GateType];
        int clockwiseSteps = CardinalDirectionHelper.ClockwiseStepDifference(CardinalDirection.NORTH, direction);

        foreach (GridComponentPlug currentPlug in placeholder)
        {
            GridComponentPlug plug = this.DuplicatePlug(currentPlug, component);

            // flip plug
            if (flipped)
            {
                plug.GetBody().SetArea(Grid2DUtil.FlipOverVerticalAxis(plug.GetBody().GetArea(), component.GetBody().GetArea().width / 2f));
                if (plug.GetBody().GetDirection() == CardinalDirection.EAST || plug.GetBody().GetDirection() == CardinalDirection.WEST)
                {
                    plug.GetBody().SetDirection(CardinalDirectionHelper.Opposite(plug.GetBody().GetDirection()));
                }
            }

            // rotate plug
            plug.GetBody().SetDirection(CardinalDirectionHelper.Rotate(plug.GetBody().GetDirection(), clockwiseSteps));
            plug.GetBody().SetArea(Grid2DUtil.RotateClockwiseInSameQuad(plug.GetBody().GetArea(), clockwiseSteps));
            component.AddPlug(plug.GetBody().GetArea().position, plug.GetBody().GetDirection(), plug.PlugType);
        }
    }

    private void AddOnValueChange(LogicComponent comp)
    {
        Action<int> action = this.onInputValueChange[comp.GateType];
        if (action != null)
        {
            foreach (GridComponentPlug plug in comp.GetIncomingPlugs())
            {
                plug.SubscribeOnValueChange(action);
            }
        }
    }

    private GridComponentPlug DuplicatePlug(GridComponentPlug plug, LogicComponent comp)
    {
        return new GridComponentPlug(plug.GetBody().GetArea().x, plug.GetBody().GetArea().y, plug.GetBody().GetDirection(), plug.PlugType, comp);
    }
}
