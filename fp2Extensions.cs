using Unity.Mathematics.FixedPoint;
using UnityEngine;

namespace IdolShowdownHitboxViewer;

public static class fp2Extensions
{
    public static Vector2 ToVector2(this fp2 vector)
    {
        return new Vector2((float)vector.x, (float)vector.y);
    }
    
    public static Vector3 ToVector3(this fp2 vector)
    {
        return new Vector3((float)vector.x, (float)vector.y);
    }
}