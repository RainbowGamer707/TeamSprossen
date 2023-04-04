//ARDUINO CODE FOR SPROSSEN LILYPAD
#include <Adafruit_CircuitPlayground.h>
#include <math.h>

//INTIALISE VARIABLES AND VALUES
#define LOW_SHAKE_THRESHOLD  15 // Low acceleration threshold for shake detection, the smaller the value, the more sensitive
#define HIGH_SHAKE_THRESHOLD 25 //High-range threshold to move from positive to negative reaction
float X, Y, Z, totalAccel;

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
  // COMPUTE TOTAL ACCELERATION
  X = 0;
  Y = 0;
  Z = 0;
  for (int i=0; i<10; i++) {
    X += CircuitPlayground.motionX();
    Y += CircuitPlayground.motionY();
    Z += CircuitPlayground.motionZ();
    delay(1);
  }
  X /= 10;
  Y /= 10;
  Z /= 10;

  totalAccel = sqrt(X*X + Y*Y + Z*Z);

  //CHANGE LEDS BASED ON AGGRESIVENESS
  if (totalAccel > LOW_SHAKE_THRESHOLD && totalAccel < HIGH_SHAKE_THRESHOLD) {
    Serial.println(1); //1 = Positive Reaction, Green colour
  }else if (totalAccel > HIGH_SHAKE_THRESHOLD) {
    Serial.println(2); //2 - Negative Reaction, Red colour
  }else {
    Serial.println(0); //0 - Neutral State, Purple colour
  }
  
  if(Serial.available() > 0) { //CHECK IF WE RECIEVED ANY SERIAL COMMUNICATION FROM UNITY
    String s1 = Serial.readString(); 
    s1.trim();
    }
}
