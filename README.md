# PhotoStation Frame

## Overview

The PhotStation Frame is a digital picture frame software as UWP app with support for Windows IoT Core with a special look at Raspberry Pi 3 that accesses your Synology DiskStation with installed PhotoStation software. 

### Preconditions 
- Synology DiskStation with Phote Station 6
- Windows 10 (Fall Creators Update+)
- Visual Studio with Windows Universal Platform Toolset (2017+)

### Features

- Runs on Raspberry Pi with Windows IoT Core (Tested on RPi3)
- Support for HTTP & HTTPS 
- Show albums and smart albums
- Random sorting 
- Supports larg image sets (Tested with 18k images from smart album an RPi3)
- Experimentel Support of Portait mode

## Setup

- TBD

## Roadmap

### Planned
- Add Companion App for Configuration over BLE (Xamarin.Forms)
- Build hardware picture frame and publish HowTo/Documentation 
- Add image assets and publish app to store
- Energy management (QuitHours, MonitorPowerControl - Hardware)
- Add I2C Accelerometer Sensor for Portait and Landscape Support 
- Image caching

### v.NextNext 

- VoiceControl
- Presence Sensor for power control 
- Support for other Cloud providers
- Some cool stuff with [VisionService](https://azure.microsoft.com/en-us/services/cognitive-services/directory/vision/)
- Cross Plattform App



