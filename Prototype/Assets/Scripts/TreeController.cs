using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    // Init Tree renderer
    private Renderer _treeRenderer;
    
    // Init variable to track health of persistant interaction (Tree).
    private int _soulHealth;
    
    // Init Audio Clips and Sources for Sprossen
    public AudioClip treeHappy;
    public AudioClip treeNeutral;
    public AudioClip treeSad;
    private AudioSource _treeAudio;

    public Material rainbowLeaves;
    public Material greenLeaves;
    public Material aquaLeaves;
    public Material blueLeaves;
    public Material yellowLeaves;
    public Material orangeLeaves;
    public Material redLeaves;


    // Start is called before the first frame update
    void Start()
    { 
        // Populate variables from the SerialController.
        _soulHealth = SerialController.SoulHealth;

        // Get Renderer component and set initial colour.
        _treeRenderer = GetComponent<Renderer>();
        _treeRenderer.material.color = Color.blue;
        
        // Get component for AudioSource
        _treeAudio = GetComponent<AudioSource>();
    }

    
    // Update is called once per frame
    void Update()
    {
        // Populate variables from the SerialController.
        _soulHealth = SerialController.SoulHealth;
        
        // CHANGE COLOUR OF TREE BASED ON CURRENT _soulHealth. 
        // var material = _treeRenderer.material;
        //
        // switch (_soulHealth)
        // {
        //     case <= 100:
        //         material.color = Color.red;
        //         break;
        //     case > 100 and <= 300:
        //         material.color = Color.magenta;
        //         break;
        //     case > 300 and < 500:
        //         material.color = Color.yellow;
        //         break;
        //     case >= 500 and <= 650:
        //         material.color = Color.blue;
        //         break;
        //     case > 650 and <= 850:
        //         material.color = Color.cyan;
        //         break;
        //     case > 850 and <= 999:
        //         material.color = Color.green;
        //         break;
        //     case >= 1000:
        //         material.color = Color.white;
        //         break;
        // }

        // CHANGE COLOUR OF SURFACE BASED ON CURRENT INTERACTION LEVEL. (Rough/Gentle etc)
        _treeRenderer.material = _soulHealth switch
        {
            <= 100 => redLeaves,
            > 100 and <= 300 => orangeLeaves,
            > 300 and < 500 => yellowLeaves,
            >= 500 and <= 650 => blueLeaves,
            > 650 and <= 850 => aquaLeaves,
            > 850 and <= 999 => greenLeaves,
            >= 1000 => rainbowLeaves
        };
        
        //Debug.Log("TC - _soulHealth = " + _soulHealth);

        if (_soulHealth < 500)
        {
            //Debug.Log("HERE 1");
            if (_treeAudio.isPlaying.Equals(treeSad)) return;
            //Debug.Log("HERE 1.2");
            _treeAudio.Stop();
            _treeAudio.clip = treeSad;
            _treeAudio.volume = 0.3f;
            _treeAudio.Play();
        } 
        else if (_soulHealth >= 650)
        {
            //Debug.Log("HERE 3");
            if (_treeAudio.isPlaying.Equals(treeHappy)) return;
            //Debug.Log("HERE 3.2");
            _treeAudio.Stop();
            _treeAudio.clip = treeHappy;
            _treeAudio.volume = 0.2f;
            _treeAudio.Play();
        }
        else
        {
            //Debug.Log("HERE 2");
            if (_treeAudio.isPlaying.Equals(treeNeutral)) return;
            //Debug.Log("HERE 2.2");
            _treeAudio.Stop();
            _treeAudio.clip = treeNeutral;
            _treeAudio.volume = 0.1f;
            _treeAudio.Play();
        }

        if (_soulHealth > 1000)
        {
            _soulHealth = 1000;
        }

        if (_soulHealth < 0)
        {
            _soulHealth = 0;
        }
    }
}
