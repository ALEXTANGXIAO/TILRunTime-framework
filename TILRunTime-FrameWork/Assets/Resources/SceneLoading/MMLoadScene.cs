using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using MoreMountains.Tools;
using UnityEngine.SceneManagement;

namespace MoreMountains.Tools
{	
	/// <summary>
	/// 
	/// </summary>
	public class MMLoadScene : UnitySingleton<MMLoadScene> 
	{
		/// the possible modes to load scenes. Either Unity's native API, or MoreMountains' LoadingSceneManager
		public enum LoadingSceneModes { UnityNative, MMLoadingSceneManager }


		public string SceneName;

		public LoadingSceneModes LoadingSceneMode = LoadingSceneModes.UnityNative;

		/// <summary>
		/// 加载场景
		/// </summary>
		public virtual void LoadScene()
		{
			if (LoadingSceneMode == LoadingSceneModes.UnityNative)
			{
				SceneManager.LoadScene (SceneName);
			}
			if (LoadingSceneMode == LoadingSceneModes.MMLoadingSceneManager)
			{
				LoadingSceneManager.LoadScene (SceneName); 
			}
		}
	}
}
