using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CheatsMod
{
	public class LudicrousSpeed : MonoBehaviour
	{
		private bool _engageLudicrousSpeed;

		private void Update()
		{
			if (Keyboard.current.ctrlKey.isPressed)
			{
				if (Keyboard.current.lKey.wasPressedThisFrame)
				{
					_engageLudicrousSpeed = true;
				}
				else if (Keyboard.current.lKey.wasReleasedThisFrame)
				{
					_engageLudicrousSpeed = false;
				}
			}
		}

		private void FixedUpdate()
		{
			if (_engageLudicrousSpeed)
			{
				_engageLudicrousSpeed = false;
				Locator.GetShipBody().AddVelocityChange(Locator.GetShipBody().transform.forward * 25000f);
				MainClass.Console.WriteLine("ENGAGE LUDICROUS SPEED");
			}
		}
	}

}
