﻿using OWML.Common;
using OWML.ModHelper;
using OWML.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CheatsMod.Utils
{
    public static class Fog
    {
        private static HashSet<FogWarpVolume> fogWarpVolumnes = new HashSet<FogWarpVolume>();
        private static HashSet<PlanetaryFogController> planetaryFogControllers = new HashSet<PlanetaryFogController>();
        private static HashSet<FogOverrideVolume> fogOverrideVolumes = new HashSet<FogOverrideVolume>();
        private static Dictionary<object, Color> originalColor = new Dictionary<object, Color>();

        private static void OnAwakeFogWarpVolume(ref FogWarpVolume __instance)
        {
            fogWarpVolumnes.Add(__instance);
            originalColor.Add(__instance, __instance.GetFogColor());
            __instance.SetValue("_fogColor", enabled ? originalColor[__instance] : Color.clear);
        }

        private static void OnDestroyFogWarpVolume(ref FogWarpVolume __instance)
        {
            fogWarpVolumnes.Remove(__instance);
            originalColor.Remove(__instance);
        }

        private static void OnAwakePlanetaryFogController(ref PlanetaryFogController __instance)
        {
            planetaryFogControllers.Add(__instance);
            originalColor.Add(__instance, __instance.fogTint);
            __instance.fogTint = enabled ? originalColor[__instance] : Color.clear;
        }

        private static void OnDestroyPlanetaryFogController(ref PlanetaryFogController __instance)
        {
            planetaryFogControllers.Remove(__instance);
            originalColor.Remove(__instance);
        }

        private static void OnAwakeFogOverrideVolume(ref FogOverrideVolume __instance)
        {
            fogOverrideVolumes.Add(__instance);
            originalColor.Add(__instance, __instance.tint);
            __instance.tint = enabled ? originalColor[__instance] : Color.clear;
        }

        private static void OnDestroyFogOverrideVolume(ref FogOverrideVolume __instance)
        {
            fogOverrideVolumes.Remove(__instance);
            originalColor.Remove(__instance);
        }


        public static void Start()
        {
            MainClass.HarmonyHelper.AddPostfix<FogWarpVolume>("Awake", typeof(Fog), "OnAwakeFogWarpVolume");
            MainClass.HarmonyHelper.AddPostfix<FogWarpVolume>("OnDestroy", typeof(Fog), "OnDestroyFogWarpVolume");
            MainClass.HarmonyHelper.AddPostfix<PlanetaryFogController>("OnEnable", typeof(Fog), "OnAwakePlanetaryFogController");
            MainClass.HarmonyHelper.AddPostfix<PlanetaryFogController>("OnDisable", typeof(Fog), "OnDestroyPlanetaryFogController");
            MainClass.HarmonyHelper.AddPostfix<FogOverrideVolume>("Awake", typeof(Fog), "OnAwakeFogOverrideVolume");
            MainClass.HarmonyHelper.AddPostfix<FogOverrideVolume>("OnDestroy", typeof(Fog), "OnDestroyFogOverrideVolume");
        }

        private static bool _enabled = true;
        public static bool enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
                foreach (FogWarpVolume volume in fogWarpVolumnes)
                {
                    volume.SetValue("_fogColor", value ? originalColor[volume] : Color.clear);
                }
                foreach (PlanetaryFogController volume in planetaryFogControllers)
                {
                    volume.fogTint = value ? originalColor[volume] : Color.clear;
                }
                foreach (FogOverrideVolume volume in fogOverrideVolumes)
                {
                    volume.tint = value ? originalColor[volume] : Color.clear;
                }
            }
        }
    }
}
