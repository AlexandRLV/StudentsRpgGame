using System;
using UnityEngine;

namespace PropsPools
{
    [Serializable]
    internal class DistrictPropsPlaceInfo
    {
        [SerializeField] internal string PathToContainer;
        [SerializeField] internal int HolderId;
    }
}