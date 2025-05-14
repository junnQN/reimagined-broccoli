using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class VirtualCameraInitializer : MonoBehaviour
{
    void Start()
    {
        // Find the main camera
        GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        
        // Get the CM vcam1 game object (already created in the scene)
        GameObject vcam = GameObject.Find("CM vcam1");
        
        if (vcam != null && mainCamera != null)
        {
            // Position the virtual camera at the same position as the main camera
            vcam.transform.position = mainCamera.transform.position;
            
            // Dynamic component addition for Cinemachine components
            var cinemachineAssembly = System.AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name.Contains("Cinemachine"));
                
            if (cinemachineAssembly != null)
            {
                // Add virtual camera component
                var vcamType = cinemachineAssembly.GetTypes()
                    .FirstOrDefault(t => t.Name == "CinemachineVirtualCamera");
                if (vcamType != null)
                {
                    var vcamComponent = vcam.AddComponent(vcamType);
                    Debug.Log("Added virtual camera component");
                }
                
                // Add brain component to main camera
                var brainType = cinemachineAssembly.GetTypes()
                    .FirstOrDefault(t => t.Name == "CinemachineBrain");
                if (brainType != null && !mainCamera.GetComponent(brainType))
                {
                    mainCamera.AddComponent(brainType);
                    Debug.Log("Added brain component to main camera");
                }
            }
        }
    }
    
#if UNITY_EDITOR
    // Add a menu item to create the virtual camera
    [MenuItem("Tools/Willowdale/Create Virtual Camera")]
    public static void CreateVirtualCamera()
    {
        // Find main camera
        GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found. Please create a camera first.");
            return;
        }
        
        // Create vcam object
        GameObject vcam = new GameObject("CM vcam1");
        Undo.RegisterCreatedObjectUndo(vcam, "Create virtual camera");
        
        // Set position same as main camera
        vcam.transform.position = mainCamera.transform.position;
        
        // Try to add components if Cinemachine assembly is available
        var cinemachineAssembly = System.AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(a => a.GetName().Name.Contains("Cinemachine"));
            
        if (cinemachineAssembly != null)
        {
            // Add virtual camera component
            var vcamType = cinemachineAssembly.GetTypes()
                .FirstOrDefault(t => t.Name == "CinemachineVirtualCamera");
            if (vcamType != null)
            {
                Undo.AddComponent(vcam, vcamType);
            }
            
            // Add brain component to main camera
            var brainType = cinemachineAssembly.GetTypes()
                .FirstOrDefault(t => t.Name == "CinemachineBrain");
            if (brainType != null && !mainCamera.GetComponent(brainType))
            {
                Undo.AddComponent(mainCamera, brainType);
            }
            
            Selection.activeGameObject = vcam;
            Debug.Log("Virtual camera created successfully");
        }
        else
        {
            Debug.LogError("Cinemachine not found. Please make sure it's installed.");
        }
    }
#endif
}