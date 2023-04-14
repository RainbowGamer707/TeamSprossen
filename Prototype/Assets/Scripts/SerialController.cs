using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
        
        // Write current SoulStatus level to serial for the Sprossen Arduino to set LED's.
        // Sends a number (String) between 0 and 6 depending on current SoulHealth (0 = Best, 2 = Worst)
        switch (SprossenStatus)
        {
            case <= 0 :
                _portS.Write("2");
                break;
            case > 0 and <= 10:
                _portS.Write("1");
                break;
            case > 10:
                _portS.Write("0");
                break;
        }

        // Report Sprossen Interaction Status to 1st Arduino
        _portS.Write(SprossenStatus.ToString(CultureInfo.InvariantCulture));
        
        //---------------------------------------------- TREE LOGIC ----------------------------------------------------
        
        // Update SoulHealth based on input from Arduino. THIS WILL REQ TUNING
        SoulHealth += inputInt;
        
        // Write current SoulHealth level to serial for the 'Tree' Arduino to set LED's.
        // Sends a number (String) between 0 and 6 depending on current SoulHealth (0 = Worst, 6 = Best)
        switch (SoulHealth)
        {
            case > 0 and < 10:
                _portT.Write("0");
                break;
            case > 9 and < 25:
                _portT.Write("1");
                break;
            case > 24 and < 40:
                _portT.Write("2");
                break;
            case > 39 and < 55:
                _portT.Write("3");
                break;
            case > 54 and < 70:
                _portT.Write("4");
                break;
            case > 69 and < 85:
                _portT.Write("5");
                break;
            case > 84 and < 100:
                _portT.Write("6");
                break;
            case > 99:
                _portT.Write("7");
                break;
        }

        //---------------------------------------------- ROOM LOGIC ----------------------------------------------------
        
        // Basic Instantaneous Interaction Logic (Changes wall colour depending on interactions (Gentle/Rough)
        RoomStatus = inputInt;
    }
}
