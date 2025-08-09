#include "button.h"
#include "config.h"

uint32_t btnTimestamp = 0;

void button_init() {
  pinMode(PIN_SW, INPUT_PULLUP);
  if (DEBUG) Serial.println(F("[BTN] Button initialisiert"));
}

void button_handle(void (*onShort)(), void (*onLong)()) {
  static uint8_t state = 0;
  bool level = digitalRead(PIN_SW);
  uint32_t now = millis();

  switch (state) {
    case 0:
      if (level == LOW) {
        state = 1;
        btnTimestamp = now;
      }
      break;

    case 1:
      if (now - btnTimestamp >= BUTTON_DEBOUNCE_MS)
        state = (level == LOW) ? (btnTimestamp = now, 2) : 0;
      break;

    case 2:
      if (level == HIGH) {
        state = 4;
        btnTimestamp = now;
      } else if (now - btnTimestamp >= BUTTON_LONG_MS) {
        if (onLong) onLong();
        state = 3;
      }
      break;

    case 3:
      if (level == HIGH) {
        state = 4;
        btnTimestamp = now;
      }
      break;

    case 4:
      if (now - btnTimestamp >= BUTTON_DEBOUNCE_MS) {
        if (level == HIGH) {
          if (state != 3 && onShort) onShort();
          state = 0;
        } else {
          state = 2;
        }
      }
      break;
  }
}
