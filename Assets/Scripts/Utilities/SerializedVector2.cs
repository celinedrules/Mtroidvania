using UnityEngine;

public class SerializableVector2
{
    public float _x;
    public float _y;
    
    public SerializableVector2(float x, float y)
    {
        _x = x;
        _y = y;
    }
    
    public SerializableVector2(Vector2 vector)
    {
        _x = vector.x;
        _y = vector.y;
    }

    public Vector2 ToVector() => new(_x, _y);
}