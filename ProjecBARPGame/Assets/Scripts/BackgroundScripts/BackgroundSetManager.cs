using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSetManager : MonoBehaviour
{
    public GameObject[] backgroundSets;
    private GameObject currentSet;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (backgroundSets.Length > 0)
        {
            SetActiveSet(backgroundSets[0]);
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void SetActiveSet(GameObject newSet)
    {
        foreach (GameObject set in backgroundSets)
        {
            set.SetActive(set == newSet);
        }

        currentSet = newSet;
    }

    public GameObject GetCurrentSet()
    {
        return currentSet;
    }
}
