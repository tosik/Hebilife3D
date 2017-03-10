using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Context : MonoBehaviour
{
    public static Context Instance;

    void Awake()
    {
        Instance = this;

        Status = new Status();
    }

    public Status Status;
}
