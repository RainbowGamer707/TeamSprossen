using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprossenController : MonoBehaviour
{
    // Init Sprossen Renderer
    private Renderer _sprossenRenderer;
    
    // Init variable to track status of Sprossen (Angry/Happy etc). 
    private float _sprossenStatus;
    private float _lastSprossenStatus;
    private float _sprossenCheck;
    
    // Init Audio Clips and Sources for Sprossen
    public AudioClip sprossenHappy;
    public AudioClip sprossenNeutral;
    public AudioClip sprossenAngry;
    private AudioSource _sprossenAudio;
    
        
    // Start is called before the first frame update
    void Start()
    { 
        // Populate variables from the SerialController.
        _sprossenStatus = SerialController.SprossenStatus;

        _sprossenRenderer = GetComponent<Renderer>();
        _sprossenRenderer.material.color = Color.blue;

        // Get component for AudioSource
        _sprossenAudio = GetComponent<AudioSource>();
    }
    

    // Update is called once per frame
    void Update()
    {
        // Capture last value of Sprossen for comparison
        _lastSprossenStatus = _sprossenStatus switch
             {
                 < 10 and >= 0 => 0,
                 >= 10 => 1,
                 _ => -1
             };
        // Re-Populate variables from the SerialController.
        _sprossenStatus = SerialController.SprossenStatus;


        //Debug.Log("SC - _lastSprossenStatus = " + _lastSprossenStatus);
        //Debug.Log("SC - _sprossenStatus = " + _sprossenStatus);
        
        // CHANGE COLOUR OF SPRITE BASED ON SPROSSEN MOVEMENT, 0 = PURPLE/NEUTRAL, 1 = GREEN/POSITIVE, 2 = RED/NEGATIVE
        var material = _sprossenRenderer.material;
        material.color = _sprossenStatus switch
        {
            // Todo- (Make sure this matches code sent to Arduino) CHANGE COLOUR OF SPRITE
            < 10 and >= 0 => Color.blue,
            >= 10 => Color.green,
            < 0 => Color.red,
            _ => material.color
        };
        
        // Set variable to -1, 0, or 1 depending on interaction (-1 = bad, 0 = neutral, 1 = positive.)
        // This will be compared against lastSprossenStatus variable to see if there has been a state
        // change for audio.
        _sprossenCheck = _sprossenStatus switch
        {
            < 10 and >= 0 => 0,
            >= 10 => 1,
            _ => -1
        };
        
        Debug.Log("SC - _sprossenCheck = " + _sprossenCheck + " | _lastSprossenStatus = " + _lastSprossenStatus);
        
        // Only play sound if the value (Happy/Neutral/Sad) has changed (Not working yet).

        if ((int)_sprossenCheck != (int)_lastSprossenStatus)
        {
            if ((int)_sprossenCheck == 0)
            {
                _sprossenAudio.PlayOneShot(sprossenNeutral, 0.5f);
            }
            
            if ((int)_sprossenCheck == 1)
            {
                _sprossenAudio.PlayOneShot(sprossenHappy, 0.5f);
            }
    
            if ((int)_sprossenCheck == -1)
            {
                _sprossenAudio.PlayOneShot(sprossenAngry, 0.5f);
            }
        }
        

    }
}
