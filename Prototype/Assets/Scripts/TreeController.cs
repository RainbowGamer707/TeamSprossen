using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    private Renderer _treeRenderer;
    
    // Init variable to track health of persistant interaction (Tree).
    private int _soulHealth;

    // Start is called before the first frame update
    void Start()
    { 
        // Populate variables from the SerialController.
        _soulHealth = SerialController.SoulHealth;

        _treeRenderer = GetComponent<Renderer>();
        _treeRenderer.material.color = Color.blue;
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
            // Todo - (Need to update to match the Arduino) CHANGE COLOUR OF TREE BASED ON THESE VALUES
            case < 75 and > 25:
                material.color = Color.blue;
                break;
            case > 74:
                material.color = Color.green;
                break;
            case < 26:
                material.color = Color.red;
                break;
        }
    }
}
