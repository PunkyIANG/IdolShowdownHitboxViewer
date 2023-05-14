using UnityEngine;

namespace IdolShowdownHitboxViewer;

public static class CameraExtensions
{
    public static Vector3 PixelSize(this Camera camera)
    {
        return camera.ScreenToWorldPoint(Vector3.one) - camera.ScreenToWorldPoint(Vector3.zero);
    } 
}