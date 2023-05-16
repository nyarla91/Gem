using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Extentions.Pause
{
    public class Pause : MonoBehaviour, IPauseInfo, IPauseChanger
    {
        private readonly List<MonoBehaviour> _pauseSources = new List<MonoBehaviour>();

        public bool IsPaused => _pauseSources.Where(source => source != null).ToArray().Length > 0;
        public bool IsUnpaused => ! IsPaused;
        
        public void AddPauseSource(MonoBehaviour source) => _pauseSources.Add(source);
        public void RemovePauseSource(MonoBehaviour source) => _pauseSources.TryRemove(source);
    }
}