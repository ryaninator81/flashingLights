using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using doubleNegatives;

public class doubleNegativesScript : MonoBehaviour
{
    public KMBombInfo Bomb;
    public KMAudio Audio;
    public KMSelectable[] button;
    public Renderer[] leds;
    public Texture[] ledColours;
    private List<int> selectedColours = new List<int>();
    private List<int> selectedColours2 = new List<int>();
    private int increaser = 0;
    private int increaser2 = 0;

    private int blueTotal1 = 0;
    private int greenTotal1 = 0;
    private int redTotal1 = 0;
    private int purpleTotal1 = 0;
    private int orangeTotal1 = 0;

    private int blueTotal2 = 0;
    private int greenTotal2 = 0;
    private int redTotal2 = 0;
    private int purpleTotal2 = 0;
    private int orangeTotal2 = 0;

    private int sumOf1 = 0;
    private int sumOf2 = 0;
    private int answer1 = 0;
    private int answer2 = 0;
    private int pressedButton = 0;
    private bool moduleSolved;

    //Logging
    private int stage = 0;
    static int moduleIdCounter = 1;
    int moduleId;

    void Awake()
    {
        moduleId = moduleIdCounter++;
        foreach (KMSelectable iterator in button)
        {
            KMSelectable pressedButton = iterator;
            iterator.OnInteract += delegate () { OnButtonPress(pressedButton); return false; };
        }
    }

    void Start()
    {
        while(selectedColours.Count != 12)
        {
            int index = UnityEngine.Random.Range(1,6);
            selectedColours.Add(index);
        }
        StartCoroutine(sequence());
        foreach(int colour in selectedColours)
        {
            if(colour == 1)
            {
                blueTotal1++;
            }
            else if(colour == 2)
            {
                greenTotal1++;
            }
            else if(colour == 3)
            {
                redTotal1++;
            }
            else if(colour == 4)
            {
                purpleTotal1++;
            }
            else if(colour == 5)
            {
                orangeTotal1++;
            }
        }
        Debug.LogFormat("[Flashing Lights #{0}] For the top light: blue flashes = {1}; green flashes = {2}; red flashes = {3}; purple flashes = {4}; orange flashes = {5}.", moduleId, blueTotal1, greenTotal1, redTotal1, purpleTotal1, orangeTotal1);
        blueTotal1 = blueTotal1 * 4;
        greenTotal1 = greenTotal1 * 2;
        redTotal1 = redTotal1 * 3;
        purpleTotal1 = purpleTotal1 * 6;
        orangeTotal1 = orangeTotal1 * 1;
        Debug.LogFormat("[Flashing Lights #{0}] For the top light: blue score = {1}; green score = {2}; red score = {3}; purple score = {4}; orange score = {5}.", moduleId, blueTotal1, greenTotal1, redTotal1, purpleTotal1, orangeTotal1);
        sumOf1 = blueTotal1 + greenTotal1 + redTotal1 + purpleTotal1 + orangeTotal1;
        Debug.LogFormat("[Flashing Lights #{0}] The top total is {1}.", moduleId, sumOf1);

        while(selectedColours2.Count != 12)
        {
            int index = UnityEngine.Random.Range(1,6);
            selectedColours2.Add(index);
        }
        StartCoroutine(sequence2());
        foreach(int colour in selectedColours2)
        {
            if(colour == 1)
            {
                blueTotal2++;
            }
            else if(colour == 2)
            {
                greenTotal2++;
            }
            else if(colour == 3)
            {
                redTotal2++;
            }
            else if(colour == 4)
            {
                purpleTotal2++;
            }
            else if(colour == 5)
            {
                orangeTotal2++;
            }
        }
        Debug.LogFormat("[Flashing Lights #{0}] For the bottom light: blue flashes = {1}; green flashes = {2}; red flashes = {3}; purple flashes = {4}; orange flashes = {5}.", moduleId, blueTotal2, greenTotal2, redTotal2, purpleTotal2, orangeTotal2);
        blueTotal2 = blueTotal2 * 2;
        greenTotal2 = greenTotal2 * 7;
        redTotal2 = redTotal2 * 6;
        purpleTotal2 = purpleTotal2 * 9;
        orangeTotal2 = orangeTotal2 * 3;
        Debug.LogFormat("[Flashing Lights #{0}] For the bottom light: blue score = {1}; green score = {2}; red score = {3}; purple score = {4}; orange score = {5}.", moduleId, blueTotal2, greenTotal2, redTotal2, purpleTotal2, orangeTotal2);
        sumOf2 = blueTotal2 + greenTotal2 + redTotal2 + purpleTotal2 + orangeTotal2;
        Debug.LogFormat("[Flashing Lights #{0}] The bottom total is {1}.", moduleId, sumOf2);
        answer1 = (sumOf1 % 5) + 1;
        answer2 = (sumOf2 % 5) + 1;
        Debug.LogFormat("[Flashing Lights #{0}] Press button {1}, then button {2}.", moduleId, answer1, answer2);
    }

