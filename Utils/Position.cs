using OWML.Common;
using OWML.ModHelper;
using OWML.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CheatsModNoCR.Utils
{
    public static class Position
    {
        public class Size : IEquatable<Size>
        {
            public float size { get; }
            public float influence { get; }

            public Size(float size, float influence)
            {
                this.size = size;
                this.influence = influence;
            }

            public override string ToString()
            {
                return $"({Math.Round(size, 4).ToString("G4")}, {Math.Round(influence, 4).ToString("G4")})";
            }

            public override bool Equals(System.Object other)
            {
                if (other != null && other is Size)
                {
                    return Equals((Size)(other as Size));
                }
                return false;
            }

            public bool Equals(Size other)
            {
                if (other != null)
                {
                    return size == other.size
                        && influence == other.influence;
                }
                return false;
            }

            public override int GetHashCode()
            {
                return (size.GetHashCode() * 4)
                    + (influence.GetHashCode() * 16);
            }
        }


        private static Dictionary<HeavenlyBody, Func<AstroObject>> _astroLookup = StandardAstroLookup;
        private static Dictionary<HeavenlyBody, Func<OWRigidbody>> _bodyLookup = StandardBodyLookup;
        private static Dictionary<HeavenlyBody, AstroObject> astros = new Dictionary<HeavenlyBody, AstroObject>();
        private static Dictionary<HeavenlyBody, OWRigidbody> bodies = new Dictionary<HeavenlyBody, OWRigidbody>();

        public static Dictionary<HeavenlyBody, Func<AstroObject>> StandardAstroLookup
        {
            get
            {
                var standardAstroLookup = new Dictionary<HeavenlyBody, Func<AstroObject>>();
                var scene = LoadManager.GetCurrentScene();
                MainClass.Console.WriteLine("StandardAstroLookup " + scene);
                if (scene == OWScene.SolarSystem)
                {
                    standardAstroLookup.Add(HeavenlyBodies.Sun, () => Locator.GetAstroObject(AstroObject.Name.Sun));
                    standardAstroLookup.Add(HeavenlyBodies.SunStation, () => Locator.GetMinorAstroObject("Sun Station"));
                    standardAstroLookup.Add(HeavenlyBodies.HourglassTwins, () => Locator.GetAstroObject(AstroObject.Name.TowerTwin)?.GetPrimaryBody());
                    standardAstroLookup.Add(HeavenlyBodies.AshTwin, () => Locator.GetAstroObject(AstroObject.Name.TowerTwin));
                    standardAstroLookup.Add(HeavenlyBodies.EmberTwin, () => Locator.GetAstroObject(AstroObject.Name.CaveTwin));
                    standardAstroLookup.Add(HeavenlyBodies.TimberHearth, () => Locator.GetAstroObject(AstroObject.Name.TimberHearth));
                    standardAstroLookup.Add(HeavenlyBodies.SkyShutterSatellite, () => Locator.GetAstroObject(AstroObject.Name.TimberHearth)?.GetSatellite());
                    standardAstroLookup.Add(HeavenlyBodies.Attlerock, () => Locator.GetAstroObject(AstroObject.Name.TimberHearth)?.GetMoon());
                    standardAstroLookup.Add(HeavenlyBodies.BrittleHollow, () => Locator.GetAstroObject(AstroObject.Name.BrittleHollow));
                    standardAstroLookup.Add(HeavenlyBodies.HollowsLantern, () => Locator.GetAstroObject(AstroObject.Name.BrittleHollow)?.GetMoon());
                    standardAstroLookup.Add(HeavenlyBodies.GiantsDeep, () => Locator.GetAstroObject(AstroObject.Name.GiantsDeep));
                    standardAstroLookup.Add(HeavenlyBodies.ProbeCannon, () => Locator.GetAstroObject(AstroObject.Name.ProbeCannon));
                    standardAstroLookup.Add(HeavenlyBodies.DarkBramble, () => Locator.GetAstroObject(AstroObject.Name.DarkBramble));
                    standardAstroLookup.Add(HeavenlyBodies.InnerDarkBramble_Hub, () => Locator.GetMinorAstroObject("Hub Dimension"));
                    standardAstroLookup.Add(HeavenlyBodies.InnerDarkBramble_EscapePod, () => Locator.GetMinorAstroObject("Escape Pod Dimension"));
                    standardAstroLookup.Add(HeavenlyBodies.InnerDarkBramble_Nest, () => Locator.GetMinorAstroObject("Angler Nest Dimension"));
                    standardAstroLookup.Add(HeavenlyBodies.InnerDarkBramble_Feldspar, () => Locator.GetMinorAstroObject("Pioneer Dimension"));
                    standardAstroLookup.Add(HeavenlyBodies.InnerDarkBramble_Gutter, () => MainClass.getSector(Sector.Name.BrambleDimension, body => OuterFogWarpVolume.Name.ExitOnly == body?.GetComponentInChildren<OuterFogWarpVolume>()?.GetName()).GetAttachedOWRigidbody().GetComponent<AstroObject>());
                    standardAstroLookup.Add(HeavenlyBodies.InnerDarkBramble_Vessel, () => Locator.GetMinorAstroObject("Vessel Dimension"));
                    standardAstroLookup.Add(HeavenlyBodies.InnerDarkBramble_Maze, () => Locator.GetMinorAstroObject("Cluster Dimension"));
                    standardAstroLookup.Add(HeavenlyBodies.InnerDarkBramble_SmallNest, () => MainClass.getSector(Sector.Name.BrambleDimension, body => OuterFogWarpVolume.Name.SmallNest == body?.GetComponentInChildren<OuterFogWarpVolume>()?.GetName()).GetAttachedOWRigidbody().GetComponent<AstroObject>());
                    standardAstroLookup.Add(HeavenlyBodies.InnerDarkBramble_Secret, () => Locator.GetMinorAstroObject("Elsinore Dimension"));
                    standardAstroLookup.Add(HeavenlyBodies.Interloper, () => Locator.GetAstroObject(AstroObject.Name.Comet));
                    standardAstroLookup.Add(HeavenlyBodies.WhiteHole, () => Locator.GetAstroObject(AstroObject.Name.WhiteHole));
                    standardAstroLookup.Add(HeavenlyBodies.WhiteHoleStation, () => Locator.GetAstroObject(AstroObject.Name.WhiteHoleTarget));
                    standardAstroLookup.Add(HeavenlyBodies.Stranger, () => Locator.GetAstroObject(AstroObject.Name.RingWorld));
                    standardAstroLookup.Add(HeavenlyBodies.DreamWorld, () => Locator.GetAstroObject(AstroObject.Name.DreamWorld));
                    standardAstroLookup.Add(HeavenlyBodies.QuantumMoon, () => Locator.GetAstroObject(AstroObject.Name.QuantumMoon));
                    standardAstroLookup.Add(HeavenlyBodies.SatelliteBacker, () => Locator.GetMinorAstroObject("Backer's Satellite"));
                    standardAstroLookup.Add(HeavenlyBodies.SatelliteMapping, () => Locator.GetAstroObject(AstroObject.Name.MapSatellite));//MainClass.getSector(Sector.Name.Unnamed, body => "Sector_HearthianMapSatellite".Equals(body?.gameObject?.name))?.GetAttachedOWRigidbody()?.GetComponent<AstroObject>());
                    standardAstroLookup.Add(HeavenlyBodies.EyeOfTheUniverse, () => MainClass.getSector(Sector.Name.EyeOfTheUniverse)?.GetAttachedOWRigidbody()?.GetComponent<AstroObject>());
                    standardAstroLookup.Add(HeavenlyBodies.EyeOfTheUniverse_Vessel, () => MainClass.getSector(Sector.Name.Vessel)?.GetAttachedOWRigidbody()?.GetComponent<AstroObject>());
                }
                else
                {
                    standardAstroLookup.Add(HeavenlyBodies.EyeOfTheUniverse, () => MainClass.getSector(Sector.Name.EyeOfTheUniverse)?.GetAttachedOWRigidbody()?.GetComponent<AstroObject>());
                    standardAstroLookup.Add(HeavenlyBodies.EyeOfTheUniverse_Vessel, () => MainClass.getSector(Sector.Name.Vessel, body => Sector.Name.EyeOfTheUniverse == body.GetRootSector().GetName())?.GetAttachedOWRigidbody()?.GetComponent<AstroObject>());
                }

                return standardAstroLookup;
            }
        }

        public static Dictionary<HeavenlyBody, Func<OWRigidbody>> StandardBodyLookup
        {
            get
            {
                var standardBodyLookup = new Dictionary<HeavenlyBody, Func<OWRigidbody>>();
                var scene = LoadManager.GetCurrentScene();
                MainClass.Console.WriteLine("StandardBodyLookup " + scene);
                if (scene == OWScene.SolarSystem)
                {
                    standardBodyLookup.Add(HeavenlyBodies.Player, () => Locator.GetPlayerBody());
                    standardBodyLookup.Add(HeavenlyBodies.Ship, () => Locator.GetShipBody());
                    standardBodyLookup.Add(HeavenlyBodies.Probe, () => Locator.GetProbe()?.GetAttachedOWRigidbody());
                    standardBodyLookup.Add(HeavenlyBodies.ModelShip, () => GameObject.Find("ModelRocket_Body")?.GetAttachedOWRigidbody());
                    standardBodyLookup.Add(HeavenlyBodies.Sun, () => Locator.GetAstroObject(AstroObject.Name.Sun)?.GetAttachedOWRigidbody());
                    standardBodyLookup.Add(HeavenlyBodies.SunStation, () => Locator.GetAstroObject(AstroObject.Name.SunStation)?.GetAttachedOWRigidbody() ?? Locator.GetWarpReceiver(NomaiWarpPlatform.Frequency.SunStation)?.GetAttachedOWRigidbody());
                    standardBodyLookup.Add(HeavenlyBodies.HourglassTwins, () => Locator.GetAstroObject(AstroObject.Name.HourglassTwins)?.GetAttachedOWRigidbody() ?? Locator.GetAstroObject(AstroObject.Name.TowerTwin)?.GetPrimaryBody()?.GetAttachedOWRigidbody());
                    standardBodyLookup.Add(HeavenlyBodies.AshTwin, () => Locator.GetAstroObject(AstroObject.Name.TowerTwin)?.GetAttachedOWRigidbody());
                    standardBodyLookup.Add(HeavenlyBodies.EmberTwin, () => Locator.GetAstroObject(AstroObject.Name.CaveTwin)?.GetAttachedOWRigidbody());
                    standardBodyLookup.Add(HeavenlyBodies.TimberHearth, () => Locator.GetAstroObject(AstroObject.Name.TimberHearth)?.GetAttachedOWRigidbody());
                    standardBodyLookup.Add(HeavenlyBodies.SkyShutterSatellite, () => Locator.GetAstroObject(AstroObject.Name.TimberHearth)?.GetSatellite()?.GetAttachedOWRigidbody());
                    standardBodyLookup.Add(HeavenlyBodies.Attlerock, () => Locator.GetAstroObject(AstroObject.Name.TimberMoon)?.GetAttachedOWRigidbody() ?? Locator.GetAstroObject(AstroObject.Name.TimberHearth)?.GetMoon()?.GetAttachedOWRigidbody());
                    standardBodyLookup.Add(HeavenlyBodies.BrittleHollow, () => Locator.GetAstroObject(AstroObject.Name.BrittleHollow)?.GetAttachedOWRigidbody());
                    standardBodyLookup.Add(HeavenlyBodies.HollowsLantern, () => Locator.GetAstroObject(AstroObject.Name.VolcanicMoon)?.GetAttachedOWRigidbody() ?? Locator.GetAstroObject(AstroObject.Name.BrittleHollow)?.GetMoon()?.GetAttachedOWRigidbody());
                    standardBodyLookup.Add(HeavenlyBodies.GiantsDeep, () => Locator.GetAstroObject(AstroObject.Name.GiantsDeep)?.GetAttachedOWRigidbody());
                    standardBodyLookup.Add(HeavenlyBodies.ProbeCannon, () => Locator.GetAstroObject(AstroObject.Name.ProbeCannon)?.GetAttachedOWRigidbody());
                    standardBodyLookup.Add(HeavenlyBodies.NomaiProbe, () => Locator.GetAstroObject(AstroObject.Name.ProbeCannon)?.GetComponent<OrbitalProbeLaunchController>()?._probeBody);
                    standardBodyLookup.Add(HeavenlyBodies.NomaiEmberTwinShuttle, () => Locator.GetNomaiShuttle(NomaiShuttleController.ShuttleID.HourglassShuttle)?.GetOWRigidbody());
                    standardBodyLookup.Add(HeavenlyBodies.NomaiBrittleHollowShuttle, () => Locator.GetNomaiShuttle(NomaiShuttleController.ShuttleID.BrittleHollowShuttle)?.GetOWRigidbody());
                    standardBodyLookup.Add(HeavenlyBodies.DarkBramble, () => Locator.GetAstroObject(AstroObject.Name.DarkBramble)?.GetAttachedOWRigidbody());
                    standardBodyLookup.Add(HeavenlyBodies.InnerDarkBramble_Hub, () => MainClass.getSector(Sector.Name.BrambleDimension, body => OuterFogWarpVolume.Name.Hub == body?.GetComponentInChildren<OuterFogWarpVolume>()?.GetName())?.GetAttachedOWRigidbody());
                    standardBodyLookup.Add(HeavenlyBodies.InnerDarkBramble_EscapePod, () => MainClass.getSector(Sector.Name.BrambleDimension, body => OuterFogWarpVolume.Name.EscapePod == body?.GetComponentInChildren<OuterFogWarpVolume>()?.GetName())?.GetAttachedOWRigidbody());
                    standardBodyLookup.Add(HeavenlyBodies.InnerDarkBramble_Nest, () => MainClass.getSector(Sector.Name.BrambleDimension, body => OuterFogWarpVolume.Name.AnglerNest == body?.GetComponentInChildren<OuterFogWarpVolume>()?.GetName())?.GetAttachedOWRigidbody());
                    standardBodyLookup.Add(HeavenlyBodies.InnerDarkBramble_Feldspar, () => MainClass.getSector(Sector.Name.BrambleDimension, body => OuterFogWarpVolume.Name.Pioneer == body?.GetComponentInChildren<OuterFogWarpVolume>()?.GetName())?.GetAttachedOWRigidbody());
                    standardBodyLookup.Add(HeavenlyBodies.InnerDarkBramble_Gutter, () => MainClass.getSector(Sector.Name.BrambleDimension, body => OuterFogWarpVolume.Name.ExitOnly == body?.GetComponentInChildren<OuterFogWarpVolume>()?.GetName())?.GetAttachedOWRigidbody());
                    standardBodyLookup.Add(HeavenlyBodies.InnerDarkBramble_Vessel, () => MainClass.getSector(Sector.Name.VesselDimension, body => OuterFogWarpVolume.Name.Vessel == body?.GetComponentInChildren<OuterFogWarpVolume>()?.GetName())?.GetAttachedOWRigidbody());
                    standardBodyLookup.Add(HeavenlyBodies.InnerDarkBramble_Maze, () => MainClass.getSector(Sector.Name.BrambleDimension, body => OuterFogWarpVolume.Name.Cluster == body?.GetComponentInChildren<OuterFogWarpVolume>()?.GetName())?.GetAttachedOWRigidbody());
                    standardBodyLookup.Add(HeavenlyBodies.InnerDarkBramble_SmallNest, () => MainClass.getSector(Sector.Name.BrambleDimension, body => OuterFogWarpVolume.Name.SmallNest == body?.GetComponentInChildren<OuterFogWarpVolume>()?.GetName())?.GetAttachedOWRigidbody());
                    standardBodyLookup.Add(HeavenlyBodies.InnerDarkBramble_Secret, () => MainClass.getSector(Sector.Name.BrambleDimension, body => body?.GetComponentInChildren<SecretFogWarpVolume>() != null)?.GetAttachedOWRigidbody());
                    standardBodyLookup.Add(HeavenlyBodies.Interloper, () => Locator.GetAstroObject(AstroObject.Name.Comet)?.GetAttachedOWRigidbody());
                    standardBodyLookup.Add(HeavenlyBodies.WhiteHole, () => Locator.GetAstroObject(AstroObject.Name.WhiteHole)?.GetAttachedOWRigidbody());
                    standardBodyLookup.Add(HeavenlyBodies.WhiteHoleStation, () => Locator.GetAstroObject(AstroObject.Name.WhiteHoleTarget)?.GetAttachedOWRigidbody());
                    standardBodyLookup.Add(HeavenlyBodies.Stranger, () => Locator.GetAstroObject(AstroObject.Name.RingWorld)?.GetAttachedOWRigidbody());
                    standardBodyLookup.Add(HeavenlyBodies.DreamWorld, () => Locator.GetAstroObject(AstroObject.Name.DreamWorld)?.GetAttachedOWRigidbody());
                    standardBodyLookup.Add(HeavenlyBodies.QuantumMoon, () => Locator.GetAstroObject(AstroObject.Name.QuantumMoon)?.GetAttachedOWRigidbody());
                    standardBodyLookup.Add(HeavenlyBodies.SatelliteBacker, () => Locator.GetMinorAstroObject("Backer's Satellite")?.GetAttachedOWRigidbody());
                    standardBodyLookup.Add(HeavenlyBodies.SatelliteMapping, () => Locator.GetAstroObject(AstroObject.Name.MapSatellite)?.GetAttachedOWRigidbody() ?? MainClass.getSector(Sector.Name.Unnamed, body => "Sector_HearthianMapSatellite".Equals(body?.gameObject?.name))?.GetAttachedOWRigidbody());
                    standardBodyLookup.Add(HeavenlyBodies.EyeOfTheUniverse, () => Locator.GetAstroObject(AstroObject.Name.Eye)?.GetAttachedOWRigidbody() ?? MainClass.getSector(Sector.Name.EyeOfTheUniverse)?.GetAttachedOWRigidbody());
                    standardBodyLookup.Add(HeavenlyBodies.EyeOfTheUniverse_Vessel, () => Locator.GetMinorAstroObject("Vessel")?.GetAttachedOWRigidbody() ?? MainClass.getSector(Sector.Name.Vessel)?.GetAttachedOWRigidbody());
                }
                else
                {
                    standardBodyLookup.Add(HeavenlyBodies.EyeOfTheUniverse, () => Locator.GetAstroObject(AstroObject.Name.Eye)?.GetAttachedOWRigidbody() ?? MainClass.getSector(Sector.Name.EyeOfTheUniverse)?.GetAttachedOWRigidbody());
                    standardBodyLookup.Add(HeavenlyBodies.EyeOfTheUniverse_Vessel, () => Locator.GetMinorAstroObject("Vessel")?.GetAttachedOWRigidbody() ?? MainClass.getSector(Sector.Name.Vessel)?.GetAttachedOWRigidbody());
                }

                return standardBodyLookup;
            }
        }

        public static Dictionary<HeavenlyBody, Func<AstroObject>> AstroLookup
        {
            get
            {
                var value = new Dictionary<HeavenlyBody, Func<AstroObject>>();
                foreach (var val in _astroLookup)
                {
                    value.Add(val.Key, val.Value);
                }
                return value;
            }
            set
            {
                astros.Clear();
                _astroLookup.Clear();
                foreach (var val in value)
                {
                    _astroLookup.Add(val.Key, val.Value);
                }
            }
        }

        public static Dictionary<HeavenlyBody, Func<OWRigidbody>> BodyLookup
        {
            get
            {
                var value = new Dictionary<HeavenlyBody, Func<OWRigidbody>>();
                foreach (var val in _bodyLookup)
                {
                    value.Add(val.Key, val.Value);
                }
                return value;
            }
            set
            {
                bodies.Clear();
                _bodyLookup.Clear();
                foreach (var val in value)
                {
                    _bodyLookup.Add(val.Key, val.Value);
                }
            }
        }

        public static void Awake()
        {
            _astroLookup = StandardAstroLookup;
            _bodyLookup = StandardBodyLookup;
            astros.Clear();
            bodies.Clear();
        }

        public static HeavenlyBody find(OWRigidbody body)
        {
            if (body == null)
            {
                return HeavenlyBody.None;
            }

            foreach (HeavenlyBody pp in HeavenlyBody.GetValues())
            {
                if (getBody(pp) == body)
                {
                    return pp;
                }
            }

            return HeavenlyBody.None;
        }

        public static HeavenlyBody find(AstroObject body)
        {
            if (body == null)
            {
                return HeavenlyBody.None;
            }

            foreach (HeavenlyBody pp in HeavenlyBody.GetValues())
            {
                if (getAstro(pp) == body)
                {
                    return pp;
                }
            }

            return HeavenlyBody.None;
        }

        private static AstroObject lookupAstro(HeavenlyBody value)
        {
            if (value == null)
            {
                return null;
            }

            Func<AstroObject> obj;
            if (_astroLookup.TryGetValue(value, out obj))
            {
                var owBody = obj?.Invoke();
                if (owBody != null)
                {
                    astros[value] = owBody;
                }
                return owBody;
            }
            return null;
        }

        public static AstroObject getAstro(HeavenlyBody body)
        {
            if (body == null)
            {
                return null;
            }

            AstroObject obj;
            if (!astros.TryGetValue(body, out obj)
                || obj == null || obj?.gameObject == null || !obj.gameObject.activeSelf)
            {
                obj = lookupAstro(body);
            }
            return obj == null || obj?.gameObject == null || !obj.gameObject.activeSelf ? null : obj;
        }

        public static HeavenlyBody[] getAstros()
        {
            var keys = new HeavenlyBody[_astroLookup.Count];
            _astroLookup.Keys.CopyTo(keys, 0);
            return keys;
        }

        private static OWRigidbody lookupBody(HeavenlyBody value)
        {
            if (value == null)
            {
                return null;
            }

            Func<OWRigidbody> obj;
            if (_bodyLookup.TryGetValue(value, out obj))
            {
                var owBody = obj?.Invoke();
                if (owBody != null)
                {
                    bodies[value] = owBody;
                }
                return owBody;
            }
            return null;
        }

        public static OWRigidbody getBody(HeavenlyBody body)
        {
            if (body == null)
            {
                return null;
            }

            MainClass.Console.WriteLine($"Getting rigidbody from {body.name}");

            OWRigidbody obj;
            if (!bodies.TryGetValue(body, out obj)
                || obj == null || obj?.GetRigidbody() == null || obj?.gameObject == null || !obj.gameObject.activeSelf)
            {
                obj = lookupBody(body);
            }
            return obj == null || obj?.GetRigidbody() == null || obj?.gameObject == null || !obj.gameObject.activeSelf ? null : obj;
        }

        public static HeavenlyBody[] getBodies()
        {
            var keys = new HeavenlyBody[_bodyLookup.Count];
            _bodyLookup.Keys.CopyTo(keys, 0);
            return keys;
        }
    }
}
