#pragma warning disable 169
#pragma warning disable 649

using System;
using System.Collections;
using SRF;
using SRF.UI.Layout;
using UnityEngine;
using UnityEngine.UI;

namespace SRHierarchyTab
{
    public class HierarchyControl : SRMonoBehaviourEx
    {
        [SerializeField]
        private VirtualVerticalLayoutGroup hierarchyScrollLayoutGroup;

        [SerializeField]
        private ScrollRect hierarchyScrollRect;

        private Vector2? scrollPosition;
        public Action<HierarchyEntry> SelectedItemChanged;
        private string filter;


        public bool EnableSelection
        {
            get { return this.hierarchyScrollLayoutGroup.EnableSelection; }
            set { this.hierarchyScrollLayoutGroup.EnableSelection = value; }
        }

        public string Filter
        {
            get { return this.filter; }
            set
            {
                if (this.filter != value)
                {
                    this.filter = value;
                }
            }
        }

        protected override void Awake ()
        {
            base.Awake();

            this.hierarchyScrollLayoutGroup.SelectedItemChanged.AddListener(OnSelectedItemChanged);
        }

        protected override void Start ()
        {
            base.Start();
            this.StartCoroutine(ScrollToBottom());
        }

        protected override void OnEnable ()
        {
            this.Refresh();
        }

        IEnumerator ScrollToBottom ()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            this.scrollPosition = new Vector2(0, 0);
        }

        private void OnSelectedItemChanged (object arg0)
        {
            var entry = arg0 as HierarchyEntry;

            if (this.SelectedItemChanged != null)
            {
                this.SelectedItemChanged(entry);
            }
        }

        protected override void Update ()
        {
            base.Update();

            if (this.scrollPosition.HasValue)
            {
                this.hierarchyScrollRect.normalizedPosition = this.scrollPosition.Value;
                this.scrollPosition = null;
            }
        }

        public void Refresh ()
        {
            if (this.hierarchyScrollRect.normalizedPosition.y.ApproxZero())
            {
                this.scrollPosition = this.hierarchyScrollRect.normalizedPosition;
            }

            this.hierarchyScrollLayoutGroup.ClearItems();

            /// @todo ヒエラルキーの取得
            HierarchyEntry[] entries = new HierarchyEntry[0];

            for (var i = 0; i < entries.Length; i++)
            {
                var entry = entries[i];

                if (!string.IsNullOrEmpty(Filter))
                {
                    if (entry.HierarchyPath.IndexOf(Filter, StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        if (entry == this.hierarchyScrollLayoutGroup.SelectedItem)
                        {
                            this.hierarchyScrollLayoutGroup.SelectedItem = null;
                        }
                        continue;
                    }
                }

                this.hierarchyScrollLayoutGroup.AddItem(entry);
            }
        }
    }
}
