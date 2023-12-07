using System;
using UnityEngine;

namespace Services.SoundsSystem
{
    [Serializable]
    public class SoundContainer
    {
        [SerializeField] public SoundType Type;
        [SerializeField] public AudioClip Clip;
    }
}