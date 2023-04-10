#include <Adafruit_CircuitPlayground.h>
#include <Keyboard.h>
#include <math.h>

// Init timer
unsigned long last_time = 0;

// Current health of persistant interaction. 
int soulHealth = 50;

// Init sound pressure level value.
float SPLvalue;

void setup()
{
  // Initialize Circuit Playground library.
  CircuitPlayground.begin();

  // Uncomment below to start with NO LED's on.
  // CircuitPlayground.clearPixels();
  
  // Initialize Serial Connection
  Serial.begin(9600);

  // Set initial LED state
  CircuitPlayground.setPixelColor(2,   0, 255,   0);

}

void loop()
{  
  // Check the slide switch to see if the prototype is turned on.
  if (!CircuitPlayground.slideSwitch()) {return;}
  
  // Print a heartbeat
  if (millis() > last_time + 2000)
  {
    Serial.println("Arduino functional");
    last_time = millis();
  }

//---------------------------------------------------------------------------

// Using microphone to detect sound. soulHeath will be affected by value
// Will be moved to different board eventually
  
  // Calculate Sound Pressure Level. (In 10Ms batches)
  SPLvalue = CircuitPlayground.mic.soundPressureLevel(10);



  // Send a different keystroke to Unity depending on the SPL reading
  // Also play a different tone, depending on SPL, as feedback for the user
  if (SPLvalue > 65.0 && SPLvalue <80.0) {
    soulHealth += 1;
    CircuitPlayground.playTone(100, 20);
  }

  if (SPLvalue > 80.0 && SPLvalue < 90.0) {
    CircuitPlayground.playTone(500, 20);
  }

  if (SPLvalue > 90.0) {
    soulHealth += -1;
    CircuitPlayground.playTone(1000, 20);
  }

  // See current soulHealth in serial. Disable if you want to see SPL level.
  Serial.println(soulHealth);

  // Uncomment to see SPLvalue in Serial Monitor.
  // Serial.print("Sound Sensor SPL: ");
  // Serial.println(SPLvalue);
   
//---------------------------------------------------------------------------

//Set LED's on Arduino based off soulHealth value.

  // Set to RED if under 16
  if (soulHealth < 16.0) {
    for (int i = 0; i < 10; i++) {
      CircuitPlayground.setPixelColor(i, 0xFF0000);
    }
  }

  // Set to ORANGE if between 16 & 24
  if (soulHealth > 15 && soulHealth < 25.0) {
    for (int i = 0; i < 10; i++) {
      CircuitPlayground.setPixelColor(i, 0xcc5500);
    }
  }

  // Set to YELLOW if between 25 & 49
  if (soulHealth > 24.0 && soulHealth < 50.0) {
    for (int i = 0; i < 10; i++) {
      CircuitPlayground.setPixelColor(i, 0xFFFF00);
    }
  }
  
  // Set to 'OCEAN' (Blue) if between 50 & 64
  if (soulHealth > 49.0 && soulHealth < 65.0) {
    for (int i = 0; i < 10; i++) {
      CircuitPlayground.setPixelColor(i, 0x016064);
    }
  }

  // Set to TURQUOISE if between 65 & 79
  if (soulHealth > 64.0 && soulHealth < 80.0) {
    for (int i = 0; i < 10; i++) {
      CircuitPlayground.setPixelColor(i, 0x30D5C8);
      }
  }

  // Set to GREEN if between 80 & 99
  if (soulHealth > 79.0 && soulHealth < 100.00) {
    for (int i = 0; i < 10; i++) {
      CircuitPlayground.setPixelColor(i, 0x00FF00);
      }
    }

  // Set to RAINBOW if 100 or over
  if (soulHealth > 99.0) {
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

//---------------------------------------------------------------------------

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
