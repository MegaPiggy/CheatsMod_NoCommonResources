using OWML.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CheatsMod.CR
{
    public static class Items
    {
        private static HashSet<SlideCollectionContainer> inverted = new HashSet<SlideCollectionContainer>();

        public enum SlideReelStory
        {
            None,
            Story_1,
            Story_2,
            Story_3,
            Story_4,
            Story_5_Complete,
            Story_5_NoVessel,
            Story_5_NoInterloper,
            Story_5_NoInterloperNoVessel,
            LibraryPath_1,
            LibraryPath_2,
            LibraryPath_3,
            Seal_1,
            Seal_2,
            Seal_3,
            DreamRule_1,
            DreamRule_2_v1,
            DreamRule_2_v2,
            DreamRule_3,
            Burning,
            Experiment,
            DamageReport,
            LanternSecret,
            SupernovaEscape,
            SignalJammer,
            Homeworld,
            PrisonPeephole_Vision,
            PrisonerFarewellVision,
            TowerVision
        }

        public static void Start()
        {
            MainClass.HarmonyHelper.AddPrefix<SlideCollectionContainer>("OnTextureLoaded", typeof(Items), "onPrefixSlideCollectionContainerOnTextureLoaded");
        }

        public static void Awake()
        {
            inverted.Clear();
        }

        public static Transform GetParentBody() => MainClass.Body?.transform ?? Locator.GetAstroObject(AstroObject.Name.Sun)?.transform ?? Locator.GetAstroObject(AstroObject.Name.Eye)?.transform ?? Locator.GetPlayerBody()?.transform;

        public static void SetUpStreaming(OWItem item) => SetUpStreaming(item.gameObject);

        public static void SetUpStreaming(GameObject obj)
        {
            var assetBundlesList = new List<string>();

            var tables = Resources.FindObjectsOfTypeAll<StreamingMaterialTable>();
            foreach (var streamingHandle in obj.GetComponentsInChildren<StreamingMeshHandle>())
            {
                var assetBundle = streamingHandle.assetBundle;
                assetBundlesList.SafeAdd(assetBundle);

                if (streamingHandle is StreamingRenderMeshHandle or StreamingSkinnedMeshHandle)
                {
                    var materials = streamingHandle.GetComponent<Renderer>().sharedMaterials;

                    if (materials.Length == 0) continue;

                    foreach (var table in tables)
                    {
                        foreach (var lookup in table._materialPropertyLookups)
                        {
                            if (materials.Contains(lookup.material))
                            {
                                assetBundlesList.SafeAdd(table.assetBundle);
                            }
                        }
                    }
                }
            }

            foreach (var assetBundle in assetBundlesList)
            {
                StreamingManager.LoadStreamingAssets(assetBundle);
            }
        }

        public static ScrollItem createScroll()
        {
            // TODO
            return null;
        }

        public struct SlideReel
        {
            public SlideReelStory type;
            public bool burned;
            public string name;
            public string container;
            public bool invert = false;

            public SlideReel(SlideReelStory type, bool burned)
            {
                this.type = type;
                this.burned = burned;
                string burnS = burned ? "Burned" : "Complete";
                switch (type)
                {
                    case SlideReelStory.Story_1:
                        name = $"Reel_1_Story_{burnS}";
                        container = name;
                        break;
                    case SlideReelStory.Story_2:
                        name = $"Reel_2_Story_{burnS}";
                        container = name;
                        break;
                    case SlideReelStory.Story_3:
                        name = $"Reel_3_Story_{burnS}";
                        container = name;
                        break;
                    case SlideReelStory.Story_4:
                        name = $"Reel_4_Story_Burned";
                        container = name;
                        if (!burned)
                        {
                            container = "Reel_4_Story_Vision";
                            invert = true;
                        }
                        break;
                    case SlideReelStory.LibraryPath_1:
                        name = $"Reel_1_LibraryPath";
                        container = name;
                        break;
                    case SlideReelStory.LibraryPath_2:
                        name = $"Reel_2_LibraryPath";
                        container = name;
                        break;
                    case SlideReelStory.LibraryPath_3:
                        name = $"Reel_3_LibraryPath";
                        container = name;
                        break;
                    case SlideReelStory.Seal_1:
                        name = $"Reel_1_Seal";
                        container = name;
                        break;
                    case SlideReelStory.Seal_2:
                        name = $"Reel_2_Seal";
                        container = name;
                        break;
                    case SlideReelStory.Seal_3:
                        name = $"Reel_3_Seal";
                        container = name;
                        break;
                    case SlideReelStory.DreamRule_1:
                        name = $"Reel_1_DreamRule";
                        container = name;
                        break;
                    case SlideReelStory.DreamRule_2_v1:
                        name = $"Reel_2_DreamRule_v1";
                        container = name;
                        break;
                    case SlideReelStory.DreamRule_2_v2:
                        name = $"Reel_2_DreamRule_v2";
                        container = name;
                        break;
                    case SlideReelStory.DreamRule_3:
                        name = $"Reel_3_DreamRule";
                        container = name;
                        break;
                    case SlideReelStory.Burning:
                        name = $"Reel_Burning";
                        container = name;
                        break;
                    case SlideReelStory.Experiment:
                        if (burned)
                        {
                            name = $"Reel_ExperimentWatch_{burnS}";
                        }
                        else
                        {
                            name = $"Reel_ExperimentWatch";
                        }
                        container = name;
                        break;
                    case SlideReelStory.DamageReport:
                        name = $"Reel_DamageReport";
                        container = name;
                        break;
                    case SlideReelStory.LanternSecret:
                        name = $"Reel_LanternSecret";
                        container = name;
                        break;
                    case SlideReelStory.Story_5_Complete:
                        name = "Reel_Destroyed_8";
                        container = "Reel_5_Story_Vision_Complete";
                        invert = true;
                        break;
                    case SlideReelStory.Story_5_NoVessel:
                        name = "Reel_Destroyed_8";
                        container = "Reel_5_Story_Vision_NoVessel";
                        invert = true;
                        break;
                    case SlideReelStory.Story_5_NoInterloper:
                        name = "Reel_Destroyed_8";
                        container = "Reel_5_Story_Vision_NoInterloper";
                        invert = true;
                        break;
                    case SlideReelStory.Story_5_NoInterloperNoVessel:
                        name = "Reel_Destroyed_8";
                        container = "Reel_5_Story_Vision_NoInterloperNoVessel";
                        invert = true;
                        break;
                    case SlideReelStory.PrisonPeephole_Vision:
                        name = "Reel_Destroyed_8";
                        container = "Reel_PrisonPeephole_Vision";
                        invert = true;
                        break;
                    case SlideReelStory.PrisonerFarewellVision:
                        name = "Reel_Destroyed_8";
                        container = "Reel_PrisonerFarewellVision";
                        invert = true;
                        break;
                    case SlideReelStory.TowerVision:
                        name = "Reel_Destroyed_8";
                        container = "Reel_TowerVision";
                        invert = true;
                        break;
                    case SlideReelStory.SignalJammer:
                        name = "Reel_Destroyed_8";
                        container = "AutoProjector_SignalJammer";
                        break;
                    case SlideReelStory.Homeworld:
                        name = "Reel_Destroyed_8";
                        container = "AutoProjector_Homeworld";
                        break;
                    case SlideReelStory.SupernovaEscape:
                        name = "Reel_Destroyed_8";
                        container = "AutoProjector_SupernovaEscape";
                        break;
                    default:
                        name = "Reel_Destroyed_8";
                        container = name;
                        break;
                }
            }

            public override string ToString()
            {
                var burnS = burned ? "Burned" : "Complete";
                var invertS = invert ? "Inverted" : "Verted";
                return $"{type} {{{burnS}, {name}, {container}, {invertS}}}";
            }
        }

        public static SlideReel structFromType(SlideReelStory type, bool burned)
        {
            return new SlideReel(type, burned);
        }

        public static SlideReelItem createSlideReel(SlideReelStory type, bool burned)
        {
            var slideStruct = structFromType(type, burned);

            string name = slideStruct.name;
            string container = slideStruct.container;
            bool invert = slideStruct.invert;

            foreach (SlideReelItem reel in Resources.FindObjectsOfTypeAll<SlideReelItem>())
            {
                if (reel.name.Contains(name))
                {
                    var newReel = reel.Instantiate(GetParentBody());
                    SetVisible(newReel, true);
                    if (invert)
                    {
                        inverted.Add(newReel.slidesContainer);
                    }
                    if (!name.Equals(container))
                    {
                        newReel.slidesContainer.ClearSlides();
                        newReel.slidesContainer._changeSlidesAllowed = true;
                        HashSet<int> expendedIndex = new HashSet<int>();
                        foreach (SlideCollectionContainer collection in Resources.FindObjectsOfTypeAll<SlideCollectionContainer>())
                        {
                            if (collection.name.Contains(container))
                            {
                                newReel.slidesContainer._shipLogOnComplete = collection._shipLogOnComplete;
                                newReel.slidesContainer._autoLoadStreaming = collection._autoLoadStreaming;
                                newReel.slidesContainer._invertBlackFrames = collection._invertBlackFrames;
                                newReel.slidesContainer._musicRanges = collection._musicRanges;
                                newReel.slidesContainer.slideCollection.streamingAssetIdentifier = collection.slideCollection.streamingAssetIdentifier;
                                newReel.slidesContainer.slideCollection.SetAssetBundle(collection.slideCollection.GetAssetBundle());
                                foreach (var slide in collection.slideCollection.slides)
                                {
                                    if (!expendedIndex.Contains(slide.GetStreamingIndex()))
                                    {
                                        expendedIndex.Add(slide.GetStreamingIndex());
                                        var newSlide = new Slide(slide);
                                        newReel.slidesContainer.AddSlide(newSlide);
                                    }
                                }
                            }
                        }
                    }
                    SetUpStreaming(newReel);
                    return newReel;
                }
            }
            return null;
        }

        private static bool onPrefixSlideCollectionContainerOnTextureLoaded(ref SlideCollectionContainer __instance, ref int index, ref Texture texture)
        {
            if (inverted.Contains(__instance))
            {
                foreach (var slide in __instance.slideCollection.slides)
                {
                    if (slide.GetStreamingIndex() == index)
                    {
                        slide._image = invertColors(makeReadable(texture)) as Texture2D;
                    }
                }

                if (index == __instance.GetCurrentSlide().GetStreamingIndex())
                {
                    __instance.GetCurrentSlide().InvokeTextureUpdate();
                }

                return false;
            }

            return true;
        }

        private static Texture makeReadable(Texture texture)
        {
            if (!texture.isReadable)
            {
                RenderTexture renderTex = RenderTexture.GetTemporary(
                            texture.width,
                            texture.height,
                            0,
                            RenderTextureFormat.Default,
                            RenderTextureReadWrite.Linear);

                Graphics.Blit(texture, renderTex);
                RenderTexture previous = RenderTexture.active;
                RenderTexture.active = renderTex;
                Texture2D readableText = new Texture2D(texture.width, texture.height);
                readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
                readableText.Apply();
                RenderTexture.active = previous;
                RenderTexture.ReleaseTemporary(renderTex);

                return readableText;
            }

            return texture;
        }

        private static Texture invertColors(Texture texture)
        {
            Texture2D newTexture = new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, false);
            var colors = (texture as Texture2D).GetPixels();
            var newColors = new Color[colors.Length];
            for (int i = 0; i < colors.Length; i++)
            {
                var color = colors[i];
                newColors[i] = new Color(1f - color.r, 1f - color.g, 1f - color.b, color.a);
            }
            newTexture.SetPixels(newColors);
            newTexture.Apply();

            return newTexture;
        }

        public static SharedStone createShareStone(NomaiRemoteCameraPlatform.ID type)
        {
            Transform parent = GetParentBody();
            foreach (SharedStone stone in Resources.FindObjectsOfTypeAll<SharedStone>())
            {
                if (stone.GetRemoteCameraID().Equals(type))
                {
                    var newStone = stone.Instantiate(parent);
                    SetVisible(newStone, true);
                    SetUpStreaming(newStone);
                    return newStone;
                }
            }
            GameObject fallbackStone = new GameObject("ShareStoneFallback_" + type.ToString());
            fallbackStone.transform.parent = parent;
            fallbackStone.SetActive(false);
            SphereCollider sc = fallbackStone.AddComponent<SphereCollider>();
            sc.center = Vector3.zero;
            sc.radius = 0.4f;
            sc.isTrigger = false;
            OWCollider owc = fallbackStone.AddComponent<OWCollider>();
            owc._collider = sc;
            SharedStone item = fallbackStone.AddComponent<SharedStone>();
            item._connectedPlatform = type;
            item._animDuration = 0.4f;
            item._animOffsetY = 0.08f;
            GameObject animRoot = new GameObject("AnimRoot");
            animRoot.transform.parent = fallbackStone.transform;
            TransformAnimator transformAnimator = animRoot.AddComponent<TransformAnimator>();
            item._animator = transformAnimator;
            OWRenderer renderer = Resources.FindObjectsOfTypeAll<OWRenderer>().FirstOrDefault(renderer => renderer.gameObject.name == "Props_NOM_SharedStone");
            if (renderer != null)
            {
                GameObject.Instantiate(renderer.gameObject, animRoot.transform);
            }
            fallbackStone.SetActive(true);
            SetUpStreaming(fallbackStone);
            return item;
        }

        public static VisionTorchItem createVisionTorch()
        {
            foreach (VisionTorchItem torch in Resources.FindObjectsOfTypeAll<VisionTorchItem>())
            {
                var newTorch = torch.Instantiate(GetParentBody());
                SetVisible(newTorch, true);
                foreach (Behaviour behaviour in newTorch.GetComponentsInChildren<Behaviour>())
                    behaviour.enabled = true;
                SetUpStreaming(newTorch);
                return newTorch;
            }
            return null;
        }

        public static NomaiConversationStone createConversationStone(NomaiWord type)
        {
            foreach (NomaiConversationStone stone in Resources.FindObjectsOfTypeAll<NomaiConversationStone>())
            {
                if (stone.GetWord().Equals(type))
                {
                    var newStone = stone.Instantiate(GetParentBody());
                    newStone.Reveal();
                    SetUpStreaming(newStone);
                    return newStone;
                }
            }
            return null;
        }

        public static SimpleLanternItem createLantern(bool broken, bool lit)
        {
            foreach (SimpleLanternItem lantern in Resources.FindObjectsOfTypeAll<SimpleLanternItem>())
            {
                if (lantern.IsInteractable()
                    && (broken == lantern.name.Contains("BROKEN")))
                {
                    var newLantern = lantern.Instantiate(GetParentBody());
                    newLantern.name = broken ? "Prefab_IP_BrokenLanternItem" : "Prefab_IP_SimpleLanternItem";
                    SetVisible(newLantern, true);
                    newLantern._startsLit = lit;
                    newLantern._lit = lit;
                    newLantern._held = false;
                    if (newLantern._collisionChecker != null)
                    {
                        newLantern._collisionChecker.OnEnterCustomCollider = new OWEvent(32);
                    }
                    if (newLantern._extinguishElectricityEffect != null)
                    {
                        MainClass.Instance.ModHelper.Events.Unity.FireOnNextUpdate(() =>
                            newLantern._extinguishElectricityEffect._electricityRenderer.SetActivation(false));
                    }
                    newLantern.Awake();
                    newLantern.Start();

                    SetUpStreaming(newLantern);
                    return newLantern;
                }
            }
            return null;
        }

        public static DreamLanternItem createDreamLantern(DreamLanternType type, bool lit)
        {
            foreach (DreamLanternItem lantern in Resources.FindObjectsOfTypeAll<DreamLanternItem>())
            {
                if (lantern.IsInteractable()
                    && lantern.GetLanternType().Equals(type))
                {
                    DreamLanternController sourceController = lantern.GetComponent<DreamLanternController>();
                    if (sourceController != null)
                    {
                        lantern._lanternController = sourceController;
                        sourceController.enabled = true;
                    }
                    lantern.enabled = true;
                    var parentBody = GetParentBody();
                    var parentSector = parentBody.GetComponentInChildren<Sector>();
                    var newLantern = lantern.Instantiate(parentBody);
                    SetVisible(newLantern, true);
                    newLantern.SetSector(parentSector);
                    newLantern.enabled = true;
                    DreamLanternController newController = newLantern.GetComponent<DreamLanternController>();
                    if (sourceController != null && newController != null)
                    {
                        newLantern._lanternController = newController;
                        newController.enabled = true;
                        newController._focuserPetalsBaseEulerAngles = sourceController._focuserPetalsBaseEulerAngles;
                        newController._concealerRootsBaseScale = sourceController._concealerRootsBaseScale;
                        newController._concealerCoversStartPos = sourceController._concealerCoversStartPos;
                        if (type == DreamLanternType.Functioning)
                        {
                            MainClass.ModHelperInstance.Events.Unity.FireOnNextUpdate(() =>
                            {
                                try
                                {
                                    newLantern.enabled = true;
                                    newController.enabled = true;
                                    newController.SetLit(MainClass.Instance.inDreamWorld);
                                    newController.SetFocus(1);
                                    newController.SetConcealed(false);
                                    newController.UpdateVisuals();
                                }
                                catch (Exception ex)
                                {
                                    MainClass.Console.WriteLine(ex.ToString(), MessageType.Error);
                                }
                            });
                        }
                        else if (type != DreamLanternType.Malfunctioning)
                        {
                            newLantern.SetLit(MainClass.Instance.inDreamWorld);
                        }
                    }
                    SetUpStreaming(newLantern);
                    return newLantern;
                }
            }
            return null;
        }

        public static WarpCoreItem createWarpCore(WarpCoreType type)
        {
            foreach (WarpCoreItem core in Resources.FindObjectsOfTypeAll<WarpCoreItem>())
            {
                if (core.GetWarpCoreType().Equals(type))
                {
                    var newCore = core.Instantiate(GetParentBody());
                    SetVisible(newCore, true);
                    SetUpStreaming(newCore);
                    return newCore;
                }
            }
            return null;
        }

        private static void SetVisible(OWItem item, bool visible)
        {
            item._visible = visible;
            foreach (OWRenderer render in item._renderers)
            {
                if (render)
                {
                    render.enabled = true;
                    render.SetActivation(true);
                    render.SetLODActivation(visible);
                    if (render.GetRenderer())
                    {
                        render.GetRenderer().enabled = true;
                    }
                }
            }
            foreach (ParticleSystem particleSystem in item._particleSystems)
            {
                if (particleSystem)
                {
                    if (visible)
                        particleSystem.Play(true);
                    else
                        particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                }
            }
            foreach (OWLight2 light in item._lights)
            {
                if (light)
                {
                    light.enabled = true;
                    light.SetActivation(true);
                    light.SetLODActivation(visible);
                    if (light.GetLight())
                    {
                        light.GetLight().enabled = true;
                    }
                }
            }
        }
    }
}
