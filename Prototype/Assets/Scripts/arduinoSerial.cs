using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class arduinoSerial : MonoBehaviour
{
    //INITIALISE SERIAL VARIABLES
    SerialPort port_S; //Two ports, one for (S)prossen and one for (T)ree
    SerialPort port_T;
    public string portName_S;
    public string portName_T;
    public string[] ports;

    //INTIALISE OTHER VARIABLES
    GameObject sprossen;

    // Start is called before the first frame update
    void Start()
    {
        //START SERIAL COMMUNICATION
        ports = SerialPort.GetPortNames();

        //SET PORT NAMES
        //COPY AND PASTE TO SET FOR YOUR OWN COMPUTER, COMMENT OUT OTHERS
        portName_S = "/dev/cu.usbmodem1413401";
        portName_T = "/dev/cu.usbmodem1413301";

        //SETUP PORT
        port_S = new SerialPort(portName_S, 9600);
        port_T = new SerialPort(portName_T, 9600);

        //SET TIMEOUTS TO LOW NUMBERS TO MAKE COMMUNICATION FAST
        port_S.ReadTimeout = 10;
        port_S.WriteTimeout = 10;
        port_S.DtrEnable = true;
        port_S.RtsEnable = true;

        port_T.ReadTimeout = 10;
        port_T.WriteTimeout = 10;
        port_T.DtrEnable = true;
        port_T.RtsEnable = true;

        //OPEN THE PORT
        port_S.Open();
        port_T.Open();

        //ASSIGN GAMEOBJECTS
        sprossen = GameObject.Find("sprossen");
    }

    // Update is called once per frame
    void Update()
    {
        if(port_S.IsOpen && port_T.IsOpen) //CHECK SERIAL PORT IS OPEN
        {
            if(port_S.BytesToRead > 0) //CHECK IF UNITY HAS RECIEVED ANY COMMUNICATION FROM ARDUINO
            {
                string inputStr = port_S.ReadLine().Trim();
                int inputInt = int.Parse(inputStr);

                //CHANGE COLOUR OF SPRITE BASED ON SPROSSEN MOVEMENT, 0 = PURPLE/NEUTRAL | 1 = GREEN/POSITIVE | 2 = RED/NEGATIVE
                if (inputInt == 0)
                {
                    sprossen.GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 0 / 255f, 255 / 255f); //CHANGE COLOUR OF UNITY SPRITE, RGB VALUES, BUT DIVIDE BY 255 AS UNITY ONLY ACCEPTS VALUES BETWEEN 0 AND 1
                    port_T.Write("0");
                } else if (inputInt == 1)
                {
                    sprossen.GetComponent<SpriteRenderer>().color = new Color(0 / 255f, 255 / 255f, 0 / 255f);
                    port_T.Write("1");
                } else if (inputInt == 2)
                {
                    sprossen.GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 0 / 255f, 0 / 255f);
                    port_T.Write("2");
                }
            }
        }
    }
}
