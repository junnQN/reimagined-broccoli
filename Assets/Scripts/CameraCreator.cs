using UnityEngine;

public class CameraCreator : MonoBehaviour
{
    // A flag to ensure this script only runs once
    private bool created = false;

    void Awake()
    {
        if (!created)
        {
            CreateCinemachineSetup();
            created = true;
        }
    }

    void CreateCinemachineSetup()
    {
        // Create the CM vcam1 object
        GameObject vcamObject = new GameObject("CM vcam1");
        
        // Get the type of the CinemachineVirtualCamera component
        System.Type vcamType = System.Type.GetType("Cinemachine.CinemachineVirtualCamera, Cinemachine");
        if (vcamType != null)
        {
            // Add the virtual camera component
            Component virtualCamera = vcamObject.AddComponent(vcamType);
            
            // Get the main camera
            GameObject mainCamera = GameObject.FindWithTag("MainCamera");
            if (mainCamera != null)
            {
                // Position the virtual camera at the same position as the main camera
                vcamObject.transform.position = mainCamera.transform.position;
                vcamObject.transform.rotation = mainCamera.transform.rotation;
                
                // Add CinemachineBrain to the main camera if needed
                System.Type brainType = System.Type.GetType("Cinemachine.CinemachineBrain, Cinemachine");
                if (brainType != null && mainCamera.GetComponent(brainType) == null)
                {
                    mainCamera.AddComponent(brainType);
                }
            }
            
            Debug.Log("Cinemachine Virtual Camera created successfully!");
        }
        else
        {
            Debug.LogError("Could not find Cinemachine.CinemachineVirtualCamera type. Make sure Cinemachine is imported correctly.");
        }
    }
}