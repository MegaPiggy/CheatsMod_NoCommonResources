using OWML.Common;
using OWML.ModHelper;
using OWML.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CheatsMod.CR
{
    public static class Anglerfish
    {
        public static HashSet<AnglerfishController> anglerfish = new HashSet<AnglerfishController>();

        private static bool? _enabledAI = null;
        private static bool _canStun = true;
        private static bool _canFeel = true;
        private static bool _canHear = true;
        private static bool _canSmell = false;
        private static bool _canSee = false;
        private static float? _overrideAcceleration = null;
        private static float? _overrideInvestigateSpeed = null;
        private static float? _overrideChaseSpeed = null;
        private static float? _overrideTurnSpeed = null;
        private static float? _overrideQuickTurnSpeed = null;
        private static float? _overrideMouthOpenDistance = null;
        private static float? _overridePursueDistance = null;
        private static float? _overrideEscapeDistance = null;

        private static float _visionX = 180f;
        private static float _visionY = 100f;
        private static float _visionYoffset = 45f;
        private static float _visionDistance = 200f;
        private static float _smellDistance = 500f;

        public static bool? enabledAI { get { return _enabledAI.GetValueOrDefault(true); } set { _enabledAI = value; updateAnglerfish(); } }
        public static bool canStun { get { return _canStun; } set { _canStun = value; } }
        public static bool canFeel { get { return _canFeel; } set { _canFeel = value; } }
        public static bool canHear { get { return _canHear; } set { _canHear = value; } }
        public static bool canSmell { get { return _canSmell; } set { _canSmell = value; } }
        public static bool canSee { get { return _canSee; } set { _canSee = value; } }
        public static float? acceleration { get { return _overrideAcceleration.GetValueOrDefault(getParameter("_acceleration", 2f)); } set { _overrideAcceleration = value; updateParameter("_acceleration", value, 2f); } }
        public static float? investigateSpeed { get { return _overrideInvestigateSpeed.GetValueOrDefault(getParameter("_investigateSpeed", 20f)); } set { _overrideInvestigateSpeed = value; updateParameter("_investigateSpeed", value, 20f); } }
        public static float? chaseSpeed { get { return _overrideChaseSpeed.GetValueOrDefault(getParameter("_chaseSpeed", 42f)); } set { _overrideChaseSpeed = value; updateParameter("_chaseSpeed", value, 42f); } }
        public static float? turnSpeed { get { return _overrideTurnSpeed.GetValueOrDefault(getParameter("_turnSpeed", 180f)); } set { _overrideTurnSpeed = value; updateParameter("_turnSpeed", value, 180f); } }
        public static float? quickTurnSpeed { get { return _overrideQuickTurnSpeed.GetValueOrDefault(getParameter("_quickTurnSpeed", 360f)); } set { _overrideQuickTurnSpeed = value; updateParameter("_quickTurnSpeed", value, 360f); } }
        public static float? mouthOpenDistance { get { return _overrideMouthOpenDistance.GetValueOrDefault(getParameter("_arrivalDistance", 100f)); } set { _overrideMouthOpenDistance = value; updateParameter("_arrivalDistance", value, 100f); } }
        public static float? pursueDistance { get { return _overridePursueDistance.GetValueOrDefault(getParameter("_pursueDistance", 200f)); } set { _overridePursueDistance = value; updateParameter("_pursueDistance", value, 200f); } }
        public static float? escapeDistance { get { return _overrideEscapeDistance.GetValueOrDefault(getParameter("_escapeDistance", 500f)); } set { _overrideEscapeDistance = value; updateParameter("_escapeDistance", value, 500f); } }

        public static float? visionX { get { return _visionX; } set { _visionX = value.GetValueOrDefault(180f); } }
        public static float? visionY { get { return _visionY; } set { _visionY = value.GetValueOrDefault(100f); } }
        public static float? visionYoffset { get { return _visionYoffset; } set { _visionYoffset = value.GetValueOrDefault(45f); } }
        public static float? visionDistance { get { return _visionDistance; } set { _visionDistance = value.GetValueOrDefault(200f); } }
        public static float? smellDistance { get { return _smellDistance; } set { _smellDistance = value.GetValueOrDefault(500f); } }

        public static void Start()
        {
            MainClass.HarmonyHelper.AddPrefix<AnglerfishController>("Awake", typeof(Anglerfish), nameof(Anglerfish.Awake));
            MainClass.HarmonyHelper.AddPrefix<AnglerfishController>("OnDestroy", typeof(Anglerfish), nameof(Anglerfish.OnDestroy));
            MainClass.HarmonyHelper.AddPrefix<AnglerfishController>("OnImpact", typeof(Anglerfish), nameof(Anglerfish.onFeel));
            MainClass.HarmonyHelper.AddPrefix<AnglerfishController>("OnClosestAudibleNoise", typeof(Anglerfish), nameof(Anglerfish.onHearSound));
        }

        private static bool Awake(ref AnglerfishController __instance)
        {
            anglerfish.Add(__instance);
            updateAnglerfish(__instance);
            return true;
        }

        private static bool OnDestroy(ref AnglerfishController __instance)
        {
            anglerfish.Remove(__instance);
            return true;
        }

        private static bool onFeel(ref ImpactData impact)
        {
            if (!canFeel)
            {
                var attachedOwRigidbody = impact.otherCollider.GetAttachedOWRigidbody();
                if ((attachedOwRigidbody.CompareTag("Player") || attachedOwRigidbody.CompareTag("Ship")))
                {
                    return false;
                }
            }
            return true;
        }

        private static bool onHearSound(ref NoiseMaker noiseMaker)
        {
            return canHear;
        }

        public static void Update()
        {
            foreach (AnglerfishController anglerfishController in anglerfish)
            {
                updateAnglerfish(anglerfishController);
                if (!canStun)
                {
                    updateParameter(anglerfishController, "_stunTimer", 0f);
                }

                if (anglerfishController.isActiveAndEnabled)
                {
                    if (canSmell && (anglerfishController.GetAnglerState() == AnglerfishController.AnglerState.Investigating || anglerfishController.GetAnglerState() == AnglerfishController.AnglerState.Lurking))
                    {
                        var distance = Locator.GetPlayerBody().GetPosition() - anglerfishController.transform.position;
                        var bramble = getParameter<OWRigidbody>(anglerfishController, "_brambleBody");

                        if (distance.magnitude < _smellDistance)
                        {
                            anglerfishController.SetValue("_localDisturbancePos", bramble.transform.InverseTransformPoint(getPlayerBody().GetPosition()));
                            anglerfishController.Invoke("ChangeState", AnglerfishController.AnglerState.Investigating);
                        }
                    }
                    if (canSee && (anglerfishController.GetAnglerState() == AnglerfishController.AnglerState.Investigating || anglerfishController.GetAnglerState() == AnglerfishController.AnglerState.Lurking))
                    {
                        var distance = Locator.GetPlayerBody().GetPosition() - anglerfishController.transform.position;
                        var xAngle = Vector3.Angle(distance, anglerfishController.transform.forward);
                        var yAngle = Vector3.Angle(distance, anglerfishController.transform.up) - _visionYoffset;

                        if (distance.magnitude <= _visionDistance && (xAngle * 2) <= _visionX && 0 <= yAngle && yAngle <= _visionY)
                        {
                            anglerfishController.SetValue("_targetBody", getPlayerBody());
                            anglerfishController.Invoke("ChangeState", AnglerfishController.AnglerState.Chasing);
                        }
                    }
                }
            }
        }

        private static void updateAnglerfish()
        {
            foreach (AnglerfishController anglerfishController in anglerfish)
            {
                updateAnglerfish(anglerfishController);
            }
        }

        public static void updateAnglerfish(AnglerfishController anglerfishController)
        {
            if (enabledAI != null)
                anglerfishController.enabled = enabledAI.Value;
            updateParameter(anglerfishController, "_acceleration", _overrideAcceleration);
            updateParameter(anglerfishController, "_investigateSpeed", _overrideInvestigateSpeed);
            updateParameter(anglerfishController, "_chaseSpeed", _overrideChaseSpeed);
            updateParameter(anglerfishController, "_turnSpeed", _overrideTurnSpeed);
            updateParameter(anglerfishController, "_quickTurnSpeed", _overrideQuickTurnSpeed);
            updateParameter(anglerfishController, "_arrivalDistance", _overrideMouthOpenDistance);
            updateParameter(anglerfishController, "_pursueDistance", _overridePursueDistance);
            updateParameter(anglerfishController, "_escapeDistance", _overrideEscapeDistance);
        }

        private static ShuttleBody getShuttleBody()
        {
            foreach (NomaiShuttleController.ShuttleID shuttleID in EnumUtils.GetValues<NomaiShuttleController.ShuttleID>())
            {
                var shuttle = Locator.GetNomaiShuttle(shuttleID);
                if (shuttle != null && shuttle._isPlayerInside) return shuttle._shuttleBody as ShuttleBody;
            }
            return null;
        }

        private static OWRigidbody getPlayerBody()
        {
            if (PlayerState.IsInsideShuttle())
            {
                return getShuttleBody();
            }
            else if (PlayerState.IsInsideShip())
            {
                return Locator.GetShipBody();
            }
            else
            {
                return Locator.GetPlayerBody();
            }
        }

        private static void updateParameter(string id, float? parameter, float defaultValue)
        {
            foreach (AnglerfishController anglerfishController in anglerfish)
            {
                anglerfishController.SetValue(id, parameter.GetValueOrDefault(defaultValue));
            }
        }

        private static void updateParameter(AnglerfishController anglerfishController, string id, float? parameter)
        {
            if (parameter != null)
            {
                anglerfishController.SetValue(id, parameter.Value);
            }
        }

        private static T getParameter<T>(string id, T defaultValue)
        {
            foreach (AnglerfishController anglerfishController in anglerfish)
            {
                return getParameter<T>(anglerfishController, id);
            }
            return defaultValue;
        }

        private static T getParameter<T>(AnglerfishController anglerfishController, string id)
        {
            return anglerfishController.GetValue<T>(id);
        }
    }
}
