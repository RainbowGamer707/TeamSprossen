using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprossenController : MonoBehaviour
{
    private Renderer _sprossenRenderer;

    // Init variable to track status of Sprossen (Angry/Happy etc). 
    private float _sprossenStatus;
    
    // Start is called before the first frame update
    void Start()
    { 
        // Populate variables from the SerialController.
        _sprossenStatus = SerialController.SprossenStatus;

        _sprossenRenderer = GetComponent<Renderer>();
        _sprossenRenderer.material.color = Color.blue;
    }

    // Update is called once per frame
    void Update()
    {
        // Populate variables from the SerialController.
        _sprossenStatus = SerialController.SprossenStatus;
        
        Debug.Log("SC - _sprossenStatus = " + _sprossenStatus);
        
        // CHANGE COLOUR OF SPRITE BASED ON SPROSSEN MOVEMENT, 0 = PURPLE/NEUTRAL, 1 = GREEN/POSITIVE, 2 = RED/NEGATIVE
        var material = _sprossenRenderer.material;
        material.color = _sprossenStatus switch
        {
            // Todo- (Make sure this matches code sent to Arduino) CHANGE COLOUR OF SPRITE
            < 10 and > -10 => Color.blue,
            > 9 => Color.green,
            < -9 => Color.red,
            _ => material.color
        };
    }
}
