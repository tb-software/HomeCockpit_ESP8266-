// File: src/main.cpp
#include <Arduino.h>
#include "encoder.h"

// Anschlussplan (HW-040 ↔ ESP8266 NodeMCU ↔ Kabelfarbe laut Foto)
// GND → GND      → Weiß
// +   → 3V3      → Pink
// SW  → D3/GPIO0 → Blau   (Button, INPUT_PULLUP)
// DT  → D2/GPIO4 → Grün   (Encoder B)
// CLK → D1/GPIO5 → Gelb   (Encoder A)

#define PIN_CLK D1  // GPIO5 – Gelb
#define PIN_DT  D2  // GPIO4 – Grün
#define PIN_SW  D3  // GPIO0 – Blau

void setup() {
  Serial.begin(115200);
  delay(200);

  encoder_begin(PIN_CLK, PIN_DT, PIN_SW);

  Serial.println();
  Serial.println(F("KY-040 @ ESP8266 gestartet"));
  Serial.println(F("Mapping: CLK=D1(gelb), DT=D2(gruen), SW=D3(blau), +3V3(pink), GND(weiss)"));
}

void loop() {
  // Heartbeat, damit man sofort sieht, dass das Board läuft
  static uint32_t lastBeat = 0;
  if (millis() - lastBeat >= 1000) {
    lastBeat = millis();
    Serial.println(F("alive"));
  }

  if (encoder_moved()) {
    encoder_resetMoved();
    Serial.print(F("Encoder: "));
    Serial.println(encoder_position());
  }

  if (encoder_buttonPressed()) {
    Serial.println(F("Button: PRESS"));
  }
}
