<p align="center"><img src="https://imgur.com/au46wFG.png"></p>

<p align="center"><a href="https://youtu.be/TiChA9fTR-4" target="_blank"><img src="https://imgur.com/gaq4sQt.png"></a></p>

# Simple Interface

### Introduction
This is a simple Unity3D interface that helps to put prefabs easily into the editor.
<p align="center"><img src="https://imgur.com/jduzq4M.jpg"></p>

## Table of Contents:
- [Prerequisites](#prerequisites)
- [Stable Build](#stable-build)
- [Installation](#installation)
- [Setup](#setup)
- [Features](#features)
  - [Place Prefab](#place-prefab)
  - [Limit Placement](#limit-placement)
  - [Drag](#drag)
  - [Fixed Position](#fixed-position)
  - [Offset Position](#offset-position)
  - [Fixed Rotation](#fixed-rotation)
  - [Random Rotation](#random-rotation)
  - [Fixed Scale](#fixed-scale)
  - [Random Scale](#random-scale)
  - [Hotkeys](#hotkeys)
- [Developer](#developer)
  - [SimpleInterfaceWindow](#simpleinterfacewindow)
  - [BaseLayout](#baselayout)
- [Bug Fixes](#bug-fixes)
- [Versioning](#versioning)
- [Authors](#authors)
- [License](#license)

## Prerequisites
#### Unity Game Engine
Unity3D version of **2020.3.3f1** and above should work. The main branch version is **2020.3.3f1**.
***
## Stable Build
[Stable-v1.0.1](https://github.com/deadlykam/SimpleInterface/tree/Stable-v1.0.1) is the latest stable build of the project. The unitypackage for this project can also be found there. If development is going to be done on this project then it is advised to branch off of any Stable branches because they will NOT be changed or update except for README.md. Any other branches are subjected to change including the main branch.
***
## Installation
1. First download the latest [SimpleInterface-vx.x.x.unitypackage](https://github.com/deadlykam/SimpleInterface/releases/tag/v1.0.1) from the latest Stable build. 
2. Once download is completed open up the Unity project you want to use this project in.
3. Now go to Assets -> Import Package -> Custom Package.
4. Select the SimpleInterface-vx.x.x you just downloaded and open it.
5. Make sure everything is selected in the Import Unity Package otherwise there will be errors. Press the **Import** button to import the package.
6. Once import is done a new menu will popup called **KamranWali**.
7. Finally to open SimpleInterface go to KamranWali -> SimpleInterface. This will open up the interface like the image below.
<p align="center"><img src="https://imgur.com/6mU2PD4.jpg"></p>

***
## Setup
Before using SimpleInterface you must load the prefabs. There are two ways to load prefabs, as follows:
1. **All Search** - Just press the **Load Prefabs** button. This will search your entire project, which is from the _Assets_ folder, for prefabs and will load them. This is not recommended as it will also search folders where there are no prefabs.
2. **Path Search _(Recommended)_** - In Unity under the Project tab go to a folder or folders that contains the prefab. Then _right click_ that folder and select **Copy Path**. Then _right click_ the **Path** text field and select _Paste_. Press _enter_. Now click the **Load Prefabs** button. This will now search for prefabs from the given path and will be faster than the previous method. This is the recommended way for loading the prefabs.

Once the prefabs have been loaded the drop down list called **Prefab Paths** will be loaded with all the prefab location and a selection grid will come up with it being populated with prefabs from a single path. Also the first prefab in the first selected path/folder will be shown in the preview window like the image below.
<p align="center"><img src="https://imgur.com/ZQgV0Tb.jpg"></p>

***
## Features
#### Place Prefab:
Once [Setup](#setup) has been done you need to first decide on which mask layer should the prefabs be placed. To do this you need to select the mask layer in **Collidable Layer**. Now select the prefab you want to place from the selection grid. Finally in the Scene window left click on the objects with the **Collidable Layer** and colliders for placing the prefab. The prefabs will be placed into the default root GameObject.

You can also place the prefabs into a certain GameObject. To do this drag and drop a GameObject from the _Hierarchy_ window into the **Root** field in the Simple Interface. Now any prefab placed will be inside the **Root** GameObject.

You can enable/disable **Place Prefab** by pressing the hotkey **U**. It is recommended to disable **Place Prefab** when not working with Simple Interface for avoiding any unnecessary prefab placement.

#### Limit Placement:
**Limit Placement** allows you to control how many prefabs can be placed. First follow [Place Prefab](#place-prefab). Then enable the **Limit Placement** by ticking it or by pressing the hotkey **B**. In the **Max** field give the maximum number of prefabs that can be placed. Below the **Max** field is a label which shows how many prefabs have been placed and how many are left. The **Placed** value should be 0 and the **Left** value should be the maximum value. If they are not then press the **Reset** button to reset the values.

Now start placing the prefabs. You will notice that the **Placed** and **Left** values are changing as the prefabs are being placed. Once the **Placed** value has become the maximum value and the **Left** value has become 0 you won't be able to place anymore prefabs. If you want to place more prefabs of the same amount then press the **Reset** button or press the hotkey **N** for resetting the **Limit Placement** and this will allow to place prefabs again.

You can enable/disable **Limit Placement** by pressing the hotkey **B**.

#### Drag:
**Drag** mode allows you to place multiple prefabs of the same type in one go by holding the left mouse button and then dragging the mouse. First follow [Place Prefab](#place-prefab). Then enable the **Drag** by ticking it or by pressing the hotkey **M**. In the Scene window hold the left mouse button and drag it around to place multiple prefabs in one go.

_Note: This will add lot of prefabs while dragging. To help control the prefab placement in drag mode please see [Limit Placement](#limit-placement) and [Offset Position](#offset-position)._

You can enable/disable **Drag** by pressing the hotkey **M**.

#### Fixed Position:
**Fixed Position** allows you to control the placement position of the prefab by giving a fixed value to any of the Vector3 axis. First follow [Place Prefab](#place-prefab). Then enable the **Fixed Position** by ticking it or by pressing the hotkey **I**. Now select the Vector3 axis that you want to give fixed value to which are **X**, **Y** and **Z**. After selecting the axis you can now give a fixed value to that axis. Now place a prefab in the Scene window. You will notice that the prefab is NOT placed where you clicked but is placed in a position with fixed values for the selected axis.

You can enable/disable **Fixed Position** by pressing the hotkey **I**.

#### Offset Position:
**Offset Position** allows you to control how far a prefab should be placed from the last placed position. First follow [Place Prefab](#place-prefab). Then enable the **Offset Position** by ticking it or by pressing the hotkey **V**. Now select the Vector3 axis that you want to give a distance threshold to which are **X**, **Y** and **Z**. After selecting the axis you can now give a threshold value to that axis. Now place a prefab in the Scene window. Then place another prefab near the last prefab. Depending on your selected axis and threshold value the second prefab will NOT be placed near the first prefab. You can only place another prefab that is further than the threshold value from the last prefab. This will allow you to not have prefabs jumbled up together when placing them.

You can enable/disable **Offset Position** by pressing the hotkey **V**.

#### Fixed Rotation:
**Fixed Rotation** allows you to control the placement rotation of the prefab by giving a fixed rotation value to any of the Quaternion angles. First follow [Place Prefab](#place-prefab). Then enable the **Fixed Rotation** by ticking it or by pressing the hotkey **O**. Now select the Quaternion angle that you want to give fixed value to which are **X**, **Y** and **Z**. After selecting the angle you can now give a fixed value to that angle. Now place a prefab in the Scene window. You will notice that the prefab has been placed with the given fixed rotation angles.

You can enable/disable **Fixed Rotation** by pressing the hotkey **O**.

#### Random Rotation:
**Random Rotation** allows you to give random placment rotation of the prefab by giving a random rotation range value to any of the Quaternion angles. First follow [Place Prefab](#place-prefab). Then enable the **Random Rotation** by ticking it or by pressing the hotkey **J**. Now select the Quaternion angle that you want to give a random value to which are **X**, **Y** and **Z**. After selecting the angle you can now give a range value for that angle in the **Min** and **Max** fields. Now place prefabs in the Scene window. You will notice that each prefab have different rotation angles. This is because each time a prefab is placed a random angle is given to that prefab from the range value.

You can enable/disable **Random Rotation** by pressing the hotkey **J**.

#### Fixed Scale:
**Fixed Scale** allows you to control the placement scale of the prefab by giving a fixed value. First follow [Place Prefab](#place-prefab). Then enable the **Fixed Scale** by ticking it or by pressing the hotkey **K**. Now give a scale value in the **Scale** field. Then place a prefab in the Scene window. You will notice that the scale of the prefab has changed to the one you have just given in the **Scale** field.

You can enable/disable **Fixed Scale** by pressing the hotkey **K**.

#### Random Scale:
**Random Scale** allows you to give random placement scale of the prefab by giving a random scale range value. First follow [Place Prefab](#place-prefab). Then enable the **Random Scale** by ticking it or by pressing the hotkey **L**. Now give a range scale in the **Min** and **Max** fields. Then place prefabs in the Scene window. You will notice that each prefab have different scale value. This is because each time a prefab is placed a random scale is given to that prefab from the range value.

You can enable/disable **Random Scale** by pressing the hotkey **L**

#### Hotkeys:
* U = Toggling prefab placement.
* I = Toggling fixed position.
* O = Toggling fixed Rotation.
* J = Toggling random Rotation.
* K = Toggling fixed Scale.
* L = Toggling random Scale.
* B = Toggling placement limit.
* N = Resetting the placement counter.
* M = Toggling drag.

***
## Developer
Simple Interface was made by keeping simple development process for developers in mind. There are blue prints that will help in adding new layout and features into Simple Interface.
#### [SimpleInterfaceWindow](https://github.com/deadlykam/SimpleInterface/blob/019fec7b04374283d29127b5fda540e2b6a97677/SimpleInterface/Assets/KamranWali/SimpleInterface/Scripts/Editor/SimpleInterfaceWindow.cs):
This is the main class that helps to create all the other layouts and use all the features. New **BaseLayouts** must be initialized in this class. The **BaseLayout \_placementLayout** must always be in the top and the **BaseLayout \_logoLayout** must always be at the bottom. Place any **BaseLayouts** in between these two **BaseLayouts** It also contains three import methods called **Vector3 GetActualPosition(Vector3)**, **Quaternion GetActualRotation(Quaternion)** and **Vector3 GetActualScale(Vector3)**. These three method shares the new calculated position, rotation and scale with the main **BaseLayout** which is the **BaseLayout \_placementLayout**. Put the appropriate functionality feature in the correct method from your custom **BaseLayout**.

#### [BaseLayout](https://github.com/deadlykam/SimpleInterface/blob/019fec7b04374283d29127b5fda540e2b6a97677/SimpleInterface/Assets/KamranWali/SimpleInterface/Scripts/Editor/Layouts/BaseLayout.cs):
This is an abstract class and is the blue print for creating a new layout and functionality. Extending this class will import all the necessary methods that MUST be filled up. Check out the other classes that extends from **BaseLayout** to understand how it works. Here is a brief explanation of the methods:
1. **void SetupOnGUI():** This is the method where all the _EditorGUILayout_ and _GUILayout_ elements must be called. Basically this method handles the look of your layout.
2. **void Update(UnityEngine.Event):** This is the method where user input must be handled and any continuous logic. Basically this method acts like _Monobehaviour.Update()_ method.
3. **bool IsShown():** This method must contain the logic for saying if the layout is being shown or active.
4. **void Hide():** This method must contain the logic for hiding the layout.
5. **void SetupOnEnable():** This method contains all the logic for initializaing any local scope variable.

After filling up these methods you must initialize the custom **BaseLayout** in the **SimpleInterfaceWindow**. First add the **BaseLayout** with the other **BaseLayout** fields but in between **BaseLayout \_placementLayout** and **BaseLayout \_logoLayout**. Then in the **void SimpleInterfaceWindow.Setup()** method initialize the custom **BaseLayout**. Under the **===Linking Opposite Layouts===** comment add any layout that needs to be hidden when the custom **BaseLayout** is shown, this is optional. Now under the **===Ading Layouts===** comment add the **BaseLayout** to the **\_manager**. Added right before **\_logoLayout**. That is it and now when you open up Simple Interface in Unity3D your new custom layout will pop up at the bottom right before the logo.

***
## Bug Fixes
- **Similar Folder Name(Issue #44):** Fixed the bug where similar folder name could not populate the drop down box.
***
## Versioning
The project uses [Semantic Versioning](https://semver.org/). Available versions can be seen in [tags on this repository](https://github.com/deadlykam/SimpleInterface/tags).
***
## Authors
- Syed Shaiyan Kamran Waliullah 
  - [Kamran Wali Github](https://github.com/deadlykam)
  - [Kamran Wali Twitter](https://twitter.com/KamranWaliDev)
  - [Kamran Wali Youtube](https://www.youtube.com/channel/UCkm-BgvswLViigPWrMo8pjg)
  - [Kamran Wali Website](https://deadlykam.github.io/)
***
## License
This project is licensed under the MIT License - see the [LICENSE.md](LICENSE) file for details.
***
