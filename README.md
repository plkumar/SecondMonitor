# Second Monitor

Master: [![Build status](https://ci.appveyor.com/api/projects/status/9a6pw8no49n8irip/branch/master?svg=true)](https://ci.appveyor.com/project/Winzarten/secondmonitor/branch/master)

Project: [![Build status](https://ci.appveyor.com/api/projects/status/9a6pw8no49n8irip?svg=true)](https://ci.appveyor.com/project/Winzarten/secondmonitor)

## Introduction:

Second Monitor is Timing/Car information application for racing simulators. It displays the actual session information, timing information and basic car status idication.

![Screenshot](/_githubStuff/SecondMonitor.png)

**Information provided**:
* Session Information  
* Live timing  for each driver
* Pit information - in practice/qually it is a simple "in/out", in race it shows number of pit stops, and the last pit stop information  * Absolute/Relative driver ordering.
* Absolute/relative times 
* Live detla times between your current lap and previous + personal best
* Timing Circle (ellipse :D ) / Track Map: Position of cars on the track. The app needs one fully timed lap for to be able to show the track map. 
* Car Information - Brake temperatures, tyre temperatures + pressures, tyres condition, pedal and wheel postion, oil and water temperatures, pedals and wheel position.
* Fuel Monitor - Monitoring the current fuel levels and average consumption. Offering a quick color-coded information if the actual fuel state is enough to finish the session, and what is the required fuel delta.
* Fuel Calculator - Use consumption from previous/current session for required for fuel calculation.  
* Detailed lap summer for each driver available by double-clicking on the driver name
* Session Reports - Ability to automatically export session reports in xlsx file. Files containig race summary, lap overview for each driver, race progress and detailed lap information for players laps
* **Rating**  - New from v5.0, improve your single player experience using the [Rating Module](https://gitlab.com/winzarten/SecondMonitor/wikis/Rating-Tutorial)

From version 4.0.0 it is possible to use Second Monitor on separate computer, than the sim is running on.
Check the [Instructions.](https://gitlab.com/winzarten/SecondMonitor/wikis/Setting-up-Second-Monitor-on-separate-computer) 
  
## Telemetry Viewer:
  ![ScreenshotTV](/_githubStuff/TelemetryViewer/TelemetryViewer.png)


Telemetry Viewer allows to view and analyse the telemetry data that the main second monitor application captures during a session. The data are saved per completed lap and grouped into individual sessions. The basic usage of the application is explained in the topics below.

[See the Wiki](https://gitlab.com/winzarten/SecondMonitor/wikis/home)
  
## Supported Simulators
* R3E - Works out of the box
* Automobilista - Requires the rFactorSharedMemoryMapPlugin (https://github.com/dallongo/rFactorSharedMemoryMap). Can be automaticaly installed by the app. This is the same plugin that is used (and automatically installed) by CrewChief, so if you're using that, you're good to go.
* RFactor 1 - Same as Automobilista. Wasn't tested, but it is the same engine as AMS, and the same plugin is used for data, so it should work.
* RFactor 2 - Requires the rF2SharedMemoryMapPlugin (https://github.com/TheIronWolfModding/rF2SharedMemoryMapPlugin). Can be automaticaly installed by the app. This is the same plugin that is used (and automatically installed) by CrewChief, so if you're using that, you're good to go.
* Assetto Corsa - Requires custom plugin, than should be automatically installed when the app detect Assetto Corsa running. The plugin needs to be enabled in the options settings manualy. 
* Project Cars 2 - Works out of the box. Just be sure to enable the shared memory inside Project Cars 2 [options](http://www.eksimracing.com/f-a-q/configure-project-cars-to-use-shared-memory/)
* Project Cars - Limited functionality as split times and lap times are not provided by the sim api. Splits don't work at all, and lap timing is done by the app, so the will be slight difference between what is in the app and what is in sim. Big thanks to mr_belowski for allowing me to use his project cars pit coordinates from CrewChief :)

## Known Issues
[Known Issues - Second Monitor](https://gitlab.com/Winzarten/SecondMonitor/wikis/Known-Issues)

[Known Issues - Telemetry Viewer](https://gitlab.com/Winzarten/SecondMonitor/wikis/Known-Issues-(Telemetry-Viewer))

[Issues Tracker](https://gitlab.com/winzarten/SecondMonitor/issues) 


## Future Plans   
 - [ ] Track Recods tracking
 - [ ] Championship module (tied to Rating)
 - [ ] Basic setup "sanity check"
 - [ ] F1 2018/F1 2019 Support
 
## Support
For Support please use the application topic on r3e forums : https://forum.sector3studios.com/index.php?threads/secondmonitor-timing-status-app.9587/
 

## Installation

Check [release tab](https://gitlab.com/winzarten/SecondMonitor/releases) for latest version
