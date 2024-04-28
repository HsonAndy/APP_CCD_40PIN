#include "WS2812B.h"
#include <WiFi.h>
#include <WiFiUdp.h> 
#include <FreeRTOS.h>
#include <task.h>
#include <FlashMemory.h>
#include "Timer.h"
#include "LED.h"
#include "Output.h"
#include "Input.h" 
#include "WiFiConfig.h"
#include "MyJPEGDecoder.h"
#include "MyWS2812.h"
#include <ArduinoJson.h>
#include <SPI.h>
#include <SD.h>
#include <SoftwareSerial.h>
#include "DFRobot_MCP23017.h"

#define NUM_WS2812B_CRGB  450
#define NUM_OF_LEDS NUM_WS2812B_CRGB
#define SYSTEM_LED_PIN PA30
#define PIN_485_Tx_Eanble PB3

bool flag_udp_232back = false;
bool flag_JsonSend = false;
bool flag_writeMode = false;
String CardID[5];
String CardID_buf[5];

DFRobot_MCP23017 mcp(Wire, /*addr =*/0x20);//constructor, change the Level of A2, A1, A0 via DIP switch to revise the I2C address within 0x20~0x27.

WiFiConfig wiFiConfig;
int UDP_SemdTime = 0;
int Localport = 0;
IPAddress ServerIp;
int Serverport;
String GetwayStr;

bool flag_WS2812B_Refresh = false;
MyWS2812 myWS2812;

byte* framebuffer;

MyTimer MyTimer_BoardInit;
bool flag_boradInit = false;
MyLED MyLED_IS_Connented;

TaskHandle_t Core0Task1Handle;
TaskHandle_t Core0Task2Handle;
TaskHandle_t Core0Task3Handle;
TaskHandle_t Core0Task4Handle;

SoftwareSerial mySerial(PA8, PA7); // RX, TX
SoftwareSerial mySerial_485(PB2, PB1); // RX, TX

String Version = "Ver 1.0.2";
int counterNum_sensor1 = 0;
int counterNum_sensor2 = 0;
int counterNum_sensor3 = 0;
int counterNum_sensor4 = 0;


void setup() 
{   
    

}
void loop() 
{
   
   if(MyTimer_BoardInit.IsTimeOut() && !flag_boradInit)
   {     
        mySerial_485.begin(115200);
        pinMode(PIN_485_Tx_Eanble, OUTPUT);
        digitalWrite(PIN_485_Tx_Eanble, HIGH);  
        mySerial.begin(115200);        
        mySerial.println(Version);
        while(mcp.begin() != 0)
        {
          Serial.println("Initialization of the chip failed, please confirm that the chip connection is correct!");
          delay(1000);
        };
       
        MyLED_IS_Connented.Init(SYSTEM_LED_PIN);   
        MyLED_IS_Connented.BlinkTime = 500;
        MyTimer_BoardInit.StartTickTime(1000);
           
        xTaskCreate(Core0Task1,"Core0Task1", 1024,NULL,1,&Core0Task1Handle);
        IO_Init();
        flag_boradInit = true;
   }
   if(flag_boradInit)
   {
       sub_IO_Program(); 
   }
   
      
    
}

