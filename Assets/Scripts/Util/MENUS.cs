using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MENUS : MonoBehaviour
{
    public static MENUS Instance { get; private set; }

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
