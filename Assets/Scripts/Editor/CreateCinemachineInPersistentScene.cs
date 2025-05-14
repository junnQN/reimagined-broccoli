using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEditor.SceneManagement;

public static class CreateCinemachineInPersistentScene
{
    [MenuItem("Tools/Willowdale/Create Virtual Camera in PersistentScene")]
    public static void CreateVirtualCamera()
    {
        // Check if PersistentScene is loaded
        string persistentScenePath = "Assets/Scenes/PersistentScene.unity";
        UnityEngine.SceneManagement.Scene persistentScene;
        
        // Try to find if the scene is already loaded
        bool sceneIsLoaded = false;
        for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
        {
            UnityEngine.SceneManagement.Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
            if (scene.path == persistentScenePath)
            {
                persistentScene = scene;
                sceneIsLoaded = true;
                break;
            }
        }
        
        // Load the scene if it's not already loaded
        if (!sceneIsLoaded)
        {
            persistentScene = EditorSceneManager.OpenScene(persistentScenePath, OpenSceneMode.Single);
        }
        
        // Find main camera in the scene
        GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found in PersistentScene. Please create a camera first.");
            return;
        }
        
        // Check if a virtual camera with the same name already exists
        GameObject existingVCam = GameObject.Find("CM vcam1");
        if (existingVCam != null)
        {
            Debug.LogWarning("A virtual camera named 'CM vcam1' already exists. Creating a new one with a different name.");
            existingVCam = null;
        }
        
        // Create vcam object
        GameObject vcam = new GameObject(existingVCam == null ? "CM vcam1" : "CM vcam2");
        Undo.RegisterCreatedObjectUndo(vcam, "Create virtual camera");
        
        // Set position same as main camera
        vcam.transform.position = mainCamera.transform.position;
        
        // Try to add Cinemachine components
        System.Reflection.Assembly cinemachineAssembly = null;
        foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
        {
            if (assembly.GetName().Name.Contains("Cinemachine"))
            {
                cinemachineAssembly = assembly;
                break;
            }
        }
        
        if (cinemachineAssembly != null)
        {
            // Find CinemachineVirtualCamera type
            System.Type vcamType = null;
            foreach (var type in cinemachineAssembly.GetTypes())
            {
                if (type.Name == "CinemachineVirtualCamera")
                {
                    vcamType = type;
                    break;
                }
            }
            
            if (vcamType != null)
            {
                Undo.AddComponent(vcam, vcamType);
                Debug.Log("Added CinemachineVirtualCamera component");
            }
            
            // Find CinemachineBrain type
            System.Type brainType = null;
            foreach (var type in cinemachineAssembly.GetTypes())
            {
                if (type.Name == "CinemachineBrain")
                {
                    brainType = type;
                    break;
                }
            }
            
            if (brainType != null && mainCamera.GetComponent(brainType) == null)
            {
                Undo.AddComponent(mainCamera, brainType);
                Debug.Log("Added CinemachineBrain component to main camera");
            }
            
            Selection.activeGameObject = vcam;
            Debug.Log("Virtual camera created successfully in PersistentScene");
            
            // Save the scene
            EditorSceneManager.SaveScene(persistentScene);
        }
        else
        {
            Debug.LogError("Cinemachine assembly not found. Please make sure it's installed via Package Manager.");
            GameObject.DestroyImmediate(vcam);
        }
    }
}