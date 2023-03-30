//ARDUINO CODE FOR SPROSSEN LILYPAD
#include <Adafruit_CircuitPlayground.h>
#include <math.h>

//INTIALISE VARIABLES AND VALUES

void setup() {
  // put your setup code here, to run once:
  CircuitPlayground.begin();
  CircuitPlayground.clearPixels();

  //START SERIAL COMMUNICATION - ENSURE THEY MATCH UNITY VALUES
  Serial.begin(9600);
  Serial.setTimeout(10);

  //SET BRIGHTNESS OF LED PIXELS
  CircuitPlayground.setBrightness(255);

}

void loop() {
  // put your main code here, to run repeatedly:
  if(Serial.available() > 0) { //CHECK IF WE RECIEVED ANY SERIAL COMMUNICATION FROM UNITY
    String s1 = Serial.readString(); 
    s1.trim();
    }
}
