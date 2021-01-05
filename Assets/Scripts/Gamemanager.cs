using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{
    [SerializeField] public string _sceneName;
    private string beforeScene;

    public void sceneChange(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void saveBefore(string name)
    {
        beforeScene = name;
    }
}
