using UnityEngine;

public enum CardinalDirection { NORTH = 0, EAST = 1, SOUTH = 2, WEST = 3 }

public static class CardinalDirectionHelper
{

    public static CardinalDirection Rotate(CardinalDirection direction, int clockwiseNinetyDegreeTurns)
    {
        return (CardinalDirection)(((int)direction + clockwiseNinetyDegreeTurns) % 4);
    }

    public static Vector2Int DirectionToVector2Int(CardinalDirection direction)
    {
        switch (direction)
        {
            case (CardinalDirection.NORTH):
                return new Vector2Int(0, 1);
            case (CardinalDirection.EAST):
                return new Vector2Int(1, 0);
            case (CardinalDirection.SOUTH):
                return new Vector2Int(0, -1);
            case (CardinalDirection.WEST):
                return new Vector2Int(-1, 0);
            default:
                return new Vector2Int(0, 0);
        }
    }

    public static Vector2Int Move(CardinalDirection direction, int distance)
    {
        return DirectionToVector2Int(direction) * distance;
    }

    public static CardinalDirection Opposite(CardinalDirection direction)
    {
        switch (direction)
        {
            case (CardinalDirection.NORTH):
                return CardinalDirection.SOUTH;
            case (CardinalDirection.EAST):
                return CardinalDirection.WEST;
            case (CardinalDirection.SOUTH):
                return CardinalDirection.NORTH;
            case (CardinalDirection.WEST):
                return CardinalDirection.EAST;
            default:
                return (CardinalDirection)5;
        }
    }
    public static int ClockwiseStepDifference(CardinalDirection startingDirection, CardinalDirection endingDirection)
    {
        int steps = endingDirection - startingDirection;
        if (steps < 0)
        {
            steps += 4;
        }
        return steps;
    }
}