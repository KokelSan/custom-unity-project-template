using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for instantiating the needed managers and loading the game's starting scene.
/// This class should only be used in the scene 0 which should contain only one object, the one holding this script.
/// </summary>
public class Boot : MonoBehaviour
{
    public List<Service> ServicesToInstantiate;
    public int SceneIndexToLoadOnBootCompleted = 1;
    public int TargetedFrameRate = 60;

    private void Awake()
    {
        Application.targetFrameRate = TargetedFrameRate;
        // Time.timeScale = 0.01f;
        
        foreach (Service manager in ServicesToInstantiate)
        {
            Service instance = Instantiate(manager);
            instance.name = manager.name;
            DontDestroyOnLoad(instance);
        }

        SceneLoadingServiceHandlerData.LoadScene(new SceneLoadingParameters(SceneIndexToLoadOnBootCompleted, screenTransitionType: ScreenTransitionType.Boot));
    }
}