    public void OnButtonPress(KMSelectable iterator)
    {
        if(iterator == button[0])
        {
            pressedButton = 1;
        }
        else if(iterator == button[1])
        {
            pressedButton = 2;
        }
        else if(iterator == button[2])
        {
            pressedButton = 3;
        }
        else if(iterator == button[3])
        {
            pressedButton = 4;
        }
        else if(iterator == button[4])
        {
            pressedButton = 5;
        }
        button[2].AddInteractionPunch();
        GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        if(moduleSolved)
        {
            return;
        }
        if(iterator == button[answer1 - 1] && stage == 0)
        {
            stage++;
            Debug.LogFormat("[Flashing Lights #{0}] You pressed button {1}. That is correct.", moduleId, answer1);
        }
        else if (iterator == button[answer2 - 1] && stage == 1)
        {
            GetComponent<KMBombModule>().HandlePass();
            moduleSolved = true;
            Debug.LogFormat("[Flashing Lights #{0}] You pressed button {1}. That is correct. Module disarmed.", moduleId, answer2);
        }
        else
        {
            StopAllCoroutines();
            stage = 0;
            blueTotal1 = 0;
            greenTotal1 = 0;
            redTotal1 = 0;
            purpleTotal1 = 0;
            orangeTotal1 = 0;
            blueTotal2 = 0;
            greenTotal2 = 0;
            redTotal2 = 0;
            purpleTotal2 = 0;
            orangeTotal2 = 0;
            sumOf1 = 0;
            sumOf2 = 0;
            answer1 = 0;
            answer2 = 0;
            GetComponent<KMBombModule>().HandleStrike();
            Debug.LogFormat("[Flashing Lights #{0}] Strike! You pressed button {1}. That is not correct.", moduleId, pressedButton);
            leds[0].material.mainTexture = ledColours[0];
            leds[1].material.mainTexture = ledColours[0];
            pressedButton = 0;
            selectedColours.Clear();
            selectedColours2.Clear();
            increaser = 0;
            increaser2 = 0;
            Start();
        }
    }

    IEnumerator sequence()
    {
        yield return new WaitForSeconds(1f);
        while(!moduleSolved)
        {
            yield return new WaitForSeconds(0.5f);
            leds[0].material.mainTexture = ledColours[0];
            yield return new WaitForSeconds(0.5f);
            leds[0].material.mainTexture = ledColours[selectedColours[0 + increaser]];
            increaser++;
            if(increaser == 12)
            {
                yield return new WaitForSeconds(0.5f);
                leds[0].material.mainTexture = ledColours[0];
                increaser = 0;
                yield return new WaitForSeconds(0.5f);
            }
        }
        leds[0].material.mainTexture = ledColours[0];
    }

    IEnumerator sequence2()
    {
        yield return new WaitForSeconds(1f);
        while(!moduleSolved)
        {
            yield return new WaitForSeconds(0.3f);
            leds[1].material.mainTexture = ledColours[0];
            yield return new WaitForSeconds(0.3f);
            leds[1].material.mainTexture = ledColours[selectedColours2[0 + increaser2]];
            increaser2++;
            if(increaser2 == 12)
            {
                yield return new WaitForSeconds(0.3f);
                leds[1].material.mainTexture = ledColours[0];
                increaser2 = 0;
                yield return new WaitForSeconds(0.3f);
            }
        }
        leds[1].material.mainTexture = ledColours[0];
    }

    private string TwitchHelpMessage = @"Use '!{0} press 1' to press a button.";

    IEnumerator ProcessTwitchCommand(string command) // Almost exactly like british slang :P
    {
        var parts = command.ToLowerInvariant().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length == 2 && parts[0] == "press" && parts[1].Length == 1 && "12345".Contains(parts[1]))
        {
            yield return null;
            OnButtonPress(button[Int32.Parse(parts[1]) - 1]);
        }
    }
}
