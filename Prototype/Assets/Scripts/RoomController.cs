using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    private Renderer _surfaceRenderer;

    // Init variable to track spontaneous interaction. 
    private float _roomStatus;
    
    // Start is called before the first frame update
    void Start()
    { 
        // Populate variables from the SerialController.
        _roomStatus = SerialController.RoomStatus;

        _surfaceRenderer = GetComponent<Renderer>();
        _surfaceRenderer.material.color = Color.blue;
    }

    // Update is called once per frame
    void Update()
    {
        // Populate variables from the SerialController.
        _roomStatus = SerialController.RoomStatus;
        
        // CHANGE COLOUR OF SURFACE BASED ON CURRENT INTERACTION LEVEL. (Rough/Gentle etc)
        var material = _surfaceRenderer.material;
        material.color = _roomStatus switch
        {
            // CHANGE COLOUR OF SURFACE BASED ON THESE VALUES
            < 5 and > -5=> Color.blue,
            > 4 => Color.green,
            < -4 => Color.red,
            _ => material.color
        };
    }
}
