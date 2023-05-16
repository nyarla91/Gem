using System;
using Extentions;
using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(Camera))]
    public class MainCamera : Transformable
    {
        public Camera Camera { get; private set; }
        
        private void Awake()
        {
            Camera = GetComponent<Camera>();
        }
    }
}