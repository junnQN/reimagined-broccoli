using UnityEngine;
using UnityEditor;
using Cinemachine;

public class AddVirtualCamera : Editor
{
    [MenuItem("Willowdale/Add Virtual Camera")]
    public static void AddCinemachineVirtualCamera()
    {
        // Create the virtual camera object
        GameObject vcamGameObject = new GameObject("CM vcam1");
        
        // Add the virtual camera component
        CinemachineVirtualCamera vcam = vcamGameObject.AddComponent<CinemachineVirtualCamera>();
        
        // Get the main camera and set it as the target
        GameObject mainCamera = GameObject.FindWithTag("MainCamera");
        if (mainCamera != null)
        {
            // Set the follow target if needed
            // vcam.Follow = mainCamera.transform;
            
            // Position the virtual camera at the same position as the main camera
            vcamGameObject.transform.position = mainCamera.transform.position;
            vcamGameObject.transform.rotation = mainCamera.transform.rotation;
            
            // Set the brain on the main camera if it doesn't exist
            if (mainCamera.GetComponent<CinemachineBrain>() == null)
            {
                CinemachineBrain brain = mainCamera.AddComponent<CinemachineBrain>();
                brain.m_ShowDebugText = true;
                brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;
                brain.m_DefaultBlend.m_Time = 1.5f;
            }
        }

        // Set some default values
        vcam.Priority = 10;
        
        // Select the newly created virtual camera
        Selection.activeGameObject = vcamGameObject;
        
        Debug.Log("Cinemachine Virtual Camera added");
    }
}