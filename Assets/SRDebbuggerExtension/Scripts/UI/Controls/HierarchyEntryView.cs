using System;
using SRDebugger;
using SRF;
using SRF.UI.Layout;
using UnityEngine;
using UnityEngine.UI;

namespace SRHierarchyTab
{
    [RequireComponent(typeof(RectTransform))]
    public class HierarchyEntryView : SRMonoBehaviourEx, IVirtualView
    {
        private HierarchyEntry prevData;
        private RectTransform rectTransform;

        [SerializeField]
        private Text hierarchyPathPreview;

        public void SetDataContext (object data)
        {
            var path = data as HierarchyEntry;

            if (path == null)
            {
                throw new Exception("Data should be a HierarchyEntry");
            }

            // Only update everything else if data context has changed, not just for an update
            if (path == this.prevData)
            {
                return;
            }
            this.prevData = path;

            this.hierarchyPathPreview.text = path.PathPreview;
        }

        protected override void Awake ()
        {
            base.Awake();

            this.rectTransform = CachedTransform as RectTransform;
            this.hierarchyPathPreview.supportRichText = Settings.Instance.RichTextInConsole;
        }
    }
}
