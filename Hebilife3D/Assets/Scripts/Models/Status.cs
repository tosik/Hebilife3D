using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hebilife;
using System.Linq;

public class Status
{
    public int Steps;
    public int Snakes;
    public int Feeds;

    public void Reflect(Schale schale)
    {
        Snakes = schale.Snakes.Count();
        Feeds = schale.Feeds.Items.Count();
    }
}
