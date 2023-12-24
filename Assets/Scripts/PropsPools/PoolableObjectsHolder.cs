using UnityEngine;

namespace PropsPools
{
    public class PoolableObjectsHolder : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] internal int HolderID;

        [ContextMenu("Setup")]
        public void Setup()
        {
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
                }
            }
        }

        private void OnDrawGizmos()
        {
            UnityEditor.Handles.Label(transform.position,
                new GUIContent(gameObject.name + " HolderID: " + HolderID));
        }
#endif
    }
}