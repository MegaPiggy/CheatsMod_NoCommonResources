using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheatsMod.Utils
{
    public class HeavenlyBody : IEquatable<HeavenlyBody>
    {
        private static Dictionary<string, HeavenlyBody> _map = new Dictionary<string, HeavenlyBody>();
        private static int _nextValue = 1;

        public readonly static HeavenlyBody None = new HeavenlyBody("None", 0, true);

        private int _value;
        private string _name;
        private bool _pseudoHeavenlyBody;

        public int value { get { return _value; } }
        public string name { get { return _name; } }
        public bool pseudoHeavenlyBody { get { return _pseudoHeavenlyBody; } }

        public static HeavenlyBody FromString(string name, bool create = false)
        {
            HeavenlyBody value;
            if (_map.TryGetValue(name, out value))
            {
                return value;
            }
            else if (create)
            {
                return new HeavenlyBody(name);
            }
            else
            {
                return HeavenlyBody.None;
            }
        }

        public static HeavenlyBody[] GetValues()
        {
            return _map.Values.ToArray();
        }

        private HeavenlyBody(string name, int value, bool isPseudoHeavenlyBody = false)
        {
            this._value = value;
            this._name = name;
            this._pseudoHeavenlyBody = isPseudoHeavenlyBody;

            _map.Add(_name, this);
        }

        public HeavenlyBody(string name, bool isPseudoHeavenlyBody = false)
        {
            if (name == null)
            {
                throw new ArgumentException($"name cannot be null");
            }
            else if (name.Length == 0)
            {
                throw new ArgumentException($"name cannot be blank");
            }
            else if (_map.ContainsKey(name))
            {
                throw new ArgumentException($"{name} already in use");
            }

            this._value = _nextValue++;
            this._name = name;
            this._pseudoHeavenlyBody = isPseudoHeavenlyBody;

            _map.Add(_name, this);
        }

        public override string ToString()
        {
            return _name;
        }

        public override bool Equals(object other)
        {
            if (other is null && this._value == None._value)
            {
                return true;
            }
            else if (!(other is null) && other is HeavenlyBody otherBody)
            {
                return Equals(otherBody);
            }
            return false;
        }

        public bool Equals(HeavenlyBody other)
        {
            if (other is null && this._value == None._value)
            {
                return true;
            }
            if (other != null)
            {
                return _value == other._value;
            }
            return false;
        }

        public static bool operator ==(HeavenlyBody x, HeavenlyBody y)
        {
            if (x is null)
            {
                return y is null || y._value == None._value;
            }
            else if (y is null)
            {
                return x is null || x._value == None._value;
            }
            else
            {
                return x._value == y._value;
            }
        }

        public static bool operator !=(HeavenlyBody x, HeavenlyBody y)
        {
            return !(x == y);
        }

        public override int GetHashCode()
        {
            return _value;
        }

        ~HeavenlyBody()
        {
            if (_name != null)
            {
                _map.Remove(_name);
            }
            _value = None._value;
            _name = null;
        }
    }

    public static class HeavenlyBodies
    {
        private const string prefix = "OuterWild_Standard_";

        public static HeavenlyBody None = HeavenlyBody.None;
        public static HeavenlyBody Player = new HeavenlyBody($"{prefix}Player");
        public static HeavenlyBody Ship = new HeavenlyBody($"{prefix}Ship");
        public static HeavenlyBody Probe = new HeavenlyBody($"{prefix}Probe");
        public static HeavenlyBody ModelShip = new HeavenlyBody($"{prefix}Model_Ship");
        public static HeavenlyBody Sun = new HeavenlyBody($"{prefix}Sun");
        public static HeavenlyBody SunStation = new HeavenlyBody($"{prefix}Sun_Station");
        public static HeavenlyBody HourglassTwins = new HeavenlyBody($"{prefix}Hourglass_Twins", true);
        public static HeavenlyBody AshTwin = new HeavenlyBody($"{prefix}Ash_Twin");
        public static HeavenlyBody EmberTwin = new HeavenlyBody($"{prefix}Ember_Twin");
        public static HeavenlyBody TimberHearth = new HeavenlyBody($"{prefix}Timber_Hearth");
        public static HeavenlyBody SkyShutterSatellite = new HeavenlyBody($"{prefix}Sky_Shutter_Satellite");
        public static HeavenlyBody Attlerock = new HeavenlyBody($"{prefix}Attlerock");
        public static HeavenlyBody BrittleHollow = new HeavenlyBody($"{prefix}Brittle_Hollow");
        public static HeavenlyBody HollowsLantern = new HeavenlyBody($"{prefix}Hollows_Lantern");
        public static HeavenlyBody GiantsDeep = new HeavenlyBody($"{prefix}Giants_Deep");
        public static HeavenlyBody ProbeCannon = new HeavenlyBody($"{prefix}Probe_Cannon");
        public static HeavenlyBody NomaiProbe = new HeavenlyBody($"{prefix}Nomai_Probe");
        public static HeavenlyBody NomaiEmberTwinShuttle = new HeavenlyBody($"{prefix}Nomai_Ember_Twin_Shuttle");
        public static HeavenlyBody NomaiBrittleHollowShuttle = new HeavenlyBody($"{prefix}Nomai_Brittle_Hollow_Shuttle");
        public static HeavenlyBody DarkBramble = new HeavenlyBody($"{prefix}Dark_Bramble");
        public static HeavenlyBody InnerDarkBramble_Hub = new HeavenlyBody($"{prefix}Inner_Dark_Bramble_Hub");
        public static HeavenlyBody InnerDarkBramble_EscapePod = new HeavenlyBody($"{prefix}Inner_Dark_Bramble_Escape_Pod");
        public static HeavenlyBody InnerDarkBramble_Nest = new HeavenlyBody($"{prefix}Inner_Dark_Bramble_Nest");
        public static HeavenlyBody InnerDarkBramble_Feldspar = new HeavenlyBody($"{prefix}Inner_Dark_Bramble_Feldspar");
        public static HeavenlyBody InnerDarkBramble_Gutter = new HeavenlyBody($"{prefix}Inner_Dark_Bramble_Gutter");
        public static HeavenlyBody InnerDarkBramble_Vessel = new HeavenlyBody($"{prefix}Inner_Dark_Bramble_Vessel");
        public static HeavenlyBody InnerDarkBramble_Maze = new HeavenlyBody($"{prefix}Inner_Dark_Bramble_Maze");
        public static HeavenlyBody InnerDarkBramble_SmallNest = new HeavenlyBody($"{prefix}Inner_Dark_Bramble_Small_Nest");
        public static HeavenlyBody InnerDarkBramble_Secret = new HeavenlyBody($"{prefix}Inner_Dark_Bramble_Secret");
        public static HeavenlyBody Interloper = new HeavenlyBody($"{prefix}Interloper");
        public static HeavenlyBody WhiteHole = new HeavenlyBody($"{prefix}White_Hole");
        public static HeavenlyBody WhiteHoleStation = new HeavenlyBody($"{prefix}White_Hole_Station");
        public static HeavenlyBody Stranger = new HeavenlyBody($"{prefix}Stranger");
        public static HeavenlyBody DreamWorld = new HeavenlyBody($"{prefix}Dream_World");
        public static HeavenlyBody QuantumMoon = new HeavenlyBody($"{prefix}Quantum_Moon");
        public static HeavenlyBody SatelliteBacker = new HeavenlyBody($"{prefix}Satellite_Backer");
        public static HeavenlyBody SatelliteMapping = new HeavenlyBody($"{prefix}Satellite_Mapping");
        public static HeavenlyBody EyeOfTheUniverse = new HeavenlyBody($"{prefix}Eye_Of_The_Universe");
        public static HeavenlyBody EyeOfTheUniverse_Vessel = new HeavenlyBody($"{prefix}Eye_Of_The_Universe_Vessel");
    }
}
