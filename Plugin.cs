﻿using BepInEx;
using HarmonyLib;

namespace IdolShowdownHitboxViewer
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            Harmony.CreateAndPatchAll(typeof(HitboxPatch));
            Harmony.CreateAndPatchAll(typeof(HurtboxPatch));
        }
    }


}