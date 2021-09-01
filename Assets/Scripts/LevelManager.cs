using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region Singleton Pattern
    static LevelManager instance;

    public static LevelManager Instance {
        get {
            if (!instance)
            {
                instance = new GameObject().AddComponent<LevelManager>();
                instance.name = "Game Manager";
            }
            return instance;
        }
    }

    void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this);
    }
    #endregion
    public delegate void MyDelegate();
    public MyDelegate restartLevelDelegate;
    public MyDelegate levelClearedDelegate;

    [ContextMenu("Delegate DEBUG")]
    void DEBUG()
    {
        foreach (var item in restartLevelDelegate.GetInvocationList())
        {
            print(item.Target);
        }
    }

    internal void RestartLevel()
    {
        Time.timeScale = 1;
        restartLevelDelegate.Invoke();

    }

    internal void LevelCleared()
    {
        levelClearedDelegate.Invoke();
    }
}