void Core0Task1( void * pvParameters )
{
    for(;;)
    {      
       
       if(flag_boradInit)
       {
             String sensor_str1 = String(counterNum_sensor1);
              int checksum_s1_3 = 2;
              int checksum_s1_2 = (counterNum_sensor1 >> 8) & 0xFF;
              int checksum_s1_1 = counterNum_sensor1 & 0xFF; 
              int checksum_s1_0 = 3;

              
              String sensor_str2 = String(counterNum_sensor2);
              int checksum_s2_3 = 4;
              int checksum_s2_2 = (counterNum_sensor2 >> 8) & 0xFF;
              int checksum_s2_1 = counterNum_sensor2 & 0xFF; 
              int checksum_s2_0 = 5;

              
              String sensor_str3 = String(counterNum_sensor3);
              int checksum_s3_3 = 6;
              int checksum_s3_2 = (counterNum_sensor3 >> 8) & 0xFF;
              int checksum_s3_1 = counterNum_sensor3 & 0xFF; 
              int checksum_s3_0 = 7;

              String sensor_str4 = String(counterNum_sensor4);
              int checksum_s4_3 = 8;
              int checksum_s4_2 = (counterNum_sensor4 >> 8) & 0xFF;
              int checksum_s4_1 = counterNum_sensor4 & 0xFF; 
              int checksum_s4_0 = 9;


               if (checksum_s1_1 > 9) 
             {
              checksum_s1_2 += checksum_s1_1 / 10;
              checksum_s1_1 %= 10;
             }
             byte str_checksum_s1[4] = {(checksum_s1_3),(checksum_s1_2 + 48), (checksum_s1_1 + 48), (checksum_s1_0)};
             if(sensor_str1.length() < 5) sensor_str1 = "0" + sensor_str1;
             if(sensor_str1.length() < 5) sensor_str1 = "0" + sensor_str1;
             if(sensor_str1.length() < 5) sensor_str1 = "0" + sensor_str1;
             if(sensor_str1.length() < 5) sensor_str1 = "0" + sensor_str1;
             if(sensor_str1.length() < 5) sensor_str1 = "0" + sensor_str1;


               if (checksum_s2_1 > 9) 
             {
              checksum_s2_2 += checksum_s2_1 / 10;
              checksum_s2_1 %= 10;
             }
             byte str_checksum_s2[4] = {(checksum_s2_3),(checksum_s2_2 + 48), (checksum_s2_1 + 48), (checksum_s2_0)};
             if(sensor_str2.length() < 5) sensor_str2 = "0" + sensor_str2;
             if(sensor_str2.length() < 5) sensor_str2 = "0" + sensor_str2;
             if(sensor_str2.length() < 5) sensor_str2 = "0" + sensor_str2;
             if(sensor_str2.length() < 5) sensor_str2 = "0" + sensor_str2;
             if(sensor_str2.length() < 5) sensor_str2 = "0" + sensor_str2;


               if (checksum_s3_1 > 9) 
             {
              checksum_s3_2 += checksum_s3_1 / 10;
              checksum_s3_1 %= 10;
             }
             byte str_checksum_s3[4] = {(checksum_s3_3),(checksum_s3_2 + 48), (checksum_s3_1 + 48), (checksum_s3_0)};
             if(sensor_str3.length() < 5) sensor_str3 = "0" + sensor_str3;
             if(sensor_str3.length() < 5) sensor_str3 = "0" + sensor_str3;
             if(sensor_str3.length() < 5) sensor_str3 = "0" + sensor_str3;
             if(sensor_str3.length() < 5) sensor_str3 = "0" + sensor_str3;
             if(sensor_str3.length() < 5) sensor_str3 = "0" + sensor_str3;

              if (checksum_s4_1 > 9) 
             {
              checksum_s4_2 += checksum_s4_1 / 10;
              checksum_s4_1 %= 10;
             }
             byte str_checksum_s4[4] = {(checksum_s4_3),(checksum_s4_2 + 48), (checksum_s4_1 + 48), (checksum_s4_0)};
             if(sensor_str4.length() < 5) sensor_str4 = "0" + sensor_str4;
             if(sensor_str4.length() < 5) sensor_str4 = "0" + sensor_str4;
             if(sensor_str4.length() < 5) sensor_str4 = "0" + sensor_str4;
             if(sensor_str4.length() < 5) sensor_str4 = "0" + sensor_str4;
             if(sensor_str4.length() < 5) sensor_str4 = "0" + sensor_str4;
             
             mySerial.write(str_checksum_s1 , 4);
             mySerial.write(str_checksum_s2 , 4);
             mySerial.write(str_checksum_s3 , 4);
             mySerial.write(str_checksum_s4 , 4);
             mySerial.println();
             
             mySerial.flush();            
             digitalWrite(PIN_485_Tx_Eanble, HIGH);  
             mySerial_485.write(str_checksum_s1 , 4);
             mySerial_485.write(str_checksum_s2 , 4);
             mySerial_485.write(str_checksum_s3 , 4);
             mySerial_485.write(str_checksum_s4 , 4);

             mySerial_485.println();
             mySerial_485.flush();
             MyLED_IS_Connented.Blink();                  
       }
          
       delay(60);
    }
    
}
