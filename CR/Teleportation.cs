using OWML.Common;
using OWML.ModHelper;
using OWML.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CheatsMod.CR
{

    public static class Teleportation
    {
        public static void teleportPlayerToSun()
        {
            var parent = Position.getBody(HeavenlyBodies.Sun);
            if (Locator.GetPlayerBody() && parent)
            {
                ignoreSand(false);
                teleportPlayerTo(parent, new Vector3(0, 5000f, 0), Vector3.zero, Vector3.zero, Vector3.zero, Quaternion.identity);
            }
        }

        public static void teleportPlayerToSunStation()
        {
            var parent = Position.getBody(HeavenlyBodies.SunStation);
            if (Locator.GetPlayerBody() && parent)
            {
                ignoreSand(false);
                teleportPlayerTo(parent, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Quaternion.identity);
            }
        }

        public static void teleportPlayerToEmberTwin()
        {
            var parent = Position.getBody(HeavenlyBodies.EmberTwin);
            if (Locator.GetPlayerBody() && parent)
            {
                ignoreSand(false);
                teleportPlayerTo(parent, new Vector3(0, 165f, 0), Vector3.zero, Vector3.zero, Vector3.zero, Quaternion.identity);
            }
        }

        public static void teleportPlayerToAshTwin()
        {
            var parent = Position.getBody(HeavenlyBodies.AshTwin);
            if (Locator.GetPlayerBody() && parent)
            {
                ignoreSand(false);
                teleportPlayerTo(parent, new Vector3(0, 180f, 0), Vector3.zero, Vector3.zero, Vector3.zero, Quaternion.identity);
            }
        }

        public static void teleportPlayerToAshTwinProject()
        {
            var planet = Position.getBody(HeavenlyBodies.AshTwin);
            if (Locator.GetPlayerBody() && planet)
            {
                var platform = Locator.GetWarpReceiver(NomaiWarpPlatform.Frequency.TimeLoop).GetPlatformCenter();
                var localPosition = platform.position - planet.GetPosition();
                float ratio = 0f;
                if (!PlayerState.IsInsideShip() && !PlayerState.IsInsideShuttle())
                {
                    ratio = (localPosition.magnitude - 1.85f) / localPosition.magnitude;
                }
                else
                {
                    ratio = (localPosition.magnitude - 6f) / localPosition.magnitude;
                }
                var position = new Vector3(localPosition.x * ratio + planet.GetPosition().x, localPosition.y * ratio + planet.GetPosition().y, localPosition.z * ratio + planet.GetPosition().z);

                ignoreSand(true);
                teleportPlayerTo(position, planet.GetPointVelocity(position), Vector3.zero, planet.GetAcceleration(), platform.rotation);
                GlobalMessenger<OWRigidbody>.FireEvent("EnterTimeLoopCentral", Locator.GetPlayerBody());
            }
        }

        public static void teleportPlayerToTimberHearth()
        {
            var parent = Position.getBody(HeavenlyBodies.TimberHearth);
            if (Locator.GetPlayerBody() && parent)
            {
                ignoreSand(false);
                teleportPlayerTo(parent, new Vector3(0, 280f, 0), Vector3.zero, Vector3.zero, Vector3.zero, Quaternion.identity);
            }
        }

        public static void teleportPlayerToSkyShutterSatellite()
        {
            var parent = Position.getBody(HeavenlyBodies.SkyShutterSatellite);
            if (Locator.GetPlayerBody() && parent)
            {
                ignoreSand(false);
                if (!PlayerState.IsInsideShip() && !PlayerState.IsInsideShuttle())
                {
                    teleportPlayerTo(parent, new Vector3(0, 0, -1f), Vector3.zero, Vector3.zero, Vector3.zero, Quaternion.identity);
                }
                else
                {
                    teleportPlayerTo(parent, new Vector3(0, 0, -8f), Vector3.zero, Vector3.zero, Vector3.zero, Quaternion.identity);
                }

            }
        }

        public static void teleportPlayerToAttlerock()
        {
            var parent = Position.getBody(HeavenlyBodies.Attlerock);
            if (Locator.GetPlayerBody() && parent)
            {
                ignoreSand(false);
                teleportPlayerTo(parent, new Vector3(0f, 85f, 0f), Vector3.zero, Vector3.zero, Vector3.zero, Quaternion.identity);
            }
        }

        public static void teleportPlayerToBlackHoleForgeTeleporter()
        {
            var planet = Position.getBody(HeavenlyBodies.BrittleHollow);
            if (Locator.GetPlayerBody() && planet)
            {
                var platform = Locator.GetWarpReceiver(NomaiWarpPlatform.Frequency.BrittleHollowForge).GetPlatformCenter();
                ignoreSand(false);
                teleportPlayerTo(new Vector3(platform.position.x, platform.position.y - 2f, platform.position.z), planet.GetVelocity(), planet.GetAngularVelocity(), planet.GetAcceleration(), platform.rotation);
            }
        }

        public static void teleportPlayerToHollowsLantern()
        {
            var parent = Position.getBody(HeavenlyBodies.HollowsLantern);
            if (Locator.GetPlayerBody() && parent)
            {
                ignoreSand(false);
                if (!PlayerState.IsInsideShip() && !PlayerState.IsInsideShuttle())
                {
                    teleportPlayerTo(parent, new Vector3(30.3f, 92.8f, 34.2f), Vector3.zero, Vector3.zero, Vector3.zero, Quaternion.identity);
                }
                else
                {
                    teleportPlayerTo(parent, new Vector3(27.9f, 98.6f, 34.7f), Vector3.zero, Vector3.zero, Vector3.zero, Quaternion.identity);
                }
            }
        }

        public static void teleportPlayerToGiantsDeep()
        {
            var parent = Position.getBody(HeavenlyBodies.GiantsDeep);
            if (Locator.GetPlayerBody() && parent)
            {
                ignoreSand(false);
                if (!PlayerState.IsInsideShip() && !PlayerState.IsInsideShuttle())
                {
                    teleportPlayerTo(parent, new Vector3(0f, 505f, 0f), Vector3.zero, Vector3.zero, Vector3.zero, Quaternion.identity);
                }
                else
                {
                    teleportPlayerTo(parent, new Vector3(0f, 520f, 0f), Vector3.zero, Vector3.zero, Vector3.zero, Quaternion.identity);
                }
            }
        }

        public static void teleportPlayerToProbeCannon()
        {
            var parent = Position.getBody(HeavenlyBodies.ProbeCannon);
            if (Locator.GetPlayerBody() && parent)
            {
                ignoreSand(false);
                teleportPlayerTo(parent, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Quaternion.identity);
            }
        }

        public static void teleportPlayerToProbeCannonCommandModule()
        {
            var parent = Position.getBody(HeavenlyBodies.GiantsDeep);
            if (Locator.GetPlayerBody() && parent)
            {
                ignoreSand(false);
                GlobalMessenger.FireEvent("PlayerEnterGiantsDeep");
                teleportPlayerTo(parent, new Vector3(-14.5f, -76.0f, -16.0f), Vector3.zero, Vector3.zero, Vector3.zero, Quaternion.identity);
            }
        }

        public static void teleportPlayerToDarkBramble()
        {
            var parent = Position.getBody(HeavenlyBodies.DarkBramble);
            if (Locator.GetPlayerBody())
            {
                ignoreSand(false);
                teleportPlayerTo(parent, new Vector3(0f, 950f, 0f), Vector3.zero, Vector3.zero, Vector3.zero, Quaternion.identity);
            }
        }

        public static void teleportPlayerToVessel()
        {
            var parent = Position.getBody(HeavenlyBodies.InnerDarkBramble_Vessel);
            if (Locator.GetPlayerBody() && parent)
            {
                ignoreSand(false);
                Teleportation.teleportPlayerTo(parent, new Vector3(149.1f, 11.9f, -8.6f), Vector3.zero, Vector3.zero, Vector3.zero, Quaternion.identity);
                if (PlayerState.IsInsideShuttle())
                {
                    GlobalMessenger.FireEvent("WarpPlayer");
                    GlobalMessenger.FireEvent("EnterShuttle");
                }
                else if (PlayerState.IsInsideShip())
                {
                    GlobalMessenger.FireEvent("WarpPlayer");
                    GlobalMessenger.FireEvent("EnterShip");
                }
                else
                {
                    GlobalMessenger.FireEvent("WarpPlayer");
                }
            }
        }

        public static void teleportPlayerToShip()
        {
            if (Locator.GetPlayerBody() && Locator.GetShipBody() && !PlayerState.IsInsideShip() && !PlayerState.IsInsideShuttle())
            {
                ignoreSand(false);
                teleportObjectTo(Locator.GetPlayerBody(), Locator.GetShipBody(), Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Quaternion.identity);
            }
        }

        public static void teleportPlayerToHourglassShuttle()
        {
            var parent = Position.getBody(HeavenlyBodies.NomaiEmberTwinShuttle);
            if (Locator.GetPlayerBody() && parent && !PlayerState.IsInsideShip() && !PlayerState.IsInsideShuttle())
            {
                ignoreSand(false);
                teleportObjectTo(Locator.GetPlayerBody(), parent, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Quaternion.identity);
            }
        }

        public static void teleportPlayerToBrittleHollowShuttle()
        {
            var parent = Position.getBody(HeavenlyBodies.NomaiBrittleHollowShuttle);
            if (Locator.GetPlayerBody() && parent && !PlayerState.IsInsideShip() && !PlayerState.IsInsideShuttle())
            {
                ignoreSand(false);
                teleportObjectTo(Locator.GetPlayerBody(), parent, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Quaternion.identity);
            }
        }

        public static void teleportPlayerToProbe()
        {
            if (Locator.GetPlayerBody() && Locator.GetProbe())
            {
                ignoreSand(false);
                teleportPlayerTo(Locator.GetProbe().GetAttachedOWRigidbody(), Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Quaternion.identity);
            }
        }

        public static void teleportPlayerToNomaiProbe()
        {
            var parent = Position.getBody(HeavenlyBodies.NomaiProbe);
            if (Locator.GetPlayerBody() && parent)
            {
                ignoreSand(false);
                teleportPlayerTo(parent, new Vector3(0f, 0f, -25f), Vector3.zero, Vector3.zero, Vector3.zero, Quaternion.identity);
            }
        }

        public static void teleportShipToPlayer()
        {
            if (Locator.GetPlayerBody() && Locator.GetShipBody() && !PlayerState.IsInsideShip() && !PlayerState.IsInsideShuttle())
            {
                ignoreSand(false);
                teleportObjectTo(Locator.GetShipBody(), Locator.GetPlayerBody(), Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Quaternion.identity);
            }
        }

        public static void teleportHourglassShuttleToPlayer()
        {
            var parent = Position.getBody(HeavenlyBodies.NomaiEmberTwinShuttle);
            if (Locator.GetPlayerBody() && parent && !PlayerState.IsInsideShip() && !PlayerState.IsInsideShuttle())
            {
                ignoreSand(false);
                teleportObjectTo(parent, Locator.GetPlayerBody(), Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Quaternion.identity);
            }
        }

        public static void teleportBrittleHollowShuttleToPlayer()
        {
            var parent = Position.getBody(HeavenlyBodies.NomaiBrittleHollowShuttle);
            if (Locator.GetPlayerBody() && parent && !PlayerState.IsInsideShip() && !PlayerState.IsInsideShuttle())
            {
                ignoreSand(false);
                teleportObjectTo(parent, Locator.GetPlayerBody(), Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Quaternion.identity);
            }
        }

        public static void teleportPlayerToInterloper()
        {
            var parent = Position.getBody(HeavenlyBodies.Interloper);
            if (Locator.GetPlayerBody() && parent)
            {
                ignoreSand(false);
                teleportPlayerTo(parent, new Vector3(0f, 85f, 0f), Vector3.zero, Vector3.zero, Vector3.zero, Quaternion.identity);
            }
        }

        public static void teleportPlayerToWhiteHole()
        {
            var parent = Position.getBody(HeavenlyBodies.WhiteHole);
            if (Locator.GetPlayerBody() && parent)
            {
                ignoreSand(false);
                teleportPlayerTo(parent, new Vector3(0f, 0f, 40f), Vector3.zero, Vector3.zero, Vector3.zero, Quaternion.identity);
            }
        }

        public static void teleportPlayerToMappingSatellite()
        {
            var parent = Position.getBody(HeavenlyBodies.SatelliteMapping);
            if (Locator.GetPlayerBody() && parent)
            {
                ignoreSand(false);
                teleportPlayerTo(parent, new Vector3(0f, 0f, 40f), Vector3.zero, Vector3.zero, Vector3.zero, Quaternion.identity);
            }
        }

        public static void teleportPlayerToBackerSatellite()
        {
            var parent = Position.getBody(HeavenlyBodies.SatelliteBacker);
            if (Locator.GetPlayerBody() && parent)
            {
                ignoreSand(false);
                teleportPlayerTo(parent, new Vector3(0f, 0f, 40f), Vector3.zero, Vector3.zero, Vector3.zero, Quaternion.identity);
            }
        }

        public static void teleportPlayerToWhiteHoleStation()
        {
            var parent = Position.getBody(HeavenlyBodies.WhiteHoleStation);
            if (Locator.GetPlayerBody() && parent)
            {
                ignoreSand(false);
                teleportPlayerTo(parent, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Quaternion.identity);
            }
        }

        public static void teleportPlayerToStranger()
        {
            var parent = Position.getBody(HeavenlyBodies.Stranger);
            if (Locator.GetPlayerBody() && parent)
            {
                ignoreSand(false);
                Teleportation.teleportPlayerTo(parent, new Vector3(45.5f, -169f, -290f), Vector3.zero, Vector3.zero, Vector3.zero, Quaternion.identity);
            }
        }

        private static DreamCampfire[] _validCampfires => new DreamCampfire[4]
        {
            Locator.GetDreamCampfire(DreamArrivalPoint.Location.Zone1),
            Locator.GetDreamCampfire(DreamArrivalPoint.Location.Zone2),
            Locator.GetDreamCampfire(DreamArrivalPoint.Location.Zone3),
            Locator.GetDreamCampfire(DreamArrivalPoint.Location.Zone4),
        };

        private static DreamCampfire GetClosestDreamCampfire()
        {
            DreamCampfire campfireMin = null;
            float minDist = Mathf.Infinity;
            Vector3 currentPos = Vector3.zero;
            foreach (DreamCampfire campfire in _validCampfires)
            {
                float dist = Vector3.Distance(campfire.transform.position, currentPos);
                if (dist < minDist)
                {
                    campfireMin = campfire;
                    minDist = dist;
                }
            }
            return campfireMin;
        }

        public static void enterDreamWorld()
        {
            DreamCampfire dreamCampfire = GetClosestDreamCampfire();
            if (dreamCampfire != null)
            {
                RelativeLocationData relativeLocation = new RelativeLocationData(Locator.GetPlayerBody(), dreamCampfire.GetAttachedOWRigidbody(), dreamCampfire.transform);
                DreamArrivalPoint.Location location = dreamCampfire.GetLocation();
                DreamArrivalPoint arrivalPoint = Locator.GetDreamArrivalPoint(location);
                if (Locator.GetToolModeSwapper().GetItemCarryTool().GetHeldItemType() != ItemType.DreamLantern)
                {
                    Possession.pickUpDreamLantern(DreamLanternType.Functioning, true);
                }
                Locator.GetDreamWorldController().EnterDreamWorld(dreamCampfire, arrivalPoint, relativeLocation);
            }
        }

        public static void teleportPlayerToDreamWorld()
        {
            var parent = Position.getBody(HeavenlyBodies.DreamWorld);
            var parent2 = Position.getBody(HeavenlyBodies.Stranger);
            if (Locator.GetPlayerBody() && parent && parent2)
            {
                ignoreSand(false);
                Teleportation.teleportPlayerTo(parent2, new Vector3(-21.5f, -199f, -240), Vector3.zero, Vector3.zero, Vector3.zero, Quaternion.identity);
                MainClass.Instance.ModHelper.Events.Unity.FireOnNextUpdate(enterDreamWorld);
            }
        }

        public static void teleportPlayerToQuantumMoon()
        {
            var parent = Position.getBody(HeavenlyBodies.QuantumMoon);
            if (Locator.GetPlayerBody() && parent)
            {
                ignoreSand(false);
                Teleportation.teleportPlayerTo(parent, new Vector3(0f, 80f, 0f), Vector3.zero, Vector3.zero, Vector3.zero, Quaternion.identity);
                if (PlayerState.IsInsideShuttle())
                {
                    GlobalMessenger.FireEvent("WarpPlayer");
                    GlobalMessenger.FireEvent("EnterShuttle");
                }
                else if (PlayerState.IsInsideShip())
                {
                    GlobalMessenger.FireEvent("WarpPlayer");
                    GlobalMessenger.FireEvent("EnterShip");
                }
                else
                {
                    GlobalMessenger.FireEvent("WarpPlayer");
                }
            }
        }

        public static void teleportShipToQuantumMoon()
        {
            OWRigidbody qm_rb = Locator.GetAstroObject(AstroObject.Name.QuantumMoon).GetComponent<OWRigidbody>();
            OWRigidbody s_rb = Locator.GetShipBody();

            Vector3 newPosition = qm_rb.transform.TransformPoint(new Vector3(0f, -80f, 0f));
            s_rb.SetPosition(newPosition);
            s_rb.SetRotation(Quaternion.LookRotation(qm_rb.transform.forward, -qm_rb.transform.up));
            s_rb.SetVelocity(qm_rb.GetPointVelocity(newPosition));
            s_rb.SetAngularVelocity(qm_rb.GetAngularVelocity());
        }

        private static void ignoreSand(bool ignore)
        {
            if (PlayerState.IsInsideShuttle())
            {
                foreach (SandLevelController sandLevelController in UnityEngine.Object.FindObjectsOfType<SandLevelController>())
                    foreach (Collider componentsInChild in getShuttleBody().GetComponentsInChildren<Collider>())
                        Physics.IgnoreCollision(sandLevelController.GetSandCollider(), componentsInChild, ignore);
            }
            if (PlayerState.IsInsideShip())
            {
                foreach (SandLevelController sandLevelController in UnityEngine.Object.FindObjectsOfType<SandLevelController>())
                    foreach (Collider componentsInChild in Locator.GetShipBody().GetComponentsInChildren<Collider>())
                        Physics.IgnoreCollision(sandLevelController.GetSandCollider(), componentsInChild, ignore);
            }
            GlobalMessenger<OWRigidbody>.FireEvent(ignore ? "EnterTimeLoopCentral" : "ExitTimeLoopCentral", Locator.GetPlayerBody());
            Locator.GetDreamWorldController().ExitDreamWorld(DeathType.Dream);
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

        public static void teleportPlayerTo(OWRigidbody teleportTo, Vector3 position, Vector3 velocity, Vector3 angularVelocity, Vector3 acceleration, Quaternion rotation)
        {
            MainClass.Console.WriteLine("Teleporting player to " + teleportTo.transform.root.name.Replace("_Body", ""));
            if (teleportTo != null)
            {
                teleportObjectTo(getPlayerBody(), teleportTo, position, velocity, angularVelocity, acceleration, rotation);
            }
        }

        public static void teleportPlayerTo(Vector3 position, Vector3 velocity, Vector3 angularVelocity, Vector3 acceleration, Quaternion rotation)
        {
            MainClass.Console.WriteLine("Teleporting player to " + position);
            teleportObjectTo(getPlayerBody(), position, velocity, angularVelocity, acceleration, rotation);
        }

        public static void teleportObjectTo(OWRigidbody teleportObject, OWRigidbody teleportTo, Vector3 position, Vector3 velocity, Vector3 angularVelocity, Vector3 acceleration, Quaternion rotation)
        {
            if (teleportTo)
            {
                var newPosition = teleportTo.transform.TransformPoint(position);
                var newVelocity = velocity + teleportTo.GetPointVelocity(newPosition);
                var newAnglarVelocity = angularVelocity + teleportTo.GetAngularVelocity();
                var newAcceleration = acceleration + teleportTo.GetAcceleration();
                var newRotation = rotation * teleportTo.GetRotation();
                teleportObjectTo(teleportObject, newPosition, newVelocity, newAnglarVelocity, newAcceleration, newRotation);
            }
        }

        public static void teleportObjectTo(OWRigidbody teleportObject, Vector3 position, Vector3 velocity, Vector3 angularVelocity, Vector3 acceleration, Quaternion rotation)
        {
            teleportObject.SetPosition(new Vector3(position.x, position.y, position.z));
            teleportObject.SetVelocity(new Vector3(velocity.x, velocity.y, velocity.z));
            teleportObject.SetRotation(new Quaternion(rotation.x, rotation.y, rotation.z, rotation.w));
            teleportObject.SetAngularVelocity(new Vector3(angularVelocity.x, angularVelocity.y, angularVelocity.z));

            teleportObject._lastPosition = new Vector3(position.x, position.y, position.z);
            teleportObject._currentVelocity = new Vector3(velocity.x, velocity.y, velocity.z);
            teleportObject._lastVelocity = new Vector3(velocity.x, velocity.y, velocity.z);
            teleportObject._currentAngularVelocity = new Vector3(angularVelocity.x, angularVelocity.y, angularVelocity.z);
            teleportObject._lastAngularVelocity = new Vector3(angularVelocity.x, angularVelocity.y, angularVelocity.z);
            teleportObject._currentAccel = new Vector3(acceleration.x, acceleration.y, acceleration.z);
            teleportObject._lastAccel = new Vector3(acceleration.x, acceleration.y, acceleration.z);
        }
    }
}
