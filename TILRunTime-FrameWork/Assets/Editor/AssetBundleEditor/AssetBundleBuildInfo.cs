using System.Collections.Generic;

namespace AssetBundleManager.Editor
{
    public class AssetBundleBuildInfo
    {
        /// <summary>
        /// AB包的名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// AB包中的所有资源
        /// </summary>
        public List<AssetInfo> Assets
        {
            get;
            set;
        }

        public AssetBundleBuildInfo(string name)
        {
            Name = name;
            Assets = new List<AssetInfo>();
        }
    }
}
