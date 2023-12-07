using System;
using UnityEngine;

namespace Services.SoundsSystem
{
    [Serializable]
    public class MusicContainer
    {
        [SerializeField] public MusicType Type;
        [SerializeField] public AudioClip Clip;
    }
}