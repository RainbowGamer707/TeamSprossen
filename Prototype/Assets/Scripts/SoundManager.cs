using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    
    private static AudioSource _sprossenHappy;
    private static AudioSource _sprossenAngry;
    [SerializeField] private AudioSource treeHappy, treeNeutral, treeSad;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

    }

    private void Start()
    {
        _sprossenAngry = FindObjectOfType(typeof(AudioSource)) as AudioSource;
    }

    public static void PlaySprossenHappy()
    {
        Debug.Log("SM - PlaySprossenHappy Was Called");
        _sprossenHappy.Play();
    }
    
    public static void PlaySprossenAngry()
    {
        Debug.Log("SM - PlaySprossenAngry Was Called");
        _sprossenAngry.Play();
    }
    
    public void PlayTreeHappy()
    {
        treeHappy.Play();
    }
    
    public void PlayTreeNeutral()
    {
        treeNeutral.Play();
    }
    
    public void PlayTreeSad()
    {
        treeSad.Play();
    }
}
