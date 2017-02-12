using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    protected Context Context;

    public virtual void Start()
    {
        Context = Context.Instance;
    }
}
