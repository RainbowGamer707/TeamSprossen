
#include <Adafruit_CircuitPlayground.h>
#include <Keyboard.h>
#include <math.h>

// Init timer
unsigned long last_time = 0;


void setup()
{
  // Initialize Circuit Playground library.
  CircuitPlayground.begin();

  // Uncomment below to start with NO LED's on.
  // CircuitPlayground.clearPixels();
  
  // Initialize Serial Connection
  Serial.begin(9600);
  Serial.setTimeout(10);

  // Set initial LED state
  CircuitPlayground.setPixelColor(2,   0, 255,   0);

   //SET BRIGHTNESS OF LED PIXELS ((0-255)
  CircuitPlayground.setBrightness(150);
}


void loop()
{  
  // Check the slide switch to see if the prototype is turned on.
  // if (!CircuitPlayground.slideSwitch()) {return;}
  
  // Print a heartbeat
  //if (millis() > last_time + 2000)
  //{
  //  Serial.println("Arduino functional");
  //  last_time = millis();
  //}
  
//Set LED's on Arduino based off soulHealth (s1) value (Received in serial from Unity) ---------------------------------

//  if(Serial.available() > 0) { 
    String s1 = Serial.readString(); 
    s1.trim();
    
    // Set to RED if code = 0
    if (s1 == "0") {
      for (int i = 0; i < 10; i++) {
        CircuitPlayground.setPixelColor(i, 0xFF0000); 
      }
    }
    // Set to ORANGE if code = 1
    if (s1 == "1") {
      for (int i = 0; i < 10; i++) {
        CircuitPlayground.setPixelColor(i, 0xcc5500);
      }
    }
    // Set to YELLOW if code = 2
    if (s1 == "2") {
      for (int i = 0; i < 10; i++) {
        CircuitPlayground.setPixelColor(i, 0xFFFF00);
      }
    }
    // Set to OCEAN if code = 3
    if (s1 == "3") {
      for (int i = 0; i < 10; i++) {
        CircuitPlayground.setPixelColor(i, 0x016064);
      }
    }
    // Set to TURQUOISE if code = 4
    if (s1 == "4") {
      for (int i = 0; i < 10; i++) {
        CircuitPlayground.setPixelColor(i, 0x30D5C8);
      }
    }
    // Set to GREEN if code = 5
    if (s1 == "5") {
      for (int i = 0; i < 10; i++) {
        CircuitPlayground.setPixelColor(i, 0x00FF00);
      }
    }
    // Set to RAINBOW if code = 6
    if (s1 == "6") {
      for (int i = 0; i < 10; i++) {
        CircuitPlayground.setPixelColor(0, 255,   0,   0);
        CircuitPlayground.setPixelColor(1, 128, 128,   0);
        CircuitPlayground.setPixelColor(2,   0, 255,   0);
        CircuitPlayground.setPixelColor(3,   0, 128, 128);
        CircuitPlayground.setPixelColor(4,   0,   0, 255);
        CircuitPlayground.setPixelColor(5, 0xFF0000);
        CircuitPlayground.setPixelColor(6, 0x808000);
        CircuitPlayground.setPixelColor(7, 0x00FF00);
        CircuitPlayground.setPixelColor(8, 0x008080);
        CircuitPlayground.setPixelColor(9, 0x0000FF);
      }
    }
//  }
  
//----------------------------------------------------------------------------------------------------------------------

  // Send some message when I receive an 'A' or a 'Z'.
  switch (Serial.read())
  {
    case 'A':
      Serial.println("That's the first letter of the abecedarium.");
      break;
    case 'Z':
      Serial.println("That's the last letter of the abecedarium.");
      break;
    }
}
