# CoolFan - Simple cooling system interface

## Contents
1. [Overwiev](#overview)
2. [Embedded](#embedded)
3. [Backend](#backend)
4. [Frontend](#frontend)

### Overview

CoolFan is an app which allows user to interact with their colling device. Project contains three main layers: embedded, backend and front end. Main used technologies are C++ for embedded part and C# for Higher parts. Also we used web technologies e.g. HTML, CSS, Bootstrap, JS.

### Embedded
#### Components:
- Arduino Uno
- Ethernet module W5500
- DHT 22
- Relay HL-58S
- Fan SilentiumPC Zephyr 120 MM PWM

The embedded part was made using the Arduino Uno microcontroller. The premise of the project is to create a simple cooling system. Therefore, an *Arduino Uno* microcontroller was used as the control for the connected *Silentiumpc Zephyr 120 MM PWM* fan. To enable the fan to switch on and off automatically, a module for measuring temperature and humidity *DHT 22* is also connected.

Arduino is communicationg with our program by sending UDP packets, and responding for received commands.


## High Level
### Frontend


### Backend


Contributors:
Zuzanna Cebula and Maja Chlipała
