// File: src/encoder.cpp   // REPLACE THIS FILE
#include "encoder.h"

static volatile long s_pos = 0;
static volatile bool s_moved = false;

static uint8_t s_pinCLK = 255;
static uint8_t s_pinDT  = 255;
static uint8_t s_pinSW  = 255;

static uint32_t s_lastBtnMs = 0;
static volatile uint8_t s_state = 0;       // last 2-bit state
static volatile uint32_t s_lastEdgeUs = 0; // debounce for edges (µs)

// Quadrature state table (Gray code) – robust gegen Prellen
// Index: (prev<<2) | curr, Value: -1,0,+1 Schritt
static const int8_t s_dir[16] = {
  0,-1, 1, 0,
  1, 0, 0,-1,
 -1, 0, 0, 1,
  0, 1,-1, 0
};

static inline uint8_t rdAB()
{
  const uint8_t a = digitalRead(s_pinCLK);
  const uint8_t b = digitalRead(s_pinDT);
  return (a << 1) | b; // A=MSB, B=LSB
}

static IRAM_ATTR void isrEncoder()
{
  const uint32_t now = micros();
  if (now - s_lastEdgeUs < 300) return; // ~300 µs Edge-Filter gegen Prellen
  s_lastEdgeUs = now;

  const uint8_t curr = rdAB();
  const uint8_t idx  = ((s_state & 0x03) << 2) | (curr & 0x03);
  s_state = curr;

  const int8_t d = s_dir[idx];
  if (d != 0) {
    s_pos += d;      // +1 = CW, -1 = CCW
    s_moved = true;
  }
}

void encoder_begin(uint8_t pinCLK, uint8_t pinDT, uint8_t pinSW)
{
  s_pinCLK = pinCLK;
  s_pinDT  = pinDT;
  s_pinSW  = pinSW;

  pinMode(s_pinCLK, INPUT_PULLUP);
  pinMode(s_pinDT,  INPUT_PULLUP);
  pinMode(s_pinSW,  INPUT_PULLUP);

  s_state = rdAB();

  attachInterrupt(digitalPinToInterrupt(s_pinCLK), isrEncoder, CHANGE);
  attachInterrupt(digitalPinToInterrupt(s_pinDT),  isrEncoder, CHANGE);
}

long encoder_position()
{
  noInterrupts();
  long v = s_pos;
  interrupts();
  return v;
}

void encoder_setPosition(long v)
{
  noInterrupts();
  s_pos = v;
  interrupts();
}

bool encoder_moved()
{
  return s_moved;
}

void encoder_resetMoved()
{
  s_moved = false;
}

bool encoder_buttonPressed()
{
  const uint32_t now = millis();
  if (digitalRead(s_pinSW) == LOW && (now - s_lastBtnMs) > 250) {
    s_lastBtnMs = now;
    return true;
  }
  return false;
}
