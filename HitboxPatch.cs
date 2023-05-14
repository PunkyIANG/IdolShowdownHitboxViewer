﻿using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using IdolShowdown;
using UnityEngine;

namespace IdolShowdownHitboxViewer;

public class HitboxPatch
{
    private static
        Dictionary<int, (WeakReference<LineRenderer> outline, WeakReference<LineRenderer> infill)>
        lines = new();

    private static readonly Color outlineColor = new(1f, 0f, 0f, 1f);
    private static readonly Color infillColor = new(0.9098039f, 0f, 0f, 0.5f);

    [HarmonyPatch(typeof(Hitbox), "Awake")]
    [HarmonyPostfix]
    public static void InitHitboxLineRenderers(Hitbox __instance)
    {
        CleanupRemovedLines();

        var outline = LR.Get(4, outlineColor, 5, __instance.gameObject, loop: true);
        var infill = LR.Get(2, infillColor, 4, __instance.gameObject);

        lines.Add(__instance.GetInstanceID(),
            (new WeakReference<LineRenderer>(outline), new WeakReference<LineRenderer>(infill)));

        Console.WriteLine($"added hitbox with id {__instance.GetInstanceID()}");
        Console.WriteLine(lines.Select(x => x.Key.ToString()).Join());
    }

    [HarmonyPatch(typeof(Hitbox), "UpdateMe")]
    [HarmonyPostfix]
    public static void UpdateHitboxLineRenderers(Hitbox __instance, FixedBoxCollider ___fixedBox)
    {
        if (!lines.ContainsKey(__instance.GetInstanceID()))
            return;

        var (weakOutline, weakInfill) = lines[__instance.GetInstanceID()];

        if (!weakOutline.TryGetTarget(out var outline)
            || !weakInfill.TryGetTarget(out var infill))
        {
            Console.WriteLine("[DEBUGGING] hitbox is dead");
            return;
        }


        var pos = ___fixedBox.Position().ToVector3();
        var centerToCorner = ___fixedBox.WidthAndHeight.ToVector3() / 2f;
        var lineWidth = Camera.main.PixelSize();
        var realCenterToCorner = centerToCorner - lineWidth / 2f;

        outline.SetPositions(new[]
        {
            pos + new Vector3(realCenterToCorner.x, realCenterToCorner.y),
            pos + new Vector3(-realCenterToCorner.x, realCenterToCorner.y),
            pos + new Vector3(-realCenterToCorner.x, -realCenterToCorner.y),
            pos + new Vector3(realCenterToCorner.x, -realCenterToCorner.y),
        });
        outline.widthMultiplier = lineWidth.x;

        infill.SetPositions(new[]
        {
            pos - new Vector3(realCenterToCorner.x, 0f),
            pos + new Vector3(realCenterToCorner.x, 0f)
        });
        infill.widthMultiplier = 2f * realCenterToCorner.y;
    }

    private static void CleanupRemovedLines()
    {
        lines = lines
            .Where(x => x.Value.infill.TryGetTarget(out _) && x.Value.outline.TryGetTarget(out _))
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }
}