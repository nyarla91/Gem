using UnityEngine;

namespace Extentions
{
    public interface IPauseChanger
    {
        void AddPauseSource(MonoBehaviour source);
        void RemovePauseSource(MonoBehaviour source);
    }
}