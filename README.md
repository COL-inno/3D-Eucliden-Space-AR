# 3D-Euclidean-Space-AR
AR Project developed for Commonwealth of Learning using Unity ARFoundation

## App Download
 * Apple App Store : https://apps.apple.com/ca/app/3d-euclidean-space-ar/id1474420796
 * Google Play :  https://play.google.com/store/apps/details?id=org.col.threeDEuclideanSpaceAR

## Development Environment
1. Hardware
* Alienware Aurora R8        
    CPU  : Intel Core i7-9700K@3.6 Ghz  
    RAM  : 16GB  
    OS   : Windows 10, 1809 Build       
    GPU  : Nvidia RTX2080         
* iMac 27 inch Retina 5K (Late 2019)     
    CPU  : Intel Core i5@3 Ghz     
    RAM  :  32GB     
    OS   : macOS Mojave 10.14.4     
    GPU  : AMD Radeon Pro 570X     
* Nokia 8 (TA-1004)     
    AP       : Snapdragon 835     
    RAM      : 4GB     
    Screen   : 1440 X 2560 pixels (554 ppi)     
    Camera   : 13MP, f/2.0     
    OS       :  Android 9 (Pie)     
* iPad Mini 5 (2019)     
    AP       : A12 Bionic     
    RAM      : 3GB     
    Screen   : 1536 X 2047 pixels (326 ppi)     
    Camera   : 8MP, f/2.4     
    OS       : iOS 12.3.1        
2. Software    
* Unity3D Game Engine 2019.1.6f Build – https://unity.com     
    Imported Packages: AR Foundation 2.1.0, ARCore XR Plugin 2.1.0, ARKit XR Plugin 2.1.0, TextMesh Pro
* Oculus Lipsync Unity SDK 1.39.0 - https://developer.oculus.com/downloads/package/oculus-lipsync-unity 
* UniVRM 0.53.0 - https://github.com/vrm-c/UniVRM/releases/download/v0.53.0/UniVRM-0.53.0_6b07.unitypackage
* VSCode 1.36.1 - https://code.visualstudio.com/       
    Extensions : C# 1.21.0 by Microsoft     
                 Debugger for Unity 2.7.2 by Unity     
                 Unity Code Snippets 1.3.0 by Kleber Silva       
* Blender 2.80 - http://www.blender.org
* VRM Importer for Blender 2.80 - https://github.com/iCyP/VRM_IMPORTER_for_Blender2_8/archive/master.zip
* Cats Blender Plugin 0.14.0 - https://github.com/michaeldegroot/cats-blender-plugin/releases   
* Vroid Studio 0.6.3 – https://vroid.com/studio
* Gimp 2.10.8 - https://www.gimp.org
* Adobe Mixamo – https://www.mixamo.com 
* Amazon Polly – https://aws.amazon.com/polly
* Codebeautify.org - https://codebeautify.org/yaml-to-json-xml-csv     
## Development Process     
Graphic tools used in this project perform better on the Window PC. For that advantage, the entire project was built on the Windows PC first, and then transferred to iMac as a Unity package file.      
Files and data transfer between software and libraries is summarized and visualized in the following diagram.   
![alt](https://github.com/COL-inno/3D-Eucliden-Space-AR/blob/master/Images/software_and_libs_slide.jpg)
## Build Prcoess in Unity     
1. Download the Unity Package for [Andriod](https://github.com/COL-inno/3D-Eucliden-Space-AR/raw/master/3DEuclideanSpace_Android_082219.unitypackage) or [iOS](https://github.com/COL-inno/3D-Eucliden-Space-AR/raw/master/3DEuclideanSpace_iOS_082219.unitypackage)     
2. Import the package into Unity Editor (Asset->Import Package->Custom Package)     
3. Type "screen" in User Layer 8 (Edit->Project Settings->Tags and Layers->Layer)     
4. Under Player Setting, fill the Company Name, Product Name and Version      
5. Select the icon from found in the Texture folder(COL AR)     
6. Under Other Setting,      
For Android,      
* Remove Vulkan from Graphics APIs     
* Fill the package Name, version, bundle version code accordingly       
* Switch Scripting Backend to IL2CPP and check ARMv7 and ARM64     
      
For iOS,     
* Fill the package Name, version, bundle version code accordingly     
* Type "11.0" for Taget minimum iOS version     
* Check "Requires ARKit support"     
* Switch Architecture to ARM64     
7. Select the keystore and type in the password accordingly under Publishing Setting(for Android only)      
8. Under Build Setting, click on "Add Open Scenes" and make sure 3DEuclideanSpace scene is checked     
9. Select the desired platform and click on "Switch Platform"     
10. To build Android App Bundle(*.aab) check the Build App Bundle(otherwise, builds APK)     
11. Click on "Build"     
