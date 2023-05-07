//ARDUINO CODE FOR SPROSSEN LILYPAD
#include <Adafruit_CircuitPlayground.h>
#include <math.h>

// Init timer
unsigned long last_time = 0;

//INTIALISE VARIABLES AND VALUES
#define LOW_SHAKE_THRESHOLD  10 // Low acceleration threshold for shake detection, the smaller the value, the more sensitive
#define HIGH_SHAKE_THRESHOLD 13 //High-range threshold to move from positive to negative reaction
float X, Y, Z, totalAccel;

// Init sound pressure level value.
float SPLvalue;

// Init modifier value. Value is communicated through serial connection.
int interactionValueModifier = 0;

// Pressure sensor on A11 (#12) with ground and +3.3V
float pressureSensor = A11;

// Pressure value variable.
float currentPressure = 0.0f;


void setup() {
  // put your setup code here, to run once:
  CircuitPlayground.begin();
  CircuitPlayground.clearPixels();

  //START SERIAL COMMUNICATION - ENSURE THEY MATCH UNITY VALUES
  Serial.begin(9600);
  Serial.setTimeout(10);

  //SET BRIGHTNESS OF LED PIXELS (0-255)
  CircuitPlayground.setBrightness(150);
}


void loop() {

  // Reset Modifier
  // interactionValueModifier = 0;

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
  
// UPDATE MODIFIER VALUE BASED ON AGGRESIVENESS ----------------------------------------------------------------------
  
  // +1 for Positive Reaction, Green colour
  if (totalAccel > LOW_SHAKE_THRESHOLD && totalAccel < HIGH_SHAKE_THRESHOLD) {
    interactionValueModifier += 1; 
  }

  // -1 for Negative Reaction, Red colour
  if (totalAccel > HIGH_SHAKE_THRESHOLD) {
    interactionValueModifier += -1;; 
  }

// Use microphone to detect sound. modifier will be affected by value. -----------------------------------------------
  
  // Calculate Sound Pressure Level. (In 10Ms batches)
  SPLvalue = CircuitPlayground.mic.soundPressureLevel(10);

  // Updates modifier based on sound volume level
  // Quiet/Soothing volume adds 1 to Modifier
  if (SPLvalue > 52.0 && SPLvalue < 60.0) {
    interactionValueModifier += 1;
  }

  // There is a small buffer between adding and losing modifier value.

  // Loud volume subtracts 1 from the modifier
  if (SPLvalue > 65.0) {
    interactionValueModifier += -1;
  }

  // Uncomment to see SPLvalue in Serial Monitor (For calibrating sound levels).
  //Serial.print("Sound Sensor SPL: ");
  //Serial.println(SPLvalue);

//----------------------------------------------------------------------------------------------------------------------
// PRESSURE SENSOR

delay(100);

  // Assign the input from the pressure sensor to its variable.
  currentPressure = analogRead(A11);

  // Send pressure data to serial
  //Serial.print("Pressure Sensor: ");
  //Serial.println(currentPressure);

  // Process the raw pressure measurement into the right format for the speed modification tool.
  if (currentPressure > 390.0 && currentPressure < 700.0) {
      interactionValueModifier += 1;
      Serial.println("+1");
  }
  if (currentPressure > 899) {
      interactionValueModifier += -1;
      Serial.println("-1");
  }

// Update Unity by sending modifier total to serial ------------------------------------------------------------------

  // Will send via serial every 1000 milliseconds. Change the value to make more/less frequent.
  if (millis() > last_time + 1000)
  {
    Serial.println(interactionValueModifier);
    last_time = millis();
    // Reset Modifier
    interactionValueModifier = 0;
  }
  
//  Serial.println(interactionValueModifier);
//  interactionValueModifier = 0;
  
// CHANGE LEDS BASED ON VALUE FROM SERIAL (UNITY) --------------------------------------------------------------------
  
  //CHECK IF WE RECIEVED ANY SERIAL COMMUNICATION FROM UNITY AND UPDATE LED'S
  if(Serial.available() > 0) { 
    String s1 = Serial.readString(); 
    s1.trim();
    if (s1 == "1") {
      for (int i = 0; i < 10; i++) {
        CircuitPlayground.setPixelColor(i, 255, 0, 255); 
      }
    } else if (s1 == "0") {
      for (int i = 0; i < 10; i++) {
        CircuitPlayground.setPixelColor(i, 0, 255, 0); //SETTING TO GREEN
      }
    } else if (s1 == "2") {
      for (int i = 0; i < 10; i++) {
        CircuitPlayground.setPixelColor(i, 255, 0, 0); //SETTING TO RED 
      }
    }
  }
   
//----------------------------------------------------------------------------------------------------------------------

  //Speaker Test
  // Sweep frequency from 50 to 6000 in steps of 50Hz
//  int freq;
//  for (freq=50; freq<=6000; freq=freq+50) {
//    piezo.beep(freq, 200);
//    delay (50);
//  }

}
