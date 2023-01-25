using OWML.Common;
using OWML.ModHelper;
using OWML.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CheatsMod.CR
{

    public static class Inhabitants
    {
        private static bool _enabledAI = true;
        private static bool _enabledHostility = true;

        public static bool enabledAI { get => _enabledAI; set => _enabledAI = value; }
        public static bool enabledHostility { get => _enabledHostility; set => _enabledHostility = value; }

        public static void Start()
        {
            MainClass.HarmonyHelper.AddPrefix<WaitAction>("CalculateUtility", typeof(EnabledInhabitants), "updateGhosts");
            MainClass.HarmonyHelper.AddPrefix<ChaseAction>("CalculateUtility", typeof(HostileInhabitants), "updateGhosts");
            MainClass.HarmonyHelper.AddPrefix<HuntAction>("CalculateUtility", typeof(HostileInhabitants), "updateGhosts");
            MainClass.HarmonyHelper.AddPrefix<StalkAction>("CalculateUtility", typeof(HostileInhabitants), "updateGhosts");
            MainClass.HarmonyHelper.AddPrefix<GrabAction>("CalculateUtility", typeof(HostileInhabitants), "updateGhosts");
        }

        class EnabledInhabitants
        {
            private static bool updateGhosts(ref float __result)
            {
                if (Inhabitants.enabledAI)
                {
                    return true;
                }
                else
                {
                    __result = float.MaxValue;
                    return false;
                }
            }
        }

        class HostileInhabitants
        {
            private static bool updateGhosts(ref float __result)
            {
                if (Inhabitants.enabledHostility)
                {
                    return true;
                }
                else
                {
                    __result = float.MinValue;
                    return false;
                }
            }
        }
    }
}
