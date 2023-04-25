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
    // private AudioSource _treeHappyAudio;
    // private AudioSource _treeNeutralAudio;
    // private AudioSource _treeSadAudio;


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
        // _treeHappyAudio = GetComponent<AudioSource>();
        // _treeHappyAudio.clip = treeHappy;
        // _treeNeutralAudio = GetComponent<AudioSource>();
        // _treeNeutralAudio.clip = treeNeutral;
        // _treeSadAudio = GetComponent<AudioSource>();
        // _treeSadAudio.clip = treeSad;
    }

    
    // Update is called once per frame
    void Update()
    {
        // Populate variables from the SerialController.
        _soulHealth = SerialController.SoulHealth;
        

        
        // CHANGE COLOUR OF TREE BASED ON CURRENT _soulHealth. 
        var material = _treeRenderer.material;

        switch (_soulHealth)
        {
            case <= 100:
                material.color = Color.red;
                break;
            case > 100 and <= 300:
                material.color = Color.magenta;
                break;
            case > 300 and < 500:
                material.color = Color.yellow;
                break;
            case >= 500 and <= 650:
                material.color = Color.blue;
                break;
            case > 650 and <= 850:
                material.color = Color.cyan;
                break;
            case > 850 and <= 999:
                material.color = Color.green;
                break;
            case >= 1000:
                material.color = Color.white;
                break;
        }
        
        //Debug.Log("TC - _soulHealth = " + _soulHealth);
        
        // Set sound for tree
        // if (_soulHealth < 500)
        // {
        //     Debug.Log("HERE 1");
        //     if (_treeSadAudio.isPlaying) return;
        //     _treeNeutralAudio.Stop();
        //     _treeSadAudio.Play();
        // } 
        // else if (_soulHealth is >= 500 and < 650)
        // {
        //     Debug.Log("HERE 2");
        //     if (_treeNeutralAudio.isPlaying) return;
        //     Debug.Log("HERE 2.2");
        //     if (_treeSadAudio.isPlaying)
        //     {
        //         Debug.Log("HERE 2.3");
        //         _treeSadAudio.Stop();
        //     }
        //     if (_treeHappyAudio.isPlaying)
        //     {
        //         _treeHappyAudio.Stop();
        //     }
        //     Debug.Log("HERE 2.4");
        //     _treeNeutralAudio.Play();
        // }
        // else
        // {
        //     Debug.Log("HERE 3");
        //     if (_treeHappyAudio.isPlaying) return;
        //     _treeNeutralAudio.Stop();
        //     _treeHappyAudio.Play();
        // }

        if (_soulHealth < 500)
        {
            Debug.Log("HERE 1");
            if (_treeAudio.isPlaying.Equals(treeSad)) return;
            Debug.Log("HERE 1.2");
            _treeAudio.Stop();
            _treeAudio.clip = treeSad;
            _treeAudio.Play();
        } 
        else if (_soulHealth >= 700)
        {
            Debug.Log("HERE 3");
            if (_treeAudio.isPlaying.Equals(treeHappy)) return;
            Debug.Log("HERE 3.2");
            _treeAudio.Stop();
            _treeAudio.clip = treeHappy;
            _treeAudio.Play();
        }
        else
        {
            Debug.Log("HERE 2");
            if (_treeAudio.isPlaying.Equals(treeNeutral)) return;
            Debug.Log("HERE 2.2");
            _treeAudio.Stop();
            _treeAudio.clip = treeNeutral;
            _treeAudio.Play();
        }
    }
}
