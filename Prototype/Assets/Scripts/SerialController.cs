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

    // Init variable to track health of persistant interaction (Tree). Used heavily in TreeController
    public static int SoulHealth;
    
    // Init variable to track status of Sprossen (Angry/Happy etc). Used heavily in SprossenController
    public static float SprossenStatus;

    // Init variable to control the status of the environment. Used heavily in RoomController.
    public static float RoomStatus;

    // Start is called before the first frame update
    void Start()
    {
        // START SERIAL COMMUNICATION
        ports = SerialPort.GetPortNames();

        // SET PORT NAMES
        // COPY AND PASTE TO SET FOR YOUR OWN COMPUTER, COMMENT OUT OTHERS
        // portName_S = "/dev/cu.usbmodem1413401";
        // portName_T = "/dev/cu.usbmodem1413301";
        portNameS = "COM5";
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

        //todo - Set initial health of persistant interaction (Tree). NEEDS LOGIC
        SoulHealth = 50;
        
        // Set initial value for status of Sprossen. 
        SprossenStatus = 0.0F;
        
        // Set initial value for room.
        RoomStatus = 0.0F;
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

        //------------------------------------------- SPROSSEN LOGIC ---------------------------------------------------
        
        // Decay the _sprossenStatus back toward 0 (Neutral)
        switch (SprossenStatus)
        {
            case > 0:
                SprossenStatus -= 0.5F;
                break;
            case < 0:
                SprossenStatus += 0.5F;
                break;
        }

        // Update SprossenStatus with current interaction value. THIS WILL REQ TUNING.
        SprossenStatus += inputInt;
        
        //---------------------------------------------- TREE LOGIC ----------------------------------------------------
        
        // Update SoulHealth based on input from Arduino. THIS WILL REQ TUNING
        SoulHealth += inputInt;
        
        // Report Persistant Interaction (Tree) Status to 2nd Arduino
        _portT.Write(SoulHealth.ToString());
        
        //---------------------------------------------- ROOM LOGIC ----------------------------------------------------
        
        // Basic Instantaneous Interaction Logic (Changes wall colour depending on interactions (Gentle/Rough)
        RoomStatus = inputInt;
    }
}
