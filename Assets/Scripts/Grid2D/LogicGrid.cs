using System.Collections.Generic;
using UnityEngine;
using System;

public class LogicGrid
{
    private CollisionGrid<LogicComponent> logicGrid;
    private OverlapGrid<LogicConnection> connections;

    public LogicGrid(int width, int height)
    {
        this.logicGrid = new CollisionGrid<LogicComponent>(width, height);
        this.connections = new OverlapGrid<LogicConnection>(width, height);
    }

    public void UpdateGrid()
    {
        foreach (LogicComponent component in logicGrid.GetGridComponents())
        {
            component.UpdateValue();
        }
        foreach (LogicComponent component in logicGrid.GetGridComponents())
        {
            component.TransmitOutputValue();
        }
    }

    public bool AddComponent(LogicComponent component)
    {
        if (logicGrid.canPlaceComponent(component))
        {
            logicGrid.AddComponent(component);

            GridComponentPlug[] disconnectedPlugs = this.RemoveCloseConnections(component);
            foreach (GridComponentPlug disconnectedPlug in disconnectedPlugs)
            {
                CreatePlugConnection(disconnectedPlug.LogicComponent, disconnectedPlug);
            }

            foreach (GridComponentPlug plug in component.GetOutgoingPlugs())
            {
                Debug.Log(plug);
                CreatePlugConnection(component, plug);
            }

            return true;
        }
        return false;
    }

    private void CreatePlugConnection(LogicComponent component, GridComponentPlug plug)
    {
        RectInt collisionRay = createConnectionRay(plug, component);
        LogicConnection connection = new LogicConnection(collisionRay, plug.GetBody().GetDirection());
        this.connections.AddComponent(connection);
        plug.SetConnection(connection);
        connection.OutputPlug = plug;
        bool result = tryConnectRayCollision(collisionRay, plug, component, connection);
    }

    private RectInt createConnectionRay(GridComponentPlug plug, LogicComponent component)
    {
        Vector2Int startingPoint = CardinalDirectionHelper.Move(plug.GetBody().GetDirection(), 1) + component.GetBody().GetArea().position;
        RectInt collisionRay = this.rayBoundByGrid(plug.GetBody().GetDirection(), startingPoint, component);
        LogicComponent closestLogicComponentCollision = null;

        foreach (LogicComponent logicComp in this.logicGrid.GetGridComponents())
        {
            if (logicComp.GetBody().GetArea().Overlaps(collisionRay))
            {
                if (closestLogicComponentCollision != null)
                {
                    if (this.isNewRectCloserThanCurrent(plug.GetBody().GetDirection(), closestLogicComponentCollision.GetBody().GetArea(), logicComp.GetBody().GetArea()))
                    {
                        closestLogicComponentCollision = logicComp;
                    }
                }
                else
                {
                    closestLogicComponentCollision = logicComp;
                }
                collisionRay = this.trimRay(plug.GetBody().GetDirection(), collisionRay, closestLogicComponentCollision.GetBody().GetArea());
            }
        }
        return collisionRay;
    }

    private bool tryConnectRayCollision(RectInt collisionRay, GridComponentPlug plug, LogicComponent component, LogicConnection connection)
    {
        Vector2Int moveDistance = CardinalDirectionHelper.Move(plug.GetBody().GetDirection(), this.getLongestRectSide(collisionRay) + 1);
        Vector2Int checkPos = component.GetBody().GetArea().position + plug.GetBody().GetArea().position + moveDistance;
        LogicComponent[] collisions = this.logicGrid.GetComponentsAt(checkPos);

        if (collisions.Length != 0)
        {
            LogicComponent logicComponentCollision = collisions[0];
            Vector2Int plugLocalPosition = checkPos - logicComponentCollision.GetBody().GetArea().position;

            GridComponentPlug incomingPlug = Array.Find(logicComponentCollision.GetIncomingPlugs(), (GridComponentPlug incomingPlug) =>
            {
                return incomingPlug.GetBody().GetArea().Contains(plugLocalPosition) &&
                CardinalDirectionHelper.Opposite(incomingPlug.GetBody().GetDirection()) == plug.GetBody().GetDirection();
            });

            if (incomingPlug != null)
            {
                incomingPlug.SetConnection(connection);
                connection.InputPlug = incomingPlug;
                return true;
            }
        }
        return false;
    }

    private int getLargestGridDimension()
    {
        if (this.GetWidth() > this.GetHeight())
        {
            return this.GetWidth();
        }
        return this.GetHeight();
    }

