using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class SerialController : MonoBehaviour
{
    // INITIALISE SERIAL VARIABLES
    // Init 2 variables for serial data, one for (S)prossen and one for (T)ree
    private SerialPort _portS;
    private SerialPort _portT;
    public string portNameS;
    public string portNameT;
    public string[] ports;

    // INITIALISE OTHER VARIABLES
    public GameObject sprossen;
    public GameObject leftWall;
    public GameObject rightWall;
    public GameObject backWall;
    public GameObject floor;
    public GameObject tree;
    
    // Init variable to track health of persistant interaction (Tree). 
    private int _soulHealth;
    
    // Init variable to track status of Sprossen (Angry/Happy etc). 
    private float _sprossenStatus;

    // Start is called before the first frame update
    void Start()
    {
        // START SERIAL COMMUNICATION
        ports = SerialPort.GetPortNames();

        // SET PORT NAMES
        // COPY AND PASTE TO SET FOR YOUR OWN COMPUTER, COMMENT OUT OTHERS
        // portName_S = "/dev/cu.usbmodem1413401";
        // portName_T = "/dev/cu.usbmodem1413301";
        portNameS = "COM7";
        portNameT = "COM6";

        // SETUP PORTS
        _portS = new SerialPort(portNameS, 9600);
        _portT = new SerialPort(portNameT, 9600);

        // SET TIMEOUTS TO LOW NUMBERS TO MAKE COMMUNICATION FAST
        _portS.ReadTimeout = 10;
        _portS.WriteTimeout = 10;
        _portS.DtrEnable = true;
        _portS.RtsEnable = true;

        _portT.ReadTimeout = 10;
        _portT.WriteTimeout = 10;
        _portT.DtrEnable = true;
        _portT.RtsEnable = true;

        // OPEN THE PORT
        _portS.Open();
        _portT.Open();

        // ASSIGN GAME OBJECTS
        sprossen = GameObject.Find("Sprossen 01");
        
        // Set initial health of persistant interaction (Tree). 
        _soulHealth = 50;
        
        // Set initial value for status of Sprossen. 
        _sprossenStatus = 11.0F;
    }

    // Update is called once per frame
    void Update()
    {
        // CHECK SERIAL PORT IS OPEN
        if (!_portS.IsOpen || !_portT.IsOpen) return;
        
        // CHECK IF UNITY HAS RECEIVED ANY COMMUNICATION FROM ARDUINO
        if (_portS.BytesToRead <= 0) return; 
        
        // Collect and parse data from serial connection.
        var inputStr = _portS.ReadLine().Trim();
        var inputInt = int.Parse(inputStr);

        // Decay the _sprossenStatus back toward 0 (Neutral)
        switch (_sprossenStatus)
        {
            case > 0:
                _sprossenStatus -= 0.5F;
                break;
            case < 0:
                _sprossenStatus += 0.5F;
                break;
        }

        // Update _sprossenStatus with current interaction value. THIS WILL REQ TUNING.
        _sprossenStatus += inputInt;
        
        // CHANGE COLOUR OF SPRITE BASED ON SPROSSEN MOVEMENT, 0 = PURPLE/NEUTRAL, 1 = GREEN/POSITIVE, 2 = RED/NEGATIVE
        switch (_sprossenStatus)
        {
            // CHANGE COLOUR OF SPRITE, DIVIDE RGB VALUES BY 255 AS UNITY ONLY ACCEPTS VALUES BETWEEN 0 AND 1
            case < 10 and > -10:
                sprossen.GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 0 / 255f, 255 / 255f); 
                _portS.Write("0");
                break;
            case > 9:
                _soulHealth += 1;
                sprossen.GetComponent<SpriteRenderer>().color = new Color(0 / 255f, 255 / 255f, 0 / 255f);
                _portS.Write("1");
                break;
            case < -9:
                _soulHealth -= 1;
                sprossen.GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 0 / 255f, 0 / 255f);
                _portS.Write("2");
                break;
        }
        
        // Report Persistant Interaction (Tree) Status to 2nd Arduino
        _portT.Write(_soulHealth.ToString());
    }
}
