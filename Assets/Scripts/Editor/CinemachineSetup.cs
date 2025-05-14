using UnityEngine;
using UnityEditor;
using Cinemachine;

public class CinemachineSetup : EditorWindow
{
    [MenuItem("Window/Cinemachine Setup")]
    public static void ShowWindow()
    {
        GetWindow<CinemachineSetup>("Cinemachine Setup");
    }

    void OnGUI()
    {
        GUILayout.Label("Cinemachine Virtual Camera Setup", EditorStyles.boldLabel);

        if (GUILayout.Button("Create Virtual Camera"))
        {
            CreateVirtualCamera();
        }
    }

    void CreateVirtualCamera()
    {
        try
        {
            // Create the virtual camera object
            GameObject vcam = new GameObject("CM vcam1");
            Undo.RegisterCreatedObjectUndo(vcam, "Create Virtual Camera");

            // Find the main camera
            GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            if (mainCamera != null)
            {
                // Position the virtual camera at the same position as the main camera
                vcam.transform.position = mainCamera.transform.position;
                vcam.transform.rotation = mainCamera.transform.rotation;
                
                // Add CinemachineBrain component to the main camera if it doesn't exist
                if (!mainCamera.GetComponent<CinemachineBrain>())
                {
                    Undo.AddComponent<CinemachineBrain>(mainCamera);
                    Debug.Log("Added CinemachineBrain to main camera");
                }
            }

            // Add virtual camera component
            var virtualCamera = Undo.AddComponent<CinemachineVirtualCamera>(vcam);
            
            // Set properties
            virtualCamera.Priority = 10;
            
            // Select the created object
            Selection.activeGameObject = vcam;
            
            Debug.Log("Virtual camera created successfully");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error creating virtual camera: " + e.Message);
        }
    }
}