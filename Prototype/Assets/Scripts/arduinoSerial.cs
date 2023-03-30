using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class arduinoSerial : MonoBehaviour
{
    //INITIALISE SERIAL VARIABLES
    SerialPort port;
    public string portName;
    public string[] ports;

    //INTIALISE OTHER VARIABLES

    // Start is called before the first frame update
    void Start()
    {
        //START SERIAL COMMUNICATION
        ports = SerialPort.GetPortNames();

        //SET PORT NAMES
        //COPY AND PASTE TO SET FOR YOUR OWN COMPUTER, COMMENT OUT OTHERS
        portName = "/dev/cu.usbmodem1412101";

        //SETUP PORT
        port = new SerialPort(portName, 9600);

        //SET TIMEOUTS TO LOW NUMBERS TO MAKE COMMUNICATION FAST
        port.ReadTimeout = 10;
        port.WriteTimeout = 10;
        port.DtrEnable = true;
        port.RtsEnable = true;

        //OPEN THE PORT
        port.Open();

        //ASSIGN GAMEOBJECTS
    }

    // Update is called once per frame
    void Update()
    {
        if(port.IsOpen) //CHECK SERIAL PORT IS OPEN
        {
            if(port.BytesToRead > 0) //CHECK IF UNITY HAS RECIEVED ANY COMMUNICATION FROM ARDUINO
            {

            }
        }
    }
}
