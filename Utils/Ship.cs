using OWML.Common;
using OWML.ModHelper;
using OWML.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CheatsMod.Utils
{
    public static class Ship
    {
        public static ShipResources getResources()
        {
            if (Locator.GetShipTransform() && Locator.GetShipTransform().TryGetComponent(out ShipResources resources)) return resources;
            return null;
        }

        public static ShipDamageController getDamageController()
        {
            if (Locator.GetShipTransform() && Locator.GetShipTransform().TryGetComponent(out ShipDamageController controller)) return controller;
            return null;
        }

        public static ShipThrusterModel getThrustModel()
        {
            if (Locator.GetShipTransform() && Locator.GetShipTransform().TryGetComponent(out ShipThrusterModel controller)) return controller;
            return null;
        }

        public static ShipEjectionSystem getEjectionSystem()
        {
            return Locator.GetShipTransform() ? Locator.GetShipTransform().GetComponentInChildren<ShipEjectionSystem>() : null;
        }

        public static ForceApplier getForceApplier()
        {
            return Locator.GetShipTransform() ? Locator.GetShipTransform().GetComponentInChildren<ForceApplier>() : null;
        }

        public static bool hasUnlimitedFuel { get; set; }
        public static bool hasUnlimitedOxygen { get; set; }
        public static bool isInvincible { get; set; }

        public static float fuelSeconds
        {
            get
            {
                if (getResources() != null)
                    return getResources().GetFuel();
                else
                    return maxFuelSeconds;
            }
            set
            {
                if (getResources() != null)
                {
                    getResources().SetFuel(value);
                }
            }
        }
        public static float maxFuelSeconds { get; set; } = 10000f;
        public static float oxygenSeconds
        {
            get
            {
                if (getResources() != null)
                    return getResources().GetOxygen();
                else
                    return maxOxygenSeconds;
            }
            set
            {
                if (getResources() != null)
                {
                    getResources().SetOxygen(value);
                }
            }
        }
        public static float maxOxygenSeconds { get; set; } = 6000f;
        public static bool invincible
        {
            get
            {
                if (getDamageController() != null)
                    return getDamageController()._invincible;
                else
                    return false;
            }
            set
            {
                if (getDamageController() != null)
                {
                    getDamageController()._invincible = value;
                }
            }
        }
        public static bool gravity
        {
            get
            {
                if (Locator.GetShipBody() != null)
                {
                    return getForceApplier()?.GetApplyForces() ?? true;
                }
                return true;
            }
            set
            {
                if (Locator.GetShipBody() != null)
                {
                    getForceApplier()?.SetApplyForces(value);
                }
            }
        }
        public static bool fluidCollision
        {
            get
            {
                if (Locator.GetShipBody() != null)
                {
                    return getForceApplier()?.GetApplyFluids() ?? true;
                }
                return true;
            }
            set
            {
                if (Locator.GetShipBody() != null)
                {
                    getForceApplier()?.SetApplyFluids(value);
                }
            }
        }
        public static bool collision
        {
            get
            {
                if (Locator.GetShipBody() != null)
                {
                    if (!Locator.GetShipBody().GetRequiredComponent<Rigidbody>().detectCollisions)
                    {
                        return false;
                    }
                }
                return true;
            }
            set
            {
                if (Locator.GetShipBody())
                {
                    if (!value)
                    {
                        Locator.GetShipBody().DisableCollisionDetection();
                    }
                    else
                    {
                        Locator.GetShipBody().EnableCollisionDetection();
                    }

                    foreach (Collider collider in Locator.GetShipBody().GetComponentsInChildren<Collider>())
                    {
                        if (!collider.isTrigger)
                        {
                            collider.enabled = value;
                        }
                    }
                }
            }
        }
        public static float thrust
        {
            get
            {
                var model = getThrustModel();
                if (model)
                    return model.GetMaxTranslationalThrust();
                return 50f;
            }
            set
            {
                var model = getThrustModel();
                if (model)
                    model._maxTranslationalThrust = value;
            }
        }

        public static void Update()
        {
            if (hasUnlimitedFuel)
                fuelSeconds = maxFuelSeconds;
            if (hasUnlimitedOxygen)
                oxygenSeconds = maxOxygenSeconds;
            invincible = isInvincible;

            if (fuelSeconds > maxFuelSeconds)
                fuelSeconds = maxFuelSeconds;
            if (oxygenSeconds > maxOxygenSeconds)
                oxygenSeconds = maxOxygenSeconds;
        }

        public static void repair()
        {
            if (Locator.GetShipTransform() != null)
            {
                foreach (ShipHull hull in Locator.GetShipTransform().GetComponentsInChildren<ShipHull>())
                {
                    hull._damaged = false;
                    hull._integrity = 1;
                }
                foreach (ShipComponent component in Locator.GetShipTransform().GetComponentsInChildren<ShipComponent>())
                {
                    component.SetDamaged(false);
                }
            }
        }

        public static void eject()
        {
            var shipBody = Locator.GetShipBody();
            if (shipBody != null)
            {
                var damageController = getDamageController();
                if (damageController != null) damageController._invincible = false;

                var ejectionSystem = getEjectionSystem();
                if (ejectionSystem != null)
                {
                    ejectionSystem._ejectPressed = true;
                    ejectionSystem._ejectPrimed = true;
                    ejectionSystem._raising = true;
                    ejectionSystem._audioController.PlayRaiseEjectCover();
                    ejectionSystem._pressTime += ejectionSystem._secondPressDelay;
                    ejectionSystem._coverT = 1;
                    OWRigidbody cockpitBody = ejectionSystem._cockpitModule.Detach();
                    shipBody.transform.position -= shipBody.transform.forward * 2f;
                    float ejectImpulse = ejectionSystem._ejectImpulse;
                    if (Locator.GetShipDetector().GetComponent<ShipFluidDetector>().InOceanBarrierZone())
                    {
                        MainClass.Console.WriteLine("Ship in ocean barrier zone, reducing eject impulse.", MessageType.Info);
                        ejectImpulse = 1f;
                    }
                    shipBody.AddLocalImpulse(Vector3.back * ejectImpulse);
                    Vector3 localImpulse = Vector3.forward * ejectImpulse;
                    cockpitBody.AddLocalImpulse(localImpulse);
                    ejectionSystem._audioController.PlayEject();
                    RumbleManager.PulseEject();
                    ejectionSystem.enabled = false;
                }
            }
        }

        public static void explode()
        {
            var shipBody = Locator.GetShipBody();
            if (shipBody != null)
            {
                var damageController = getDamageController();
                if (damageController != null)
                {
                    damageController._invincible = false;
                    damageController.Explode(true);
                }
            }
        }
    }
}
