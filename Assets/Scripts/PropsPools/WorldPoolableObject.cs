using UnityEngine;

namespace PropsPools
{
    public abstract class WorldPoolableObject : MonoBehaviour
    {
        internal abstract void OnTakeFromPool();

        internal abstract void OnReturnedToPool();

        internal abstract void OnDestroyCall();
        internal abstract void Place(Vector3 pos, Quaternion rot, Vector3 scale, Transform parent);

        internal abstract void Place(Vector3 pos, Quaternion rot, Transform parent);

        internal abstract void Place(Vector3 pos, Quaternion rot);
        internal abstract void Place(Vector3 pos, Quaternion rot, Vector3 scale);

        internal abstract void Place(Vector3 pos);

        internal abstract GameObject GetGameObject { get; }

    }
}