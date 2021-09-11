<p align="center"><img src="https://imgur.com/au46wFG.png"></p>

# Simple Interface

### Introduction:
This is a simple Unity3D interface that helps to put prefabs easily into the editor.
<p align="center"><img src="https://imgur.com/ta999OY.jpg"></p>

## Prerequisites
#### Unity Game Engine
Unity3D version of **2020.3.3f1** and above should work. The master version is **2020.3.3f1**.
***
## Stable Build
Stable-v1.0.0 is the latest stable build of the project. The unitypackage for this project can also be found there. If development is going to be done on this project then it is advised to branch off of any Stable branches because they will NOT be updated. Any other branches are subjected to change including the main branch.
***
## Installation:
1. First download the latest SimpleInterface-vx.x.x.unitypackage from the latest Stable build. 
2. Once download is completed open up the Unity project you want to use this project in.
3. Now go to Assets -> Import Package -> Custom Package.
4. Select the SimpleInterface-vx.x.x you just downloaded and open it.
5. Make sure everything is selected in the Import Unity Package otherwise there will be errors. Press the **Import** button to import the package.
6. Once import is done a new menu will popup called **KamranWali**.
7. Finally to open SimpleInterface go to KamranWali -> SimpleInterface. This will open up the interface like the image below.
<p align="center"><img src="https://imgur.com/6mU2PD4.jpg"></p>

***
### Setup:
Before using SimpleInterface you must load the prefabs. There are two ways to load prefabs, as follows:
1. All Search - Just press the **Load Prefabs** button. This will search your entire project, which is from the _Assets_ folder, for prefabs and will load them. This is not recommended as it will also search folders where there are no prefabs.
2. Path Search _(Recommended)_ - In Unity under the Project tab go to a folder or folders that contains the prefab. Then _right click_ that folder and select **Copy Path**. Then _right click_ the **Path** text field and select _Paste_. Press _enter_. Now click the **Load Prefabs** button. This will now search for prefabs from the given path and will be faster than the previous method. This is the recommended way for loading the prefabs.
Once the prefabs have been loaded the drop down list called **Prefab Paths** will be loaded with all the prefab location and a selection grid will come up with it being populated with prefabs from a single path like the image below.
***
