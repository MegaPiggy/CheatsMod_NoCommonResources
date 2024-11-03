using OWML.Common;
using OWML.ModHelper;
using OWML.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CheatsModNoCR.Utils
{
    public static class Player
    {
        public static PlayerResources getResources()
        {
            if (Locator.GetPlayerTransform() && Locator.GetPlayerTransform().TryGetComponent(out PlayerResources resources)) return resources;
            return null;
        }

        public static JetpackThrusterModel getJetpack()
        {
            if (getResources() && getResources().TryGetComponent(out JetpackThrusterModel model)) return model;
            return null;
        }

        public static bool hasUnlimitedBoost { get; set; }
        public static bool hasUnlimitedFuel { get; set; }
        public static bool hasUnlimitedOxygen { get; set; }
        public static bool hasUnlimitedHealth { get; set; }
        public static bool isInvincible { get; set; }
        public static float boostSeconds
        {
            get
            {
                if (getJetpack() != null)
                    return getJetpack()._boostChargeFraction * maxBoostSeconds;
                else
                    return maxBoostSeconds;
            }
            set
            {
                if (getJetpack() != null)
                {
                    getJetpack()._boostChargeFraction = value / maxBoostSeconds;
                }
            }
        }
        public static float maxBoostSeconds
        {
            get
            {
                if (getJetpack() != null)
                    return getJetpack()._boostSeconds;
                else
                    return 1f;
            }
            set
            {
                if (getJetpack() != null)
                {
                    getJetpack()._boostSeconds = value;
                }
            }
        }
        public static int suitPunctureCount
        {
            get
            {
                if (getResources() != null)
                    return getResources()._currentNumPunctures;
                else
                    return 0;
            }
            set
            {
                if (getResources() != null)
                {
                    getResources()._currentNumPunctures = value;
                }
            }
        }
        public static float health
        {
            get
            {
                if (getResources() != null)
                    return getResources()._currentHealth;
                else
                    return maxHealth;
            }
            set
            {
                if (getResources() != null)
                {
                    getResources()._currentHealth = value;
                }
            }
        }
        public static float maxHealth { get; set; } = 100f;
        public static float fuelSeconds
        {
            get
            {
                if (getResources() != null)
                    return getResources()._currentFuel;
                else
                    return maxFuelSeconds;
            }
            set
            {
                if (getResources() != null)
                {
                    getResources()._currentFuel = value;
                }
            }
        }
        public static float maxFuelSeconds { get; set; } = 100f;
        public static float oxygenSeconds
        {
            get
            {
                if (getResources() != null)
                    return getResources()._currentOxygen;
                else
                    return maxOxygenSeconds;
            }
            set
            {
                if (getResources() != null)
                {
                    getResources()._currentOxygen = value;
                }
            }
        }
        public static float maxOxygenSeconds { get; set; } = 450f;
        public static bool invincible
        {
            get
            {
                if (getResources() != null)
                    return getResources()._invincible;
                else
                    return false;
            }
            set
            {
                if (getResources() != null)
                {
                    getResources()._invincible = value;
                }
            }
        }
        public static bool gravity
        {
            get
            {
                if (Locator.GetPlayerBody())
                {
                    var applier = Locator.GetPlayerBody().GetComponentInChildren<ForceApplier>();
                    return applier?.GetApplyForces() ?? true;
                }
                return true;
            }
            set
            {
                if (Locator.GetPlayerBody())
                {
                    var applier = Locator.GetPlayerBody().GetComponentInChildren<ForceApplier>();
                    applier?.SetApplyForces(value);
                }
            }
        }
        public static bool fluidCollision
        {
            get
            {
                if (Locator.GetPlayerBody())
                {
                    var applier = Locator.GetPlayerBody().GetComponentInChildren<ForceApplier>();
                    return applier?.GetApplyFluids() ?? true;
                }
                return true;
            }
            set
            {
                if (Locator.GetPlayerBody())
                {
                    var applier = Locator.GetPlayerBody().GetComponentInChildren<ForceApplier>();
                    applier?.SetApplyFluids(value);
                }
            }
        }
        public static bool collision
        {
            get
            {
                if (Locator.GetPlayerBody())
                {
                    if (!Locator.GetPlayerBody().GetRequiredComponent<Rigidbody>().detectCollisions)
                    {
                        return false;
                    }
                }
                return true;
            }
            set
            {
                if (Locator.GetPlayerBody())
                {
                    if (!value)
                    {
                        Locator.GetPlayerBody().DisableCollisionDetection();
                    }
                    else
                    {
                        Locator.GetPlayerBody().EnableCollisionDetection();
                    }

                    foreach (Collider collider in Locator.GetPlayerBody().GetComponentsInChildren<Collider>())
                    {
                        if (!collider.isTrigger)
                        {
                            collider.enabled = value;
                        }
                    }
                }
            }
        }
        public static bool helmet
        {
            get
            {
                if (Locator.GetPlayerSuit())
                {
                    if ((spaceSuit || trainingSuit) && Locator.GetPlayerSuit().IsWearingHelmet())
                    {
                        return true;
                    }
                }
                return false;
            }
            set
            {
                if (Locator.GetPlayerSuit() && (spaceSuit || trainingSuit) && helmet != value)
                {
                    if (value)
                    {
                        Locator.GetPlayerSuit().PutOnHelmet();
                    }
                    else
                    {
                        Locator.GetPlayerSuit().RemoveHelmet();
                    }
                }
            }
        }
        public static bool spaceSuit
        {
            get
            {
                if (Locator.GetPlayerSuit())
                {
                    if (Locator.GetPlayerSuit().IsWearingSuit(false))
                    {
                        return true;
                    }
                }
                return false;
            }
            set
            {
                if (Locator.GetPlayerSuit() && value != spaceSuit)
                {
                    if (value)
                    {
                        if (trainingSuit)
                        {
                            Locator.GetPlayerSuit().RemoveSuit(true);
                            Locator.GetPlayerSuit().SuitUp(false, true, true);
                        }
                        else
                        {
                            Locator.GetPlayerSuit().SuitUp(false, false, true);
                        }
                    }
                    else
                    {
                        Locator.GetPlayerSuit().RemoveSuit(false);
                    }
                }
            }
        }
        public static bool trainingSuit
        {
            get
            {
                if (Locator.GetPlayerSuit())
                {
                    if (!Locator.GetPlayerSuit().IsWearingSuit(false) && Locator.GetPlayerSuit().IsWearingSuit(true))
                    {
                        return true;
                    }
                }
                return false;
            }
            set
            {
                if (Locator.GetPlayerSuit() && value != trainingSuit)
                {
                    if (value)
                    {
                        if (spaceSuit)
                        {
                            Locator.GetPlayerSuit().RemoveSuit(true);
                            Locator.GetPlayerSuit().SuitUp(true, true, true);
                        }
                        else
                        {
                            Locator.GetPlayerSuit().SuitUp(true, false, true);
                        }
                    }
                    else
                    {
                        Locator.GetPlayerSuit().RemoveSuit(false);
                    }
                }
            }
        }
        public static float thrust
        {
            get
            {
                var model = getJetpack();
                if (model)
                    return model.GetMaxTranslationalThrust();
                return 6f;
            }
            set
            {
                var model = getJetpack();
                if (model)
                    model._maxTranslationalThrust = value;
            }
        }

        public static void Update()
        {
            if (hasUnlimitedBoost)
                boostSeconds = maxBoostSeconds;
            if (hasUnlimitedFuel)
                fuelSeconds = maxFuelSeconds;
            if (hasUnlimitedOxygen)
                oxygenSeconds = maxOxygenSeconds;
            if (hasUnlimitedHealth)
                health = maxHealth;
            invincible = isInvincible;

            if (boostSeconds > maxBoostSeconds)
                boostSeconds = maxBoostSeconds;
            if (health > maxHealth)
                health = maxHealth;


            var resources = getResources();
            if (resources != null)
            {
                if (!resources.IsOxygenPresent() && oxygenSeconds > maxOxygenSeconds)
                {
                    if (resources.IsRefillingOxygen())
                    {
                        resources._refillingOxygen = false;
                    }
                    oxygenSeconds = maxOxygenSeconds;
                }

                if (fuelSeconds > maxFuelSeconds)
                {
                    if (resources.IsRefueling())
                    {
                        if (resources.IsHealing())
                        {
                            resources.StopRefillResources();
                            resources.StartRefillResources(false, true);
                        }
                        else
                        {
                            resources.StopRefillResources();
                        }
                    }
                    fuelSeconds = maxFuelSeconds;
                }
            }
        }
    }
}
