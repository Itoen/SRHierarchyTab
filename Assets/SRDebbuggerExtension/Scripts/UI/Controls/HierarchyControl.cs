#pragma warning disable 169
#pragma warning disable 649

using System;
using System.Collections;
using System.Collections.Generic;
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
            this.hierarchyScrollLayoutGroup.SelectedItem = null;

            var hierarchies = this.GetAllHierarchyPath();

            for (var i = 0; i < hierarchies.Length; i++)
            {
                var hierarchyPath = hierarchies[i];

                if (!string.IsNullOrEmpty(Filter))
                {
                    if (hierarchyPath.IndexOf(Filter, StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        continue;
                    }
                }

                var hierarchyEntry = new HierarchyEntry(hierarchyPath);
                this.hierarchyScrollLayoutGroup.AddItem(hierarchyEntry);
            }
        }

        private string[] GetAllHierarchyPath ()
        {
            var hierarchyPathList = new List<string>();
            var transforms = FindObjectsOfType<Transform>();
            foreach (var transform in transforms)
            {
                if (transform.parent == null)
                {
                    hierarchyPathList.Add(transform.name);
                    continue;
                }
                var path = this.GetHierarchyPath(transform);
                hierarchyPathList.Add(path);
            }

            return hierarchyPathList.ToArray();
        }

        private string GetHierarchyPath (Transform transform)
        {
            var path = transform.name;
            var parent = transform.parent;

            while (parent != null)
            {
                path = string.Format("{0}/{1}", parent.name, path);
                parent = parent.parent;
            }

            return path;
        }
    }
}
