# Danse Macabre

## Installation Instructions

### 1. Download the project

Run the following in a terminal with git installed:
```git clone git@github.com:/himmannshu/ar-final-project```


### 2. Import the project into Unity Hub

Click on the `Add` button and select `Add project from disk`.
<img width="700" alt="image" src="https://github.com/user-attachments/assets/0a24fdb2-6269-4871-9292-276071232e47" />



### 3. Open the Unity project


### 4. Open the `combine-03` scene


### 5. Ensure the following packages are installed:

`Window -> Package Manager`
- Meta MR Utility Kit
- Meta XR Core SDK
- Meta XR Interaction SDK
	- Meta XR Interaction SDK Essentials
- XR Hands
- XR Interaction Toolkit


### 6. Switch the build platform to Android

* Navigate to build settings (`File -> Build Settings`)
* Select Android from the listed platforms
* Click on the switch platforms

### 7. Ensure the following feature groups are enabled in the OpenXR player settings for Android:

You can access the player settings from the build settings menu. 

- Hand Interaction Poses
- Hand Tracking Subsystem
- Meta Hand Tracking Aim
- Meta Quest Support


### 8. Build and run on your Quest 3

This will require a headset in developer mode to be physically connected to the PC.
An "Allow Debugging" prompt may appear in the headset before the headset is listed as connected on the PC-- make sure to allow it.

> **Note:**  
> Make sure that you have the `Combine-03` scene checked in the build settings menu. 



## Package Utilities

| Package                 | Utility                                                               |
|-------------------------|-----------------------------------------------------------------------|
| Meta MR Utility Kit     | Physical awareness (for finding spawn locations and enemy navigation) |
| Meta XR Core SDK        | Camera tracking, passthrough, augmentation                            |
| XR Hands                | Hand joints and pose detection                                        |

