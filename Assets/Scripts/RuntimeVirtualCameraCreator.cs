using UnityEngine;
using System.Reflection;

/// <summary>
/// This script creates a virtual camera at runtime.
/// Attach it to any GameObject in your PersistentScene.
/// </summary>
public class RuntimeVirtualCameraCreator : MonoBehaviour
{
    // Whether to create the virtual camera on Start
    public bool createOnStart = true;
    
    // Optional: Specify a target for the virtual camera to follow
    public Transform followTarget;
    
    // Optional: Specify a target for the virtual camera to look at
    public Transform lookAtTarget;
    
    void Start()
    {
        if (createOnStart)
        {
            CreateVirtualCamera();
        }
    }
    
    public void CreateVirtualCamera()
    {
        try
        {
            // Create the virtual camera object
            GameObject vcamObject = new GameObject("CM vcam1");
            
            // Position it at the main camera position
            GameObject mainCamera = GameObject.FindWithTag("MainCamera");
            if (mainCamera != null)
            {
                vcamObject.transform.position = mainCamera.transform.position;
            }
            
            // Use reflection to add Cinemachine components since we can't directly reference them
            // Find the Cinemachine assembly
            Assembly cinemachineAssembly = null;
            foreach (Assembly assembly in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GetName().Name.Contains("Cinemachine"))
                {
                    cinemachineAssembly = assembly;
                    break;
                }
            }
            
            if (cinemachineAssembly != null)
            {
                // Find and add CinemachineVirtualCamera component
                System.Type vcamType = null;
                foreach (System.Type type in cinemachineAssembly.GetTypes())
                {
                    if (type.Name == "CinemachineVirtualCamera")
                    {
                        vcamType = type;
                        break;
                    }
                }
                
                if (vcamType != null)
                {
                    // Add the component
                    Component vcam = vcamObject.AddComponent(vcamType);
                    
                    // Set Follow and LookAt targets if provided
                    if (followTarget != null)
                    {
                        vcamType.GetProperty("Follow")?.SetValue(vcam, followTarget);
                    }
                    
                    if (lookAtTarget != null)
                    {
                        vcamType.GetProperty("LookAt")?.SetValue(vcam, lookAtTarget);
                    }
                    
                    // Set priority
                    vcamType.GetProperty("Priority")?.SetValue(vcam, 10);
                    
                    Debug.Log("Virtual camera component added");
                }
                else
                {
                    Debug.LogError("CinemachineVirtualCamera type not found");
                }
                
                // Add CinemachineBrain to main camera if needed
                if (mainCamera != null)
                {
                    System.Type brainType = null;
                    foreach (System.Type type in cinemachineAssembly.GetTypes())
                    {
                        if (type.Name == "CinemachineBrain")
                        {
                            brainType = type;
                            break;
                        }
                    }
                    
                    if (brainType != null && mainCamera.GetComponent(brainType) == null)
                    {
                        mainCamera.AddComponent(brainType);
                        Debug.Log("CinemachineBrain added to main camera");
                    }
                }
            }
            else
            {
                Debug.LogError("Cinemachine assembly not found. Make sure Cinemachine is imported in your project.");
                Destroy(vcamObject);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error creating virtual camera: " + e.Message);
        }
    }
}