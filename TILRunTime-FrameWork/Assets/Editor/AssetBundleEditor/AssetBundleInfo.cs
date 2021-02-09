using System.Collections.Generic;

namespace AssetBundleManager.Editor
{
    public class AssetBundleInfo
    {
        /// <summary>
        /// 当前的所有AB包
        /// </summary>
        public List<AssetBundleBuildInfo> AssetBundles
        {
            get;
            set;
        }

        public AssetBundleInfo()
        {
            AssetBundles = new List<AssetBundleBuildInfo>();
        }
    }
}
