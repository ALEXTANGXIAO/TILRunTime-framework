using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngineInternal;
using Object = UnityEngine.Object;

namespace HotFix_Project
{
    public class DUnityUtil
    {
        [TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
        public static Component AddMonoBehaviour(Type type, GameObject go)
        {
            var comp = go.GetComponent(type);
            if (comp == null)
            {
                comp = go.AddComponent(type);
            }

            return comp;
        }

        public static T AddMonoBehaviour<T>(Component comp) where T : Component
        {
            var ret = comp.GetComponent<T>();
            if (ret == null)
            {
                ret = comp.gameObject.AddComponent<T>();
            }
            return ret;
        }

        public static T AddMonoBehaviour<T>(GameObject go) where T : Component
        {
            var comp = go.GetComponent<T>();
            if (comp == null)
            {
                comp = go.AddComponent<T>();
            }
            return comp;
        }

        [TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
        public static void RmvMonoBehaviour(Type type, GameObject go)
        {
            var comp = go.GetComponent(type);
            if (comp != null)
            {
                UnityEngine.Object.Destroy(comp);
            }
        }

        public static void RmvMonoBehaviour<T>(GameObject go) where T : Component
        {
            var comp = go.GetComponent<T>();
            if (comp != null)
            {
                UnityEngine.Object.Destroy(comp);
            }
        }


        public static Transform FindChild(Transform transform, string path)
        {
            var findTrans = transform.Find(path);
            if (findTrans != null)
            {
                return findTrans;
            }

            return null;
        }

        /// <summary>
        /// 根据名字找到子节点，主要用于dummy接口
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Transform FindChildByName(Transform transform, string name)
        {
            if (transform == null)
            {
                return null;
            }

            for (int i = 0; i < transform.childCount; i++)
            {
                var childTrans = transform.GetChild(i);
                if (childTrans.name == name)
                {
                    return childTrans;
                }

                var find = FindChildByName(childTrans, name);
                if (find != null)
                {
                    return find;
                }
            }

            return null;
        }

        [TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
        public static Component FindChildComponent(Type type, Transform transform, string path)
        {
            var findTrans = transform.Find(path);
            if (findTrans != null)
            {
                return findTrans.gameObject.GetComponent(type);
            }

            return null;
        }

        public static T FindChildComponent<T>(Transform transform, string path) where T : Component
        {
            var findTrans = transform.Find(path);
            if (findTrans != null)
            {
                return findTrans.gameObject.GetComponent<T>();
            }

            return null;
        }

        public static void SetLayer(GameObject go, int layer)
        {
            if (go == null)
            {
                return;
            }
            SetLayer(go.transform, layer);
        }

        public static void SetLayer(Transform trans, int layer)
        {
            if (trans == null)
            {
                return;
            }
            trans.gameObject.layer = layer;
            for (int i = 0, imax = trans.childCount; i < imax; ++i)
            {
                Transform child = trans.GetChild(i);
                SetLayer(child, layer);
            }
        }

        public static int RandomRangeInt(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        public static float RandomRangeFloat(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        public static Vector2 RandomInsideCircle(float radius)
        {
            return UnityEngine.Random.insideUnitCircle * radius;
        }

        //public static Transform[] 
        [TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
        public static Array CreateUnityArray(Type type, int length)
        {
            return Array.CreateInstance(type, length);
        }

        public static T[] CreateUnityArray<T>(int length)
        {
            return new T[length];
        }


        public static GameObject Instantiate(GameObject go)
        {
            if (go != null)
            {
                return UnityEngine.GameObject.Instantiate(go);
            }

            return null;
        }

        public static int GetHashCodeByString(string str)
        {
            /*
            本来下面这个是放在DodUtil里的，但是由于c#和lua的边界不同，所以翻译后算的结果不一。
            int h = 0;
            for (int i = 0; i < str.Length; i++)
            {
                h = 31 * h + str[i];
            }
            return h;*/
            return str.GetHashCode();
        }

        public static List<string> GetRegexMatchGroups(string pattern, string input)
        {
            List<string> list = new List<string>();
            var regexLink = new Regex(pattern);
            var links = regexLink.Match(input);
            for (var i = 0; i < links.Groups.Count; ++i)
            {
                list.Add(links.Groups[i].Value);
            }
            return list;
        }

        /// <summary>
        /// bytes比较特殊，特别是数组，lua本身有内置的结构，csharp层也有可能有bytes数组传入和传出，所以需要针对cs的对象提交一些基础接口
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public static int GetCsBytesLen(byte[] buff)
        {
            return buff.Length;
        }

        public static void SetMaterialVector3(Material mat, int nameId, Vector3 val)
        {
            mat.SetVector(nameId, val);
        }

        public static void GetVectorData(Vector3 val, out float x, out float y, out float z)
        {
            x = val.x;
            y = val.y;
            z = val.z;
        }
        public static void GetVector2Data(Vector2 val, out float x, out float y)
        {
            x = val.x;
            y = val.y;
        }

        public static void SetGameObjectActive(GameObject go, bool active)
        {
            if (go != null && go.activeSelf != active)
            {
                go.SetActive(active);
            }
        }
    }
}
