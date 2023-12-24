using System.Collections.Generic;
using UnityEngine;

namespace PropsPools
{
    internal class DistrictPropsContainer : MonoBehaviour
    {
#if UNITY_EDITOR
        [ContextMenu("Setup")]
        public void Setup()
        {
            HashSet<string> appliedPrefabs = new HashSet<string>();
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);

                var PoolableObjects = child.GetComponents<PoolableRenderedObject>();
                if (PoolableObjects.Length > 1)
                {
                    for (int j = 1; j < PoolableObjects.Length; j++)
                    {
                        DestroyImmediate(PoolableObjects[j]);
                    }
                }

                if (PoolableObjects.Length == 0)
                {
                    var poolableRenderer = child.gameObject.AddComponent<PoolableRenderedObject>();
                    poolableRenderer.Setup();
                }/*
                if (PrefabUtility.IsPartOfPrefabInstance(child))
                {
                    GameObject childAsset = PrefabUtility.GetCorrespondingObjectFromSource(child.gameObject);
                    string childPath = AssetDatabase.GetAssetPath(childAsset);
                    if (!appliedPrefabs.Contains(childPath))
                    {
                        if (PoolableObjects.Length > 1)
                        {
                            for (int j = 1; j < PoolableObjects.Length; j++)
                            {
                                DestroyImmediate(PoolableObjects[j]);
                            }
                        }

                        if (PoolableObjects.Length == 0)
                        {
                            var poolableRenderer = child.gameObject.AddComponent<PoolableRenderedObject>();
                            poolableRenderer.Setup();
                        }
                        appliedPrefabs.Add(childPath);
                    }
                }*/
            }
        }
#endif
    }
}