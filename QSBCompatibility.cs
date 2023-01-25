using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using QSB;
using QSB.WorldSync;
using QSB.ItemSync.WorldObjects.Items;
using QSB.ItemSync.WorldObjects.Sockets;
using QSB.EchoesOfTheEye.DreamLantern.WorldObjects;
using QSB.DeathSync.Patches;

namespace CheatsMod
{
    public static class QSBCompatibility
	{
		public static void DoPatches()
		{
			MainClass.HarmonyHelper.AddPrefix<DeathPatches>(nameof(DeathPatches.HighSpeedImpactSensor_FixedUpdate), typeof(MainClass), nameof(MainClass.HighSpeedImpactSensor_FixedUpdate));
			MainClass.HarmonyHelper.AddPrefix<DeathPatches>(nameof(DeathPatches.PlayerResources_OnImpact), typeof(MainClass), nameof(MainClass.PlayerResources_OnImpact));
		}
		public static void Init(DreamLanternController controller) => QSBWorldSync.Init<QSBDreamLantern, DreamLanternController>(new List<DreamLanternController> { controller });
		public static void Init(DreamLanternItem item) => QSBWorldSync.Init<QSBDreamLanternItem, DreamLanternItem>(new List<DreamLanternItem> { item });
		public static void Init(NomaiConversationStone item) => QSBWorldSync.Init<QSBNomaiConversationStone, NomaiConversationStone>(new List<NomaiConversationStone> { item });
		public static void Init(ScrollItem item) => QSBWorldSync.Init<QSBScrollItem, ScrollItem>(new List<ScrollItem> { item });
		public static void Init(SharedStone item) => QSBWorldSync.Init<QSBSharedStone, SharedStone>(new List<SharedStone> { item });
		public static void Init(SimpleLanternItem item) => QSBWorldSync.Init<QSBSimpleLanternItem, SimpleLanternItem>(new List<SimpleLanternItem> { item });
		public static void Init(SlideReelItem item) => QSBWorldSync.Init<QSBSlideReelItem, SlideReelItem>(new List<SlideReelItem> { item });
		public static void Init(VisionTorchItem item) => QSBWorldSync.Init<QSBVisionTorchItem, VisionTorchItem>(new List<VisionTorchItem> { item });
		public static void Init(WarpCoreItem item) => QSBWorldSync.Init<QSBWarpCoreItem, WarpCoreItem>(new List<WarpCoreItem> { item });
		public static void Init(OWItemSocket item) => QSBWorldSync.Init<QSBItemSocket, OWItemSocket>(new List<OWItemSocket> { item });

		public static bool HasUnityObject(MonoBehaviour behaviour) => QSBWorldSync.UnityObjectsToWorldObjects.TryGetValue(behaviour, out IWorldObject worldObject);
    }
}
