//ARDUINO CODE FOR TREE/HEART LILYPAD
#include <Adafruit_CircuitPlayground.h>
#include <math.h>

//INTIALISE VARIABLES AND VALUES

void setup() {
  // put your setup code here, to run once:
  CircuitPlayground.begin();
  CircuitPlayground.clearPixels();
  CircuitPlayground.setAccelRange(LIS3DH_RANGE_8_G);

  //START SERIAL COMMUNICATION - ENSURE THEY MATCH UNITY VALUES
  Serial.begin(9600);
  Serial.setTimeout(10);

  //SET BRIGHTNESS OF LED PIXELS
  CircuitPlayground.setBrightness(255);

  //SET INTIAL LED COLOUR
  for (int i = 0; i < 10; i++) {
    CircuitPlayground.setPixelColor(i, 255, 0, 255); //SETTING TO PURPLE USING RGB VALUES
  }

}

void loop() {
  // put your main code here, to run repeatedly:
  if(Serial.available() > 0) { //CHECK IF WE RECIEVED ANY SERIAL COMMUNICATION FROM UNITY
    String s1 = Serial.readString(); 
    s1.trim();
    if (s1 == "0") {
      for (int i = 0; i < 10; i++) {
        CircuitPlayground.setPixelColor(i, 255, 0, 255); 
      }
    } else if (s1 == "1") {
      for (int i = 0; i < 10; i++) {
        CircuitPlayground.setPixelColor(i, 0, 255, 0); //SETTING TO GREEN
      }
    } else if (s1 == "2") {
      for (int i = 0; i < 10; i++) {
        CircuitPlayground.setPixelColor(i, 255, 0, 0); //SETTING TO RED 
      }
    }
  }
}
