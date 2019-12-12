using System.Collections;
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
    public bool seenDoorman;
    public bool seenHobo;

    public bool gameOver;

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
        //test
        //Debug.Log("Body Roll: " + dice);//test
        //Debug.Log("Body Stat: " + Body);//test
        //Debug.Log("Body Total: " + (dice + Body));//test
        return dice + Body;
    }

    public int mindRoll()
    {
        int dice = (int)Mathf.Round(Random.Range(1, 12));
        //test
        //Debug.Log("Mind Roll: " + dice);//test
        //Debug.Log("Mind Stat: " + Mind);//test
        //Debug.Log("Mind Total: " + (dice + Mind));//test
        return dice + Mind;
    }

    public int soulRoll()
    {
        Karma++;
        int dice = (int)Mathf.Round(Random.Range(1, 12));
        //Debug.Log("Soul Roll: " + dice);//test
        //Debug.Log("Soul Stat: " + Soul);//test
        //Debug.Log("Soul Total: " + (dice + Soul));//test
        return dice + Soul;
    }

    public void obtainBars()
    {
        bars++;
        //Debug.Log("Number of Bars: " + bars); //test
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
        lighter = true;
    }

    public bool getLighter()
    {
        return lighter;
    }

    public void changeNames()
    {
        names = true;
    }

    public bool getNames()
    {
        return names;
    }

    public void changeKids()
    {
        seenKids = true;
    }

    public bool getKids()
    {
        return seenKids;
    }

    public void changeDoorman()
    {
        seenDoorman = true;
    }

    public bool getDoorman()
    {
        return seenDoorman;
    }

    public void changeHobo()
    {
        seenHobo = true;
    }

    public bool getHobo()
    {
        return seenHobo;
    }

    public void makeTheGameOver()
    {
        gameOver = true;
    }

    public bool isTheGameOver()
    {
        return gameOver;
    }

    public bool playerBad()
    {
        return Karma < 0;
    }

    public bool playerGood()
    {
        return Karma > 0;
    }
}
