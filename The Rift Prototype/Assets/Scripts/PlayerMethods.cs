﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMethods : MonoBehaviour
{
    public int Karma = 0;
    public int Body;
    public int Mind;
    public int Soul;

    public int bars;
    public bool vase;
    public bool water;
    public bool lighter;
    public bool names;
    public bool seenKids;

    public void karmaLoss()
    {
        Karma--;
    }

    public void karmaGain()
    {
        Karma++;
    }

    public int bodyRoll()
    {
        Karma--;
        int dice = (int) Mathf.Round(Random.Range(1, 12));
        Debug.Log("Body Roll: " + dice);
        Debug.Log("Body Stat: " + Body);
        Debug.Log("Body Total: " + (dice + Body));
        return dice + Body;
    }

    public int mindRoll()
    {
        int dice = (int)Mathf.Round(Random.Range(1, 12));
        Debug.Log("Mind Roll: " + dice);
        Debug.Log("Mind Stat: " + Mind);
        Debug.Log("Mind Total: " + (dice + Mind));
        return dice + Mind;
    }

    public int soulRoll()
    {
        Karma++;
        int dice = (int)Mathf.Round(Random.Range(1, 12));
        Debug.Log("Soul Roll: " + dice);
        Debug.Log("Soul Stat: " + Soul);
        Debug.Log("Soul Total: " + (dice + Soul));
        return dice + Soul;
    }

    public void obtainBars()
    {
        bars++;
        Debug.Log("Number of Bars: " + bars);
    }

    public void loseBars()
    {
        bars--;
    }

    public bool getBars()
    {
        return bars > 0;
    }

    public void changeVase()
    {
        vase = !vase;
    }

    public bool getVase()
    {
        return vase;
    }

    public void changeWater()
    {
        if(vase)
        {
            water = !water;
        }
    }

    public bool getWater()
    {
        return water;
    }

    public void changeLighter()
    {
        lighter = !lighter;
    }

    public bool getLighter()
    {
        return lighter;
    }

    public void changeNames()
    {
        names = !names;
    }

    public bool getNames()
    {
        return names;
    }

    public void changeKids()
    {
        seenKids = !seenKids;
    }

    public bool getKids()
    {
        return seenKids;
    }
}
