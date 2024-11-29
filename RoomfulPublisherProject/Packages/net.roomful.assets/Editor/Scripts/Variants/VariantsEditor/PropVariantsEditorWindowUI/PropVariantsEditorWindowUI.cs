using System;
using System.Linq;
using System.Reflection;
using StansAssets.Foundation.UIElements;
using StansAssets.Plugins.Editor;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace net.roomful.assets.editor
{
    internal class PropVariantsEditorWindowUI : BaseTab
    {
        private readonly VisualElement m_variantsListBlock;
        private readonly VisualElement m_variantInfoBlock;
        private readonly VisualElement m_skinInfoBlock;
        private readonly VisualElement m_noPropAsset;
        private readonly VisualElement m_variantsList;
        private readonly VisualElement m_loadingSpinnerSkin;
        private readonly VisualElement m_loadingSpinnerVariant;
        private readonly VisualElement m_skinInfoButtons;
        private readonly VisualElement m_skinInfoButtonDone;

        private VisualElement m_skinsList;
        private TextField m_propVariantName;
        private IntegerField m_propVariantSortingOrder;
        private VisualElement m_propVariantId;
        private VisualElement m_propSkinId;
        private VisualElement m_platforms;
        private TextField m_propSkinName;
        private IntegerField m_propSkinSortOrder;
        private ObjectField m_propVariantIcon;
        private ObjectField m_propSkinIcon;
        private VisualElement m_variantInfo;
        private VisualElement m_skinListHeader;
        private VisualElement m_skinInfo;
        private VisualElement m_hierarchyTreeHeader;
        private VisualElement m_hierarchyTree;
        private IMGUIContainer m_hierarchyTreeIMGUI;
        private Toggle m_propSkinDefaultToggle;
        private Toggle m_propSkinHiddenToggle;
        private Toggle m_propSkinHeavyToggle;
        private Toggle m_propVariantHasColorSupport;
        private Toggle m_propVariantIsHidden;
        private ColorField m_defaultColor;

        private Toggle m_colorOnlySkinToggle;
        private ColorField m_skinColorOverride;

        private PropAsset m_asset;
        private bool m_useVariantEditorCameraPosition;
        private bool m_useEditorCameraPositionSkin;
        private bool m_disableSkinUnrelatedRenderersForScreenshot;
        private VariantHierarchyView m_hierarchyView;

        public PropSkin SelectedSkin { get; private set; }
        public PropVariant SelectedVariant { get; private set; }

        public Action<GUIContent> ShowNotification = delegate { };
        public Action<Action<bool, string>> ShowCreateNewPropVariant = delegate { };
        public Action<Action<bool, string>> ShowCreateNewSkinVariant = delegate { };

        private enum SearchType
        {
            FILTERMODE_ALL = 0,
            FILTERMODE_NAME = 1,
            FILTERMODE_TYPE = 2
        }

        public PropVariantsEditorWindowUI() :
            base($"{VariantsPackage.VariantsEditorUIPath}/PropVariantsEditorWindowUI/PropVariantsEditorWindowUI") {
            /* var element = new VisualElement();
             element.Add(EditorGUILayout.ObjectField());*/
            m_variantsListBlock = this.Q<VisualElement>("VariantsListBlock");
            m_variantInfoBlock = this.Q<VisualElement>("VariantInfoBlock");
            m_skinInfoBlock = this.Q<VisualElement>("SkinInfoBlock");
            m_noPropAsset = this.Q<VisualElement>("NoPropAsset");
            m_variantsList = this.Q<VisualElement>("VariantsList");

            m_skinInfoButtons = this.Q<VisualElement>("SkinInfoButtons");
            m_skinInfoButtonDone = this.Q<VisualElement>("SkinInfoButtonDone");

            //loadingSpinner
            m_loadingSpinnerVariant = this.Q<VisualElement>("LoadingSpinnerVariant");
            m_loadingSpinnerVariant.style.display = DisplayStyle.None;
            m_loadingSpinnerSkin = this.Q<VisualElement>("LoadingSpinnerSkin");
            m_loadingSpinnerSkin.style.display = DisplayStyle.None;
            //Init Add/Update buttons
            InitAddUpdateButtons();
            //PropVariantInfo
            InitPropVariantInfo();
            //PropSkinInfo
            InitPropSkinInfo();

            m_noPropAsset.style.display = DisplayStyle.Flex;
            m_variantsListBlock.style.display = DisplayStyle.None;
            m_variantInfoBlock.style.display = DisplayStyle.None;
            m_skinInfoBlock.style.display = DisplayStyle.None;
        }

        internal void Recreate(PropAsset asset) {
            if (SelectedSkin != null && m_propSkinIcon.value != SelectedSkin.Thumbnail) {
                m_propSkinIcon.SetValueWithoutNotify(SelectedSkin.Thumbnail);
            }

            if (SelectedVariant != null && m_propVariantIcon.value != SelectedVariant.Thumbnail) {
                m_propVariantIcon.SetValueWithoutNotify(SelectedVariant.Thumbnail);
            }

            if (m_asset == asset) return;
            if (asset != null && SelectedSkin != null && asset.Template.Id == SelectedSkin.Id) return;

            m_asset = asset;
            SelectedVariant = null;
            SelectedSkin = null;

            if (asset == null) {
                m_noPropAsset.style.display = DisplayStyle.Flex;
                m_variantsListBlock.style.display = DisplayStyle.None;
                m_variantInfoBlock.style.display = DisplayStyle.None;
                m_skinInfoBlock.style.display = DisplayStyle.None;
            }
            else {
                m_noPropAsset.style.display = DisplayStyle.None;
                m_variantsListBlock.style.display = DisplayStyle.Flex;
                m_variantInfoBlock.style.display = DisplayStyle.Flex;
                m_skinInfoBlock.style.display = DisplayStyle.Flex;
                m_skinInfoButtonDone.style.display = DisplayStyle.None;

                VisualizationPropVariantInfo(DisplayStyle.None);
                VisualizationPropSkinInfo(DisplayStyle.None);
                if (string.IsNullOrEmpty(m_asset.Template.Id)) {
                    RefreshVariantsWithError("The prop has to be uploaded before we can create a variant");
                }
                else {
                    RefreshVariants();
                }
            }
        }

        private void InitAddUpdateButtons() {
            var variantListUpdate = this.Q<Button>("VariantListUpdate");
            InitUpdateButton(variantListUpdate, RefreshVariants);
            var variantListAdd = this.Q<Button>("VariantListAdd");
            variantListAdd.clicked += AddVariant;
            var skinListUpdate = this.Q<Button>("SkinListUpdate");
            InitUpdateButton(skinListUpdate, RefreshSkins);
            var skinListAdd = this.Q<Button>("SkinListAdd");
            skinListAdd.clicked += AddSkin;
        }

        private void InitPropVariantInfo() {
            var variantRemove = this.Q<Button>("PropVariantRemove");
            variantRemove.clicked += RemoveVariant;
            var variantSave = this.Q<Button>("PropVariantSave");
            variantSave.clicked += SaveVariant;
            m_propVariantName = this.Q<TextField>("PropVariantName");
            m_propVariantName.RegisterValueChangedCallback(e => {
                SelectedVariant.Name = e.newValue;
            });

            m_propVariantSortingOrder = this.Q<IntegerField>("PropVariantSortingOrder");
            m_propVariantSortingOrder.RegisterValueChangedCallback(e => {
                SelectedVariant.SortOrder = e.newValue;
            });

            m_propVariantHasColorSupport = this.Q<Toggle>("PropVariantHasColorSupport");
            m_propVariantHasColorSupport.Children().FirstOrDefault()?.AddToClassList("toggle-label");
            m_propVariantHasColorSupport.RegisterValueChangedCallback(e => {
                SelectedVariant.HasColorSupport = e.newValue;
                ShowDefaultColor(e.newValue);
            });
            
            m_propVariantIsHidden = this.Q<Toggle>("PropVariantIsHidden");
            m_propVariantIsHidden.Children().FirstOrDefault()?.AddToClassList("toggle-label");
            m_propVariantIsHidden.RegisterValueChangedCallback(e => {
                SelectedVariant.IsHidden = e.newValue;
            });


            m_defaultColor = this.Q<ColorField>("DefaultSkinColor");
            m_defaultColor.RegisterValueChangedCallback(e => {
                UpdateVariantColor(e.newValue);
            });

            var children = m_defaultColor.Children().ToList();
            if (children.Count > 1) {
                children[0].AddToClassList("toggle-label");
                children[1].AddToClassList("default-color-imgui");
            }

            m_propVariantIcon = this.Q<ObjectField>("PropVariantIcon");
            m_propVariantIcon.objectType = typeof(Texture2D);
            var image = m_propVariantIcon.Children().FirstOrDefault()?.Children().FirstOrDefault()?.Children().FirstOrDefault();
            if (image != null) {
                image.style.width = new StyleLength(70);
                image.style.maxHeight = new StyleLength(StyleKeyword.Auto);
                image.style.maxWidth = new StyleLength(StyleKeyword.Auto);
                image.style.height = new StyleLength(70);
            }

            m_propVariantIcon.RegisterValueChangedCallback(e => {
                // SelectedVariant.Thumbnail = (Texture2D) e.newValue;
                SelectedVariant.Thumbnail = ConvertTexture2D((Texture2D) e.newValue, SelectedVariant.Thumbnail);
            });
            m_propVariantId = this.Q<VisualElement>("PropVariantId");
            m_variantInfo = this.Q<VisualElement>("VariantInfo");
            m_skinListHeader = this.Q<VisualElement>("SkinListHeader");
            m_skinsList = this.Q<VisualElement>("SkinsList");
            var propVariantIconToggle = this.Q<Toggle>("PropVariantIconToggle");
            propVariantIconToggle.Children().FirstOrDefault()?.AddToClassList("toggle-label");
            propVariantIconToggle.RegisterValueChangedCallback(e => {
                m_useVariantEditorCameraPosition = e.newValue;
            });
            var propVariantIconButton = this.Q<Button>("PropVariantIconButton");
            propVariantIconButton.clicked += CreateVariantPreviewIcon;
        }

        private Button m_skinEdit;
        private Button m_skinDownload;
        private Button m_skinSave;

        private void InitPropSkinInfo() {
            var skinRemove = this.Q<Button>("PropSkinRemove");
            skinRemove.clicked += RemoveSkin;
            m_skinEdit = this.Q<Button>("PropSkinEdit");
            m_skinEdit.clicked += AddHierarchyFilterType;

            m_skinDownload = this.Q<Button>("PropSkinDownload");
            m_skinDownload.clicked += DownloadSkin;

            var skinUpdateMeta = this.Q<Button>("PropSkinUpdateMeta");
            skinUpdateMeta.clicked += () =>
            {
                UpdateMetaSkin(true);
            };

            m_skinSave = this.Q<Button>("PropSkinSave");
            m_skinSave.clicked += SaveSkin;

            var skinDone = this.Q<Button>("PropSkinDone");
            skinDone.clicked += ClearHierarchyFilterType;
            m_propSkinName = this.Q<TextField>("PropSkinName");
            m_propSkinName.RegisterValueChangedCallback(e => {
                SelectedSkin.Name = e.newValue;
            });


            m_propSkinSortOrder = this.Q<IntegerField>("PropSkinNameSortOrder");
            m_propSkinSortOrder.RegisterValueChangedCallback(e => {
                SelectedSkin.SortOrder = e.newValue;
            });

            m_propSkinId = this.Q<VisualElement>("PropSkinId");
            m_platforms = this.Q<VisualElement>("Platforms");
            m_propSkinIcon = this.Q<ObjectField>("PropSkinIcon");
            m_propSkinIcon.objectType = typeof(Texture2D);
            var image = m_propSkinIcon.Children().FirstOrDefault()?.Children().FirstOrDefault()?.Children().FirstOrDefault();
            if (image != null) {
                image.style.width = new StyleLength(70);
                image.style.maxHeight = new StyleLength(StyleKeyword.Auto);
                image.style.maxWidth = new StyleLength(StyleKeyword.Auto);
                image.style.height = new StyleLength(70);
            }

            m_propSkinIcon.RegisterValueChangedCallback(e => {
                SelectedSkin.Thumbnail = ConvertTexture2D((Texture2D) e.newValue, SelectedSkin.Thumbnail);
            });

            m_skinInfo = this.Q<VisualElement>("SkinInfo");
            m_hierarchyTreeHeader = this.Q<VisualElement>("HierarchyTreeHeader");
            m_hierarchyTree = this.Q<VisualElement>("HierarchyTree");
            m_hierarchyTreeIMGUI = this.Q<IMGUIContainer>("HierarchyTreeIMGUI");
            var propSkinCameraToggle = this.Q<Toggle>("PropSkinCameraToggle");
            propSkinCameraToggle.Children().FirstOrDefault()?.AddToClassList("toggle-label-camera-skin");
            propSkinCameraToggle.RegisterValueChangedCallback(e => {
                m_useEditorCameraPositionSkin = e.newValue;
            });
            var propSkinUnrelatedObjectsToggle = this.Q<Toggle>("PropSkinUnrelatedObjectsToggle");
            propSkinUnrelatedObjectsToggle.Children().FirstOrDefault()?.AddToClassList("toggle-label");
            propSkinUnrelatedObjectsToggle.RegisterValueChangedCallback(e => {
                m_disableSkinUnrelatedRenderersForScreenshot = e.newValue;
            });
            var propSkinIconButton = this.Q<Button>("PropSkinIconButton");
            propSkinIconButton.clicked += CreateSkinPreviewIcon;
            m_propSkinDefaultToggle = this.Q<Toggle>("PropSkinDefaultToggle");
            m_propSkinDefaultToggle.RegisterValueChangedCallback(e => {
                SelectedSkin.IsDefault = e.newValue;
            });
            
            m_propSkinHiddenToggle  = this.Q<Toggle>("PropSkinHiddenToggle");
            m_propSkinHiddenToggle.RegisterValueChangedCallback(e => {
                SelectedSkin.IsHidden = e.newValue;
            });
            
            m_propSkinHeavyToggle = this.Q<Toggle>("PropSkinHeavyToggle");
            m_propSkinHeavyToggle.RegisterValueChangedCallback(e => {
                SelectedSkin.HeavySkin = e.newValue;
            });

            m_colorOnlySkinToggle = this.Q<Toggle>("ColorOnlySkinToggle");
            m_colorOnlySkinToggle.RegisterValueChangedCallback(e => {
                SelectedSkin.ColorOnly = e.newValue;
                ShowColorOverride(e.newValue);
            });

            m_skinColorOverride = this.Q<ColorField>("SkinColorOverride");
            m_skinColorOverride.RegisterValueChangedCallback(e => {
                UpdateSkinColorOverride(e.newValue);
            });

            var propSkinDebugToggle = this.Q<Toggle>("PropSkinDebugToggle");
            propSkinDebugToggle.SetValueWithoutNotify(VariantsService.DebugMode);
            propSkinDebugToggle.RegisterValueChangedCallback(e => {
                VariantsService.DebugMode = e.newValue;
                Debug.Log("Debug mode set: " + VariantsService.DebugMode);
            });

            EditorApplication.hierarchyChanged += RecreateHierarchyView;
        }

        private void RecreateHierarchyView() {
            if (SelectedVariant == null || SelectedSkin == null || m_asset == null) return;
            var customizableSkinGameObjects = CustomizableSkinGameObject.SearchCustomizableSkinGameObjects(m_asset, SelectedVariant);
            m_hierarchyView.ChangedCustomizableSkinGameObjects(customizableSkinGameObjects);
        }

        private static void InitUpdateButton(Button button, Action callback) {
            var imageName = EditorGUIUtility.isProSkin ? "refresh" : "refresh_black";
            var image = new Image { image = Resources.Load(imageName) as Texture };
            button.Add(image);
            button.tooltip = "Refresh";
            button.clicked += callback;
        }

        private void RefreshVariants() {
            m_loadingSpinnerVariant.style.display = DisplayStyle.Flex;
            m_variantsList.Clear();
            VariantsService.LoadVariantsList(m_asset.Template.Id, () => {
                m_loadingSpinnerVariant.style.display = DisplayStyle.None;
                if (!VariantsService.Variants.Any()) {
                    RefreshVariantsWithError("No variants found, but you can add new ones");
                    return;
                }

                CreateVariantItems();
            }, () => {
                RefreshVariantsWithError("Searching for variants failed, but you can add new options or update variants");
            });
        }

        private void RefreshVariantsWithError(string text) {
            m_loadingSpinnerVariant.style.display = DisplayStyle.None;
            m_loadingSpinnerSkin.style.display = DisplayStyle.None;
            var label = new Label { text = text };
            label.AddToClassList("refresh-variants-error");
            m_variantsList.Add(label);
            VisualizationPropVariantInfo(DisplayStyle.None);
            VisualizationPropSkinInfo(DisplayStyle.None);
        }

        private void AddVariant() {
            var selection = Selection.objects.Where(o => o != null && o is GameObject && !EditorUtility.IsPersistent(o))
                .Select(o => (GameObject) o).ToList();

            if (selection.Count == 0) {
                ShowNotification(new GUIContent("Variant Creation Failed! Nothing selected"));
            }
            else {
                ShowCreateNewPropVariant((isSuccessful, variantName) => {
                    if (!isSuccessful) return;
                    VariantsService.CreateVariant(variantName, m_asset, selection, SelectVariant);
                });
            }
        }

        private void RefreshSkins() {
            m_loadingSpinnerSkin.style.display = DisplayStyle.Flex;
            m_skinsList.Clear();
            SelectedSkin = null;
            VariantsService.GetSkinsList(m_asset.Template.Id, SelectedVariant.Id, list => {
                m_loadingSpinnerSkin.style.display = DisplayStyle.None;
                if (!list.Any()) {
                    if (!SelectedVariant.Skins.Any())
                        VisualizationPropSkinInfo(DisplayStyle.None);
                    return;
                }

                SelectedVariant.SetSkins(list);
                SelectSkin(SelectedVariant.Skins.FirstOrDefault());
            });
        }

        private void AddSkin() {
            if (SelectedVariant != null) {
                ShowCreateNewSkinVariant((isSuccessful, variantName) => {
                    if (isSuccessful)
                        VariantsService.CreateSkin(variantName, SelectedVariant.Id, m_asset.Template.Id, skin => {
                            SelectedVariant.AddSkin(skin);
                            SelectSkin(skin);
                        });
                });
            }
        }

        private void CreateVariantItems() {
            m_variantsList.Clear();
            foreach (var variant in VariantsService.Variants) {
                var variantItem = new Button(() => {
                    SelectVariant(variant);
                }) { text = variant.Name };
                variantItem.AddToClassList("variant-item");
                if (SelectedVariant?.Id == variant.Id) {
                    variantItem.AddToClassList("variant-item-selected");
                }

                m_variantsList.Add(variantItem);
            }
        }

        private void SelectVariant(PropVariant variant) {
            if (SelectedVariant?.Id == variant?.Id)
                return;

            SelectedVariant = variant;
            CreateVariantItems();
            PropVariantInfo();
        }

        private void CreateSkinItems() {
            m_skinsList.Clear();
            foreach (var skin in SelectedVariant.Skins) {
                var skinItem = new Button(() => {
                    SelectSkin(skin);
                }) { text = skin.Name };
                skinItem.AddToClassList("variant-item");
                if (SelectedSkin?.Id == skin.Id) {
                    skinItem.AddToClassList("variant-item-selected");
                }

                m_skinsList.Add(skinItem);
            }
        }

        private void SelectSkin(PropSkin skin) {
            if (skin?.Id == SelectedSkin?.Id) return;
            SelectedSkin = skin;

            var customizableSkinGameObjects = CustomizableSkinGameObject.SearchCustomizableSkinGameObjects(m_asset, SelectedVariant);
            if (m_hierarchyView != null) {
                m_hierarchyView.OnCustomizableSkinGameObjectChange = null;
            }

            m_hierarchyView = new VariantHierarchyView(new TreeViewState(), SelectedVariant, SelectedSkin, customizableSkinGameObjects);
            m_hierarchyView.OnCustomizableSkinGameObjectChange += RecreateHierarchyView;

            CreateSkinItems();
            PropSkinInfo();
        }

        private void PropVariantInfo() {
            if (SelectedVariant == null) return;

            VisualizationPropVariantInfo(DisplayStyle.Flex);

            m_propVariantId.Clear();
            m_propVariantId.Add(new SelectableLabel() { text = $"Id: {SelectedVariant.Id}" });

            m_propVariantName.value = SelectedVariant.Name;
            m_propVariantIcon.SetValueWithoutNotify(SelectedVariant.Thumbnail);
            m_propVariantSortingOrder.SetValueWithoutNotify(SelectedVariant.SortOrder);

            RefreshSkins();
            m_propVariantHasColorSupport.value = SelectedVariant.HasColorSupport;
            m_propVariantIsHidden.value = SelectedVariant.IsHidden;

            ShowDefaultColor(SelectedVariant.HasColorSupport);
            m_defaultColor.value = SelectedVariant.DefaultColor;
            UpdateVariantColor(SelectedVariant.DefaultColor);
        }

        private void PropSkinInfo() {
            if (SelectedSkin == null) return;

            VisualizationPropSkinInfo(DisplayStyle.Flex);
            m_colorOnlySkinToggle.value = SelectedSkin.ColorOnly;
            ShowColorOverride(m_colorOnlySkinToggle.value);

            m_skinColorOverride.value = SelectedSkin.OverrideColor;
            UpdateSkinColorOverride(SelectedSkin.OverrideColor);

            m_propSkinId.Clear();
            m_propSkinId.Add(new SelectableLabel { text = $"Id: {SelectedSkin.Id}" });
            m_propSkinName.value = SelectedSkin.Name;
            m_propSkinSortOrder.SetValueWithoutNotify(SelectedSkin.SortOrder);
            m_platforms.Clear();
            m_platforms.Add(new SelectableLabel { text = string.Concat("Platforms: ", string.Join(",", SelectedSkin.AvailablePlatforms)) });
            m_propSkinDefaultToggle.value = SelectedSkin.IsDefault;
            m_propSkinHiddenToggle.value = SelectedSkin.IsHidden;
            m_propSkinHeavyToggle.value = SelectedSkin.HeavySkin;
            m_propSkinIcon.SetValueWithoutNotify(SelectedSkin.Thumbnail);
            RefreshTree();
        }

        private void RemoveVariant() {
            if (!VariantsService.Variants.Any() || SelectedVariant == null || m_asset == null) return;
            var dialog = EditorUtility.DisplayDialog("Confirm",
                $"Are you sure want to remove '{SelectedVariant.Name}' variant?",
                "Yes",
                "No");
            if (!dialog) return;
            VariantsService.DeleteVariant(m_asset.Template.Id, SelectedVariant, () => {
                SelectNextVariant(SelectedVariant);
                RefreshVariants();
            });
        }

        private void SaveVariant() {
            if (!VariantsService.Variants.Any() || SelectedVariant == null || m_asset == null) return;

            VariantsService.SaveVariantMeta(m_asset.Template.Id, SelectedVariant);
        }

        private void RemoveSkin() {
            if (SelectedVariant == null || !SelectedVariant.Skins.Any() || SelectedSkin == null) return;
            var dialog = EditorUtility.DisplayDialog("Confirm",
                $"Are you sure want to remove '{SelectedSkin.Name}' skin?",
                "Yes",
                "No");
            if (!dialog) return;
            VariantsService.DeleteSkin(m_asset.Template.Id, SelectedSkin, (s) => {
                SelectNextSkin(SelectedVariant, s);
                SelectedVariant.RemoveSkin(s);
                CreateSkinItems();
            });
        }

        private void DownloadSkin() {
            if (m_asset == null || SelectedSkin == null) return;
            BundleService.DownloadSkin(m_asset, SelectedVariant, SelectedSkin);
            PropSkinInfo();
        }

        private void SaveSkin() {
            if (m_asset == null || SelectedSkin == null || SelectedVariant == null) return;

            var dialog = EditorUtility.DisplayDialog("Confirm",
                $"Are you sure want to upload '{m_propSkinName.text}' skin?",
                "Yes",
                "No");
            if (!dialog) return;

            UpdateMetaSkin(false);
            BundleService.UploadSkin(SelectedVariant, SelectedSkin, m_asset);
        }

        private void UpdateMetaSkin(bool showSavedMessage) {
            if (SelectedVariant == null || !SelectedVariant.Skins.Any() || SelectedSkin == null) return;
            SelectedSkin.HeavySkin = CustomizableSkinGameObject.IsHeavySkinSkin(m_asset, SelectedSkin);
            VariantsService.SaveSkinMeta(m_asset.Template.Id, SelectedSkin, showSavedMessage);
        }

        private void SelectNextVariant(PropVariant selectedVariant) {
            var nextVariantExist = false;
            PropVariant previousVariant = null;
            PropVariant variant = null;
            foreach (var next in VariantsService.Variants) {
                if (nextVariantExist) {
                    variant = next;
                    break;
                }

                if (next.Id == selectedVariant.Id) {
                    nextVariantExist = true;
                }
                else {
                    previousVariant = next;
                }
            }

            if (variant == null && previousVariant != null) {
                variant = previousVariant;
            }

            if (variant != null) {
                SelectVariant(variant);
            }
        }

        private void SelectNextSkin(PropVariant variant, PropSkin selectedSkin) {
            var firstSkin = variant.Skins.FirstOrDefault();
            var nextSkinExist = false;
            PropSkin previousSkin = null;
            PropSkin skin = null;
            foreach (var next in variant.Skins) {
                if (nextSkinExist) {
                    skin = next;
                    break;
                }

                if (next == selectedSkin) {
                    nextSkinExist = true;
                }
                else {
                    previousSkin = next;
                }
            }

            switch (skin) {
                case null when previousSkin != null:
                    skin = previousSkin;
                    break;
                case null:
                    skin = firstSkin;
                    break;
            }

            if (skin != null && skin != selectedSkin) {
                SelectSkin(skin);
            }
            else {
                VisualizationPropSkinInfo(DisplayStyle.None);
            }
        }

        private void CreateVariantPreviewIcon() {
            PropAssetScreenshotTool.CreateIcon(m_useVariantEditorCameraPosition, m_asset, out var selectedVariantIcon);
            m_propVariantIcon.value = selectedVariantIcon;
        }

        private void CreateSkinPreviewIcon() {
            if (m_disableSkinUnrelatedRenderersForScreenshot) {
                CustomizableSkinGameObject.DisableSkinUnrelatedRenderers(m_asset, SelectedVariant);
            }

            PropAssetScreenshotTool.CreateIcon(m_useEditorCameraPositionSkin, m_asset, out var selectedSkinIcon);
            m_propSkinIcon.value = selectedSkinIcon;
            foreach (var rend in m_asset.Renderers) {
                rend.enabled = true;
            }
        }

        private void VisualizationPropVariantInfo(DisplayStyle displayStyle) {
            m_variantInfo.style.display = displayStyle;
            m_skinListHeader.style.display = displayStyle;
            m_skinsList.style.display = displayStyle;
        }

        private void VisualizationPropSkinInfo(DisplayStyle displayStyle) {
            m_skinInfo.style.display = displayStyle;
            m_hierarchyTreeHeader.style.display = displayStyle;
            m_hierarchyTree.style.display = displayStyle;
        }

        private void AddHierarchyFilterType() {
            ShowButtonDone(true);
            var listGO = SelectedVariant.GameObjects.Select(item => item.gameObject).ToList();
            listGO.ForEach(item => {
                var editSkinObject = item.GetComponent<EditSkinObject>();
                if (editSkinObject == null) {
                    item.AddComponent<EditSkinObject>();
                }
            });
            SetSearchFilter("t: " + nameof(EditSkinObject), SearchType.FILTERMODE_ALL);
        }

        private void ShowButtonDone(bool show) {
            m_skinInfoButtonDone.style.display = (show) ? DisplayStyle.Flex : DisplayStyle.None;
            m_skinInfoButtons.style.display = (show) ? DisplayStyle.None : DisplayStyle.Flex;
        }

        private static void SetSearchFilter(string filter, SearchType filterMode) {
            SearchableEditorWindow hierarchy = null;
            SearchableEditorWindow[] windows = (SearchableEditorWindow[]) Resources.FindObjectsOfTypeAll(typeof(SearchableEditorWindow));
            foreach (SearchableEditorWindow window in windows) {
                if (window.GetType().ToString() == "UnityEditor.SceneHierarchyWindow") {
                    hierarchy = window;
                    break;
                }
            }

            if (hierarchy == null) {
                return;
            }

            var setSearchType = typeof(SearchableEditorWindow).GetMethod("SetSearchFilter", BindingFlags.NonPublic | BindingFlags.Instance);
            var parameters = new object[] { filter, (int) filterMode, true, false };
            setSearchType.Invoke(hierarchy, parameters);
        }

        private void ClearHierarchyFilterType() {
            ShowButtonDone(false);
            var listGO = SelectedVariant.GameObjects.Select(item => item.gameObject).ToList();
            listGO.ForEach(item => {
                Object.DestroyImmediate(item.GetComponent<EditSkinObject>());
            });
            SetSearchFilter(string.Empty, SearchType.FILTERMODE_ALL);
        }

        private void RefreshTree() {
            var rect = new Rect(0.0f, 0.0f, Screen.width, Screen.height);
            rect = new Rect(rect.x + rect.width, rect.y, Screen.width, Screen.height);
            var localRect = new Rect(0.0f, 0.0f, rect.width * 1.75f, rect.height);
            m_hierarchyTreeIMGUI.onGUIHandler = null;
            m_hierarchyTreeIMGUI.onGUIHandler += () => {
                m_hierarchyView?.OnGUI(localRect);
            };
        }

        private void UpdateVariantColor(Color color) {
            SelectedVariant.DefaultColor = color;
            if (SelectedSkin != null) {
                if (!SelectedSkin.ColorOnly)
                    ApplySkinColor(color);
            }
            else {
                ApplySkinColor(color);
            }
        }

        private void UpdateSkinColorOverride(Color color) {
            SelectedSkin.OverrideColor = color;
            if (SelectedSkin.ColorOnly) {
                foreach (var gameObject in SelectedVariant.GameObjects) {
                    var renderer = gameObject.GetComponent<Renderer>();
                    renderer.sharedMaterial.color = color;
                }
            }
        }

        private void ApplySkinColor(Color color) {
            var list = CustomizableSkinGameObject.SearchColoredSkin(m_asset, SelectedVariant);
            foreach (var rend in list)
                rend.sharedMaterial.color = color;
        }

        private Texture2D ConvertTexture2D(Texture2D newTexture, Texture2D oldTexture) {
            if (newTexture == null || !newTexture.GetPixels().Any()) return oldTexture;
            var newTexture2DInArgb32 = new Texture2D(newTexture.width, newTexture.height, TextureFormat.ARGB32, false);
            newTexture2DInArgb32.SetPixels(newTexture.GetPixels());
            newTexture2DInArgb32.Apply();
            return newTexture2DInArgb32;
        }

        private void ShowDefaultColor(bool show) {
            m_defaultColor.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void ShowColorOverride(bool show) {
            m_skinColorOverride.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
            m_skinDownload.style.display = !show ? DisplayStyle.Flex : DisplayStyle.None;
            m_skinEdit.style.display = !show ? DisplayStyle.Flex : DisplayStyle.None;
            m_skinSave.style.display = !show ? DisplayStyle.Flex : DisplayStyle.None;
            m_platforms.style.display = !show ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}
