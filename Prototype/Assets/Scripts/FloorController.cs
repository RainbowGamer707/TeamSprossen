using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    private Renderer _surfaceRenderer;

    // Init variable to track health of persistant interaction (Tree).
    private int _soulHealth;
    
    // Material References
    public Material dryGround;
    public Material yellowGrass;
    public Material lushGrass;
    
    // Start is called before the first frame update
    void Start()
    {
        // Populate variables from the SerialController.
        _soulHealth = SerialController.SoulHealth;

        _surfaceRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Populate variables from the SerialController.
        _soulHealth = SerialController.SoulHealth;

        // CHANGE COLOUR OF SURFACE BASED ON CURRENT INTERACTION LEVEL. (Rough/Gentle etc)
        _surfaceRenderer.material = _soulHealth switch
        {
            > 700 => lushGrass,
            < 500 => dryGround,
            _ => yellowGrass
        };
    }
}
