#pragma once
#include <Arduino.h>

// Debug-Modus einschalten (true/false)
#define DEBUG true

// ---------------- Pinbelegung ESP8266 NodeMCU ----------------
constexpr uint8_t PIN_CLK = D1; // GPIO5
constexpr uint8_t PIN_DT  = D2; // GPIO4
constexpr uint8_t PIN_SW  = D3; // GPIO0

// ---------------- Encoder-Einstellungen ---------------------
constexpr bool WRAP_VALUES    = false;  // true: wrap zwischen MIN..MAX, false: clamp
constexpr long MIN_VALUE      = 0;
constexpr long MAX_VALUE      = 100;
constexpr int  STEP_PER_CLICK = 1;

// ---------------- Button-Einstellungen ----------------------
constexpr uint32_t BUTTON_DEBOUNCE_MS = 20;
constexpr uint32_t BUTTON_LONG_MS     = 600;

// ---------------- Serial-Ausgabe ----------------------------
constexpr uint32_t REPORT_INTERVAL_MS = 50; // ms
