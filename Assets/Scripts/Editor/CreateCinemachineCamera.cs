using UnityEngine;
using UnityEditor;
using Cinemachine;

public static class CreateCinemachineCamera
{
    [MenuItem("Tools/Create Virtual Camera")]
    public static void CreateVirtualCamera()
    {
        // Create the virtual camera game object
        GameObject virtualCameraGO = new GameObject("CM vcam1");
        Undo.RegisterCreatedObjectUndo(virtualCameraGO, "Create virtual camera");
        
        // Add virtual camera component
        CinemachineVirtualCamera virtualCamera = Undo.AddComponent<CinemachineVirtualCamera>(virtualCameraGO);
        
        // Find the main camera
        GameObject mainCamera = GameObject.FindWithTag("MainCamera");
        if (mainCamera != null)
        {
            // Place the virtual camera at the same position as the main camera
            virtualCameraGO.transform.position = mainCamera.transform.position;
            
            // Add CinemachineBrain to the main camera if it doesn't already have one
            if (mainCamera.GetComponent<CinemachineBrain>() == null)
            {
                Undo.AddComponent<CinemachineBrain>(mainCamera);
            }
        }
        
        // Select the newly created camera
        Selection.activeGameObject = virtualCameraGO;
    }
}