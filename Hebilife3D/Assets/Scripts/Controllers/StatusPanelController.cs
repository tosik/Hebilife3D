using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusPanelController : Controller
{
    [SerializeField]
    UnityEngine.UI.Text Steps;
    [SerializeField]
    UnityEngine.UI.Text Snakes;
    [SerializeField]
    UnityEngine.UI.Text Feeds;

    void Update()
    {
        Steps.text = string.Format("Steps: {0}", Context.Status.Steps);
        Snakes.text = string.Format("Snakes: {0}", Context.Status.Snakes);
        Feeds.text = string.Format("Feeds: {0}", Context.Status.Feeds);
    }
}
