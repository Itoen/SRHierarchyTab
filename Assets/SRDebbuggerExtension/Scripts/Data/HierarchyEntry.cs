using UnityEngine;

namespace SRHierarchyTab
{
    public class HierarchyEntry
    {
        private const int PreviewLength = 180;

        public string PathPreview
        {
            get
            {
                if (string.IsNullOrEmpty(this.HierarchyPath))
                {
                    return "";
                }
                return this.HierarchyPath.Substring(0, Mathf.Min(this.HierarchyPath.Length, PreviewLength));
            }
        }

        public string HierarchyPath
        {
            private set;
            get;
        }

        public HierarchyEntry (string hierarchyPath)
        {
            this.HierarchyPath = hierarchyPath;
        }
    }
}
