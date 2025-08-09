#pragma once
#include <Arduino.h>

void button_init();
void button_handle(void (*onShort)(), void (*onLong)());
