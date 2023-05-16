using System;
using Core;
using UnityEngine;
using Zenject;

public class Boot : MonoBehaviour
{
    [Inject] private SceneLoader SceneLoader { get; set; }

    private void Start()
    {
        SceneLoader.LoadMainMenu();
    }
}