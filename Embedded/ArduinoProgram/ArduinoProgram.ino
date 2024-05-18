#include <SPI.h>
#include <Ethernet.h>
#include <ArduinoJson.h>
#include <EthernetUdp.h>
#include <DHT.h>

byte mac[] = {
  0xD0, 0xA6, 0xC3, 0xE4, 0xD2, 0x5F
};

unsigned int port = 4567;
EthernetUDP Udp;
bool hasResponded = false;
IPAddress serverIP(192, 234, 188, 44);

const int fanPin = 2;
#define DHTPIN 3     
#define DHTTYPE DHT22   

DHT dht(DHTPIN, DHTTYPE);

void setup() {
  Serial.begin(9600);
  while (!Serial) {
  }
  Serial.println("Ethernet UDP Example");

  Serial.println("Initialize Ethernet with DHCP:");
  if (Ethernet.begin(mac) == 0) {
    Serial.println("Failed to configure Ethernet using DHCP");
    if (Ethernet.hardwareStatus() == EthernetNoHardware) {
      Serial.println("Ethernet shield was not found. Sorry, can't run without hardware. :(");
    } else if (Ethernet.linkStatus() == LinkOFF) {
      Serial.println("Ethernet cable is not connected.");
    }
    while (true) {
      delay(1);
    }
  }
  Serial.print("My IP address: ");
  Serial.println(Ethernet.localIP());

  Udp.begin(port);

  pinMode(fanPin, OUTPUT);
  digitalWrite(fanPin, LOW);
  dht.begin();
}

void loop() {

  int packetSize = Udp.parsePacket();
  if (packetSize) {
    char packetBuffer[255];
    int len = Udp.read(packetBuffer, 255);
    if (len > 0) {
      packetBuffer[len] = 0;
      Serial.print("Received packet: ");
      Serial.println(packetBuffer);
      if (strcmp(packetBuffer, "FIND_ARDUINO") == 0) {
        IPAddress ip = Ethernet.localIP();
        byte ipArray[4] = { ip[0], ip[1], ip[2], ip[3] };

        serverIP = Udp.remoteIP();

        Udp.beginPacket(serverIP, Udp.remotePort());
        Udp.write(ipArray, 4);
        Udp.endPacket();
        Serial.println("Response sent with IP address:");
        Serial.println(serverIP);
      }
    }
  }

  checkUDPData();
  delay(1000);
}

void checkUDPData() {
  int packetSize = Udp.parsePacket();
  if (packetSize) {
    Serial.print("Received packet of size ");
    Serial.println(packetSize);
    char incomingPacket[packetSize + 1];
    Udp.read(incomingPacket, packetSize);
    incomingPacket[packetSize] = '\0';

    Serial.print("Received packet: ");
    Serial.println(incomingPacket);

    StaticJsonDocument<200> doc;
    DeserializationError error = deserializeJson(doc, incomingPacket);

    if (error) {
      Serial.print(F("deserializeJson() failed: "));
      Serial.println(error.f_str());
      return;
    }

    const char* command = doc["command"];

    if (strcmp(command, "off") == 0) {
      digitalWrite(fanPin, HIGH);
      Serial.println("Fan is OFF");
    } else if (strcmp(command, "on") == 0) {
      digitalWrite(fanPin, LOW);
      Serial.println("Fan is ON");
    } else if (strcmp(command, "data") == 0) {
      sendSensorData();
    } else {
      Serial.println("Invalid command");
    }
  }
}

void sendSensorData() {
  float temperature = dht.readTemperature();
  float humidity = dht.readHumidity();

  if (isnan(temperature) || isnan(humidity)) {
    Serial.println("Failed to read from DHT sensor!");
    return;
  }

  StaticJsonDocument<500> doc;
  doc["temperature"] = temperature;
  doc["humidity"] = humidity;

  char buffer[500];
  size_t n = serializeJson(doc, buffer);

  Udp.beginPacket(serverIP, port);
  Udp.write(buffer, n);
  if (Udp.endPacket() == 0) {
    Serial.println("Failed to send packet");
  } else {
    Serial.println("Packet sent successfully");
  }
}
