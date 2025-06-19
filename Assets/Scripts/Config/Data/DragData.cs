using UnityEngine;

[System.Serializable]
public class DragData
{
    public Cube Cube { get; set; }
    public Vector2 StartPosition { get; set; }
    public Vector2 EndPosition { get; set; }
    public Vector2 Position { get; set; }
    public bool IsValid { get; set; }
}
