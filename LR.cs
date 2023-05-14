using System;
using UnityEngine;

namespace IdolShowdownHitboxViewer;

public static class LR
{
    public static LineRenderer Get(int posCount, Color color, int sortOrder,
        GameObject parentGameObject, bool loop = false)
    {
        var childRenderer = new GameObject("Line Renderer");
        childRenderer.transform.SetParent(parentGameObject.transform);

        var lineRenderer = childRenderer.AddComponent<LineRenderer>();
        lineRenderer.positionCount = posCount;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"))
        {
            color = color
        };
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.sortingLayerID = SortingLayer.NameToID("Overlay");
        lineRenderer.sortingOrder = sortOrder;
        lineRenderer.loop = loop;

        return lineRenderer;
    }
}