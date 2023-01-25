using OWML.Common;
using OWML.ModHelper;
using OWML.ModHelper.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CheatsMod.CR
{
    public class InputMapping<T>
    {
        private Dictionary<T, MultiInputClass> _inputMap = new Dictionary<T, MultiInputClass>();

        public void addInput(string input, T value)
        {
            addInput(getInputConfig(input), value);
        }

        public void addInput(IModConfig config, T option, string defaultValue)
        {
            var name = Enum.GetName(option.GetType(), option).Replace("_", " ");
            var input = getInputConfigOrDefault(config, name, defaultValue);
            addInput(input, option);
        }

        private void addInput(MultiInputClass input, T value)
        {
            _inputMap.Add(value, input);
        }

        private MultiInputClass getInputConfig(string input)
        {
            return MultiInputClass.fromString(input);
        }

        private MultiInputClass getInputConfigOrDefault(IModConfig config, string id, string defaultValue)
        {
            return getInputConfig(ConfigHelper.getConfigOrDefault<string>(config, id, defaultValue));
        }

        public void Clear()
        {
            _inputMap.Clear();
        }

        public void Update()
        {
            foreach (MultiInputClass input in _inputMap.Values)
            {
                input.Update();
            }
        }

        public List<Tuple<T, MultiInputClass>> getPressedThisFrame()
        {
            var bindings = new List<Tuple<T, MultiInputClass>>();
            foreach (var keyBindings in _inputMap)
            {
                if (keyBindings.Value.isPressedThisFrame())
                {
                    bindings.Add(Tuple.Create(keyBindings.Key, keyBindings.Value));
                }
            }
            bindings.Sort((x1, x2) => x2.Item2.keyMatchCount().CompareTo(x1.Item2.keyMatchCount()));
            return bindings;
        }

        public List<Tuple<T, MultiInputClass>> getPressed()
        {
            var bindings = new List<Tuple<T, MultiInputClass>>();
            foreach (var keyBindings in _inputMap)
            {
                if (keyBindings.Value.isPressed())
                {
                    bindings.Add(Tuple.Create(keyBindings.Key, keyBindings.Value));
                }
            }
            bindings.Sort((x1, x2) => x2.Item2.keyMatchCount().CompareTo(x1.Item2.keyMatchCount()));
            return bindings;
        }

        public HashSet<Key> getKeysPressed()
        {
            var keys = new HashSet<Key>();
            foreach (Key key in Enum.GetValues(typeof(Key)))
            {
                if (Keyboard.current[key].IsActuated())
                {
                    keys.Add(key);
                }
            }

            return keys;
        }
    }


    public class InputClass : IEquatable<InputClass>
    {
        private const char _split = ',';
        private HashSet<Key> _keys;
        private int _pressed = 0;
        private float _time = 0f;

        private InputClass(HashSet<Key> keys)
        {
            this._keys = keys;
        }

        public InputClass(params Key[] keys)
        {
            this._keys = new HashSet<Key>(keys);
        }

        public void Update()
        {
            bool areAllPressed = true;
            foreach (Key key in _keys)
            {
                if (!Keyboard.current[key].IsActuated())
                {
                    areAllPressed = false;
                    break;
                }
            }

            if (areAllPressed)
            {
                _pressed++;
            }
            else
            {
                _pressed = 0;
            }
        }

        public List<Key> getKeys()
        {
            return new List<Key>(_keys);
        }

        public static InputClass fromString(string keysString)
        {
            HashSet<Key> keys = new HashSet<Key>();
            foreach (string keyString in keysString.Split(_split))
            {
                try
                {
                    var key = (Key)Enum.Parse(Key.A.GetType(), keyString, true);
                    int keyInt;
                    if (int.TryParse(keyString, out keyInt))
                    {
                        MainClass.Console.WriteLine("Key `" + keyString + "` is not recognized.", MessageType.Warning);
                    }
                    keys.Add(key);
                }
                catch (Exception)
                {
                    MainClass.Console.WriteLine("Key `" + keyString + "` is not recognized.", MessageType.Warning);
                }
            }
            return new InputClass(keys);
        }

        public override string ToString()
        {
            string value = "";
            foreach (Key key in _keys)
            {
                value += Enum.GetName(key.GetType(), key);
                value += _split;
            }
            return value.Substring(0, value.Length - 1);
        }

        public override bool Equals(System.Object other)
        {
            if (other != null && other is InputClass)
            {
                return Equals((InputClass)(other as InputClass));
            }
            return false;
        }
        public bool Equals(InputClass other)
        {
            if (other != null)
            {
                if (other._keys.Count == _keys.Count)
                {
                    foreach (var key in _keys)
                    {
                        if (!other._keys.Contains(key))
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            int hash = 0;
            foreach (var key in _keys)
            {
                hash += ((int)key) * 4;
            }
            return hash;
        }

        public bool isPressedThisFrame()
        {
            return _pressed == 1;
        }

        public bool isPressed()
        {
            return _pressed > 0;
        }

        public int frameCountPressed()
        {
            return _pressed;
        }

        public float timePressed()
        {
            return (_time == 0f) ? 0f : (Time.time - _time);
        }

        public int count()
        {
            return _keys.Count;
        }
    }

    public class MultiInputClass : IEquatable<MultiInputClass>
    {
        private const char _split = '|';
        HashSet<InputClass> _keys;
        private int _pressed = 0;
        private float _time = 0f;
        private int _keyMatchCount = 0;

        private MultiInputClass(HashSet<InputClass> keys)
        {
            _keys = keys;
        }

        public MultiInputClass(params InputClass[] keys)
        {
            _keys = new HashSet<InputClass>(keys);
        }

        public void Update()
        {
            bool isPressed = false;
            foreach (InputClass key in _keys)
            {
                key.Update();
                if (key.isPressed())
                {
                    isPressed = true;
                    if (key.count() > _keyMatchCount)
                    {
                        _keyMatchCount = key.count();
                    }
                }
            }
            if (isPressed)
            {
                _pressed++;
                if (_time == 0f)
                {
                    _time = Time.time;
                }
            }
            else
            {
                _pressed = 0;
                _time = 0f;
                _keyMatchCount = 0;
            }
        }

        public List<InputClass> getKeysCombos()
        {
            return new List<InputClass>(_keys);
        }

        public static MultiInputClass fromString(string keysString)
        {
            HashSet<InputClass> keys = new HashSet<InputClass>();
            foreach (string keyString in keysString.Split(_split))
            {
                keys.Add(InputClass.fromString(keyString));
            }
            return new MultiInputClass(keys);
        }

        public override string ToString()
        {
            string value = "";
            foreach (InputClass key in _keys)
            {
                value += key.ToString();
                value += _split;
            }
            return value.Substring(0, value.Length - 1);
        }

        public override bool Equals(System.Object other)
        {
            if (other != null && other is MultiInputClass)
            {
                return Equals((MultiInputClass)(other as MultiInputClass));
            }
            return false;
        }
        public bool Equals(MultiInputClass other)
        {
            if (other != null)
            {
                if (other._keys.Count == _keys.Count)
                {
                    foreach (var key in _keys)
                    {
                        if (!other._keys.Contains(key))
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            int hash = 0;
            foreach (var key in _keys)
            {
                hash += key.GetHashCode() * 16;
            }
            return hash;
        }

        public bool isPressedThisFrame()
        {
            return _pressed == 1;
        }

        public bool isPressed()
        {
            return _pressed > 0;
        }

        public int frameCountPressed()
        {
            return _pressed;
        }

        public float timePressed()
        {
            return (_time == 0f) ? 0f : (Time.time - _time);
        }

        public int keyMatchCount()
        {
            return _keyMatchCount;
        }
    }
}
