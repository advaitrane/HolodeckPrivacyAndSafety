# HolodeckPrivacyAndSafety
This repository contains the code for simulations of the threat models and solution models for holodeck applications.
The simulations are implemented in Unity (version 2021.3.13f1). To see instructions to install Unity refer to - [https://unity.com](https://unity.com)

Once Unity is installed, the projects in the repo can be accessed using Unity Hub and opened in Unity.

The privacy simulations can be found in the project HolodeckPrivacySimulations. Navigate to the Assets/Scenes to find the different scenes. These contain 4 scenes:
- SampleScene - the base scene describing the threat model
- MonitorScene - the first solution, involving monitoring the environment and turning the holodeck off in the presence of an adversary
- ARScene - the second solution which renders the physical structure using the holodeck and private labels using an AR headset
- ARHapticsScene - the third solutions, which uses the drones as Encountered-Type Haptic Devices to keep the structure private as well.

Each of these scenes can be run in Unity (by hitting the play button at the top) to observe the scene play out. The main user in the scenes can be controlled using the arrow keys. Moving to the spot marked by the circular plate will start up the holodeck. The user's view can be accessed on Display 2. The unauthorized viewer's view can be found on Display 3.

Videos of all the scenes can be found in the DemoVideos folder.

The folder evaluate_metrics contained Python code to calculate the privacy metrics on views obtained from the different solutions.

For more details refer to our report.

