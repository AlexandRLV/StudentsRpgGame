using System.Collections.Generic;
using UnityEngine;

namespace Startup
{
    public class Startup : MonoBehaviour
    {
        private List<IInitializer> _startupInitializers;
        private List<IInitializer> _gameplayInitializers;
    }
}