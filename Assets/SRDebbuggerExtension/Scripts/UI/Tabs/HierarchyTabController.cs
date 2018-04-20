using SRF;
using SRDebugger;
using SRDebugger.Services;
using SRDebugger.Internal;
using UnityEngine;
using UnityEngine.UI;

namespace SRHierarchyTab
{
    public class HierarchyTabController : SRMonoBehaviourEx
    {
        private readonly static int MaxLength = 2600;

        private Canvas hierarchyCanvas;

        [SerializeField]
        private HierarchyControl hierarchyControl;
        [SerializeField]
        private ScrollRect fullPathScrollRect;
        [SerializeField]
        private Text hierarchyPathText;
        [SerializeField]
        private Toggle filterToggle;
        [SerializeField]
        private InputField filterField;
        [SerializeField]
        private GameObject filterBarContainer;


        protected override void Start ()
        {
            base.Start();

            this.hierarchyCanvas = this.GetComponent<Canvas>();

            this.filterToggle.onValueChanged.AddListener(FilterToggleValueChanged);
            this.filterBarContainer.SetActive(this.filterToggle.isOn);

#if UNITY_5_3_OR_NEWER
            this.filterField.onValueChanged.AddListener(this.FilterValueChanged);
#else
            this.filterField.onValueChange.AddListener(this.FilterValueChanged);
#endif

            this.hierarchyControl.SelectedItemChanged = this.HierarchySelectedItemChanged;

            Service.Panel.VisibilityChanged += this.PanelOnVisibilityChanged;

            this.hierarchyPathText.supportRichText = Settings.Instance.RichTextInConsole;
            this.PopulateFullPathArea(null);
        }

        private void FilterToggleValueChanged (bool isOn)
        {
            if (isOn)
            {
                this.filterBarContainer.SetActive(true);
                this.hierarchyControl.Filter = this.filterField.text;
            }
            else
            {
                this.hierarchyControl.Filter = null;
                this.filterBarContainer.SetActive(false);
            }
        }

        private void FilterValueChanged (string filterText)
        {
            if (this.filterToggle.isOn && !string.IsNullOrEmpty(filterText) && filterText.Trim().Length != 0)
            {
                this.hierarchyControl.Filter = filterText;
            }
            else
            {
                this.hierarchyControl.Filter = null;
            }
        }

        private void PanelOnVisibilityChanged (IDebugPanelService debugPanelService, bool b)
        {
            if (this.hierarchyCanvas == null)
            {
                return;
            }

            if (b)
            {
                this.hierarchyCanvas.enabled = true;
            }
            else
            {
                this.hierarchyCanvas.enabled = false;
            }
        }

        private void HierarchySelectedItemChanged (object item)
        {
            var hierarchy = item as HierarchyEntry;
            this.PopulateFullPathArea(hierarchy);
        }

        private void PopulateFullPathArea (HierarchyEntry entry)
        {
            if (entry == null)
            {
                this.hierarchyPathText.text = "";
            }
            else
            {
                this.hierarchyPathText.text = entry.HierarchyPath.Substring(0, Mathf.Min(entry.HierarchyPath.Length, MaxLength));
            }

            this.fullPathScrollRect.normalizedPosition = new Vector2(0, 1);
        }
    }
}
