// How to Create a Virtual Camera in Unity

/*
Follow these steps to create a Cinemachine Virtual Camera:

1. In the Unity Editor menu, go to GameObject > Cinemachine > Virtual Camera
   (This will create a new virtual camera in your scene)

2. If you don't see the Cinemachine menu, make sure the package is installed:
   - Open Window > Package Manager
   - Select "Unity Registry" from the dropdown
   - Find "Cinemachine" and click "Install"
   - After installation, try step 1 again

3. Once created, the virtual camera will appear in your scene hierarchy as "CM vcam1"

4. The virtual camera will follow the main camera's position by default

5. To configure the virtual camera:
   - Select it in the hierarchy
   - In the Inspector, you can set:
     * Follow: The target the camera should follow (position)
     * Look At: The target the camera should always look at
     * Priority: Determines which camera takes precedence when multiple are active
     * Body: Controls how the camera follows its target
     * Aim: Controls how the camera rotates to look at its target

6. To make the Main Camera use Cinemachine:
   - Select your Main Camera
   - Add Component > Cinemachine > Cinemachine Brain
   - This component allows your Main Camera to be controlled by Cinemachine

7. More info: https://unity.com/unity/features/editor/art-and-design/cinemachine
*/

// This is just a README file to guide you through the process manually
// You can delete this script after creating your virtual camera