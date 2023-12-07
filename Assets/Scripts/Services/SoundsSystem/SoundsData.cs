using UnityEngine;

namespace Services.SoundsSystem
{
    [CreateAssetMenu(fileName = "Sounds Data")]
    public class SoundsData : ScriptableObject
    {
        [SerializeField] public SoundContainer[] SoundClips;
        [SerializeField] public MusicContainer[] MusicClips;
    }
}