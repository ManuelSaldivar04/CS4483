using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHARACTERMANAGER : MonoBehaviour
{
    public static CHARACTERMANAGER Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(this.gameObject);
    }
}
