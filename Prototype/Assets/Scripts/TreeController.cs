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
        
        Debug.Log("TC - _soulHealth = " + _soulHealth);
        
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
    }
}
