using UnityEngine;

namespace Gameplay.Character.Player
{
    public interface IPlayerTransformInfo
    {
        public Vector3 Position { get; }
        public float YRotation { get; }
    }
}