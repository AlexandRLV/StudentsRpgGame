using System.Collections.Generic;
using UnityEngine;

namespace Services.WindowsSystem
{
    [CreateAssetMenu(fileName = "Game Windows")]
    public class GameWindows : ScriptableObject
    {
        [SerializeField] public List<WindowBase> Windows;
    }
}