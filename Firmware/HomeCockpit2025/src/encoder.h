// File: src/encoder.h
#pragma once
#include <Arduino.h>

// Initialize KY-040 on given pins (all inputs use INPUT_PULLUP)
void encoder_begin(uint8_t pinCLK, uint8_t pinDT, uint8_t pinSW);

// Get/Set position (atomic)
long encoder_position();
void encoder_setPosition(long v);

// Movement flag
bool encoder_moved();
void encoder_resetMoved();

// Button edge (active LOW) with debounce (~250 ms)
bool encoder_buttonPressed();
