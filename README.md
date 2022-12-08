# HolodeckPrivacyAndSafety
This repository contains the code for simulations of the threat models and solution models for holodeck applications.
The simulations are implemented in Unity (version 2021.3.13f1). To see instructions to install Unity refer to - [https://unity.com]{https://unity.com}

Once Unity is installed, the projects in the repe can be opened using Unity Hub and run in Unity.

The privacy simulations can be found in the project HolodeckPrivacySimulations. Navigate to the Assets/Scenes to find the different scenes. These contain 4 scenes:
- SampleScene - the base scene describing the threat model
- MonitorScene - the first solution, involving monitoring the environment and turning the holodeck off in the presence of an adversary
- ARScene - the second solution which renders the physical structure using the holodeck and private labels using an AR headset
- ARHapticsScene - the third solutions, which uses the drones as Encountered-Type Haptic Devices to keep the structure private as well.

The folder evaluate_metrics contained Python code to calculate the privacy metrics on views obtained from the different solutions.