    private RectInt rayBoundByGrid(CardinalDirection direction, Vector2Int startingPoint, LogicComponent component)
    {
        switch (direction)
        {
            case (CardinalDirection.NORTH):
                return new RectInt(startingPoint.x, startingPoint.y, 1, this.GetHeight() - startingPoint.y);
            case (CardinalDirection.EAST):
                return new RectInt(startingPoint.x, startingPoint.y, this.GetWidth() - startingPoint.x, 1);
            case (CardinalDirection.SOUTH):
                return new RectInt(startingPoint.x, 0, 1, component.GetBody().GetArea().y);
            case (CardinalDirection.WEST):
                return new RectInt(0, startingPoint.y, component.GetBody().GetArea().x, 1);
            default:
                return new RectInt(0, 0, 0, 0);
        }
    }

    private bool isNewRectCloserThanCurrent(CardinalDirection rayDirection, RectInt currentRect, RectInt newRect)
    {
        switch (rayDirection)
        {
            case (CardinalDirection.NORTH):
                return newRect.yMin < currentRect.yMin;
            case (CardinalDirection.EAST):
                return newRect.xMin < currentRect.xMin;
            case (CardinalDirection.SOUTH):
                return newRect.yMax > currentRect.yMax;
            case (CardinalDirection.WEST):
                return newRect.xMax > currentRect.xMax;
            default:
                return false;
        }
    }

    private RectInt trimRay(CardinalDirection rayDirection, RectInt ray, RectInt collisionBody)
    {
        switch (rayDirection)
        {
            case (CardinalDirection.NORTH):
                return new RectInt(ray.x, ray.y, ray.width, collisionBody.y - ray.y);
            case (CardinalDirection.EAST):
                return new RectInt(ray.x, ray.y, collisionBody.x - ray.x, ray.height);
            case (CardinalDirection.SOUTH):
                return new RectInt(ray.x, collisionBody.yMax, ray.width, ray.yMax - collisionBody.yMax);
            case (CardinalDirection.WEST):
                return new RectInt(collisionBody.xMax, ray.y, ray.xMax - collisionBody.xMax, ray.height);
            default:
                return new RectInt(0, 0, 0, 0);
        }
    }

    public int getLongestRectSide(RectInt rect)
    {
        if (rect.width > rect.height)
        {
            return rect.width;
        }
        return rect.height;
    }

    private GridComponentPlug[] RemoveCloseConnections(LogicComponent component)
    {
        RectInt searchArea = component.GetBody().GetArea();
        searchArea.x -= 1;
        searchArea.y -= 1;
        searchArea.width += 2;
        searchArea.height += 2;

        LogicConnection[] foundConnections = connections.GetComponentsWithin(searchArea);
        List<GridComponentPlug> result = new List<GridComponentPlug>();
        foreach (LogicConnection connection in foundConnections)
        {
            if (connection.OutputPlug != null)
            {
                result.Add(connection.OutputPlug);
            }
            RemoveConnection(connection);

        }

        return result.ToArray();
    }

    public bool RemoveLogicComponent(LogicComponent component)
    {
        if (this.logicGrid.RemoveComponent(component))
        {
            foreach (GridComponentPlug plug in component.GetIncomingPlugs())
            {
                if (plug.GetConnection() != null)
                {
                    RemoveConnection(plug.GetConnection());
                }
            }
            foreach (GridComponentPlug plug in component.GetOutgoingPlugs())
            {
                if (plug.GetConnection() != null)
                {
                    RemoveConnection(plug.GetConnection());
                }
            }
            return true;
        }
        return false;
    }

    public bool RemoveConnection(LogicConnection connection)
    {
        if (this.connections.RemoveComponent(connection))
        {
            if (connection.InputPlug != null)
            {
                connection.InputPlug.SetConnection(null);
            }
            if (connection.OutputPlug != null)
            {
                connection.OutputPlug.SetConnection(null);
            }
            connection.InputPlug = null;
            connection.OutputPlug = null;
            return true;
        }
        return false;
    }

    public int GetWidth()
    {
        return this.logicGrid.GetWidth();
    }

    public int GetHeight()
    {
        return this.logicGrid.GetHeight();
    }

    public LogicComponent[] GetLogicComponents()
    {
        return this.logicGrid.GetGridComponents();
    }

    public LogicConnection[] GetLogicConnections()
    {
        return this.connections.GetGridComponents();
    }
}
