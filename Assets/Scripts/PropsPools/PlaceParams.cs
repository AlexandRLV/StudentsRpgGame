using System;
using UnityEngine;

namespace PropsPools
{
    [Serializable]
    internal class PlaceParams
    {
        [SerializeField] internal int ObjectID;
        [SerializeField] internal Vector3 Position;
        [SerializeField] internal Quaternion Rotation;
        [SerializeField] internal Vector3 Scale;

        //for editor script purposes only
        [SerializeField] internal int HolderId = 0;

        internal PlaceParams(int objectID, Vector3 position, Quaternion rotation, Vector3 scale, int holderID = -1)
        {
            ObjectID = objectID;
            Position = position;
            Rotation = rotation;
            Scale = scale;
            HolderId = holderID;
        }
    }
}