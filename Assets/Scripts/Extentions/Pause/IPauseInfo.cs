using UnityEngine;

namespace Extentions
{
    public interface IPauseInfo
    {
        bool IsPaused { get; }
        bool IsUnpaused { get; }
    }
}