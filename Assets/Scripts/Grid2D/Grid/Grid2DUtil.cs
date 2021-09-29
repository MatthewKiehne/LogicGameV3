using UnityEngine;

public static class Grid2DUtil
{

    public static RectInt FlipOverVerticalAxis(RectInt rect, float xPosition)
    {
        float closeToFlipLine = xPosition - rect.xMax;
        int newX = Mathf.RoundToInt(xPosition + closeToFlipLine);
        return new RectInt(newX, rect.y, rect.width, rect.height);
    }

    public static RectInt FlipOverHorizontalAxis(RectInt rect, float yPosition)
    {
        float closeToFlipLine = yPosition - rect.yMax;
        int newY = Mathf.RoundToInt(yPosition + closeToFlipLine);
        return new RectInt(rect.x, newY, rect.width, rect.height);
    }

    public static RectInt RotateClockwise(RectInt rect, Vector2 pointToRotateAround)
    {
        Vector2 pointToRotate = new Vector2(rect.xMax, rect.y);
        Vector2 localDifference = pointToRotate - pointToRotateAround;
        Vector2 localRotatedPoint = new Vector2(localDifference.y, -localDifference.x);
        Vector2 worldRotatedPoint = localRotatedPoint + pointToRotateAround;
        return new RectInt(Mathf.RoundToInt(worldRotatedPoint.x), Mathf.RoundToInt(worldRotatedPoint.y), rect.height, rect.width);
    }

    public static RectInt RotateClockwise(RectInt rect, Vector2 pointToRotateAround, int turns)
    {
        RectInt result = rect;
        for (int i = 0; i < turns; i++)
        {
            result = RotateClockwise(result, pointToRotateAround);
        }
        return result;
    }

    public static RectInt RotateClockwiseInSameQuad(RectInt rect, int turns)
    {
        RectInt result = rect;
        for (int i = 0; i < turns; i++)
        {
            RectInt rotatedPosition = RotateClockwise(result, Vector2.zero);
            rotatedPosition.y += result.xMax;
            result = rotatedPosition;
        }
        return result;
    }
}
