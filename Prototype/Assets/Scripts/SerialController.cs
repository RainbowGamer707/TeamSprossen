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
        //portNameS = "/dev/cu.usbmodem1423401";
        //portNameT = "/dev/cu.usbmodemHIDPC1";
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

        // Set initial health of persistant interaction (Tree).
        SoulHealth = 500;
        
        // Set initial value for status of Sprossen. 
        SprossenStatus = 0.0F;
        
        // Set initial value for room.
        RoomStatus = 0.0F;
    }

    // Update is called once per frame
    void Update()
    {
        // CHECK SERIAL PORT IS OPEN
        if (!_portS.IsOpen || !_portT.IsOpen)
        {
            Debug.Log("Port is not open");
            return;
        }
           
        // CHECK IF UNITY HAS RECEIVED ANY COMMUNICATION FROM ARDUINO
        if (_portS.BytesToRead <= 0) return; 
        
        // Collect and parse data from serial connection.
        var inputStr = _portS.ReadLine().Trim();
        var inputInt = int.Parse(inputStr);
        //Debug.Log("SC - inputInt = " + inputInt);

        //------------------------------------------- SPROSSEN LOGIC ---------------------------------------------------
        
        // Decay the _sprossenStatus back toward 0 (Neutral)
        switch (SprossenStatus)
        {
            case > 1:
                SprossenStatus -= 2;
                break;
            case < 0:
                SprossenStatus += 2;
                break;
        }

        if (SprossenStatus >= 17)
        {
            SprossenStatus = 16;
        }

        if (SprossenStatus <= -7)
        {
            SprossenStatus = -6;
        }

        // Update SprossenStatus with current interaction value. THIS WILL REQ TUNING.
        SprossenStatus += inputInt;
        
        //Debug.Log("SerialC - _sprossenStatus = " + SprossenStatus);
        
        // Write current SoulStatus level to serial for the Sprossen Arduino to set LED's.
        // Sends a number (String) between 0 and 6 depending on current SoulHealth (0 = Best, 2 = Worst)
        switch (SprossenStatus)
        {
            case < 0 :
                _portS.Write("2");
                break;
            case >= 0 and <= 10:
                _portS.Write("1");
                break;
            case > 10:
                _portS.Write("0");
                break;
        }

        //---------------------------------------------- TREE LOGIC ----------------------------------------------------
        
        // Update SoulHealth based on input from Arduino. THIS WILL REQ TUNING
        SoulHealth += inputInt;
        
        // Write current SoulHealth level to serial for the 'Tree' Arduino to set LED's.
        // Sends a number (String) between 0 and 6 depending on current SoulHealth (0 = Worst, 6 = Best)
        switch (SoulHealth)
        {
            case <= 100:
                _portT.Write("0");
                break;
            case > 100 and <= 300:
                _portT.Write("1");
                break;
            case > 300 and < 500:
                _portT.Write("2");
                break;
            case >= 500 and <= 650:
                _portT.Write("3");
                break;
            case > 650 and <= 850:
                _portT.Write("4");
                break;
            case > 850 and <= 999:
                _portT.Write("5");
                break;
            case >= 1000:
                _portT.Write("6");
                break;
        }

        //---------------------------------------------- ROOM LOGIC ----------------------------------------------------
        
        // Basic Instantaneous Interaction Logic (Changes wall colour depending on interactions (Gentle/Rough)
        RoomStatus = inputInt;
    }
}
