using Godot;
using System;

public class Node2DExtended : Node2D
{
    protected void DrawRectWithBorder(Vector2 position, Vector2 size, Color color, Color borderColor, int borderSize)
    {
        DrawRect(new Rect2(position, size), color);
        DrawRect(new Rect2(position, new Vector2(size.x, borderSize)), borderColor);
        DrawRect(new Rect2(position, new Vector2(borderSize, size.y)), borderColor);
        DrawRect(new Rect2(position+new Vector2(size.x-borderSize, 0), new Vector2(borderSize, size.y)), borderColor);
        DrawRect(new Rect2(position+new Vector2(0, size.y-borderSize), new Vector2(size.x, borderSize)), borderColor);
    }
}
