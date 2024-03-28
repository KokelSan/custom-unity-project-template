using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for instantiating the needed managers and loading the game's starting scene.
/// This class should only be used in the scene 0.
/// </summary>
public class Boot : MonoBehaviour
{
    public List<GameObject> ServicesToInstantiate;
    public int SceneIndexToLoadOnBootCompleted = 1;
    public int TargetedFrameRate = 60;

    private void Awake()
    {
        Application.targetFrameRate = TargetedFrameRate;
        
        foreach (GameObject go in ServicesToInstantiate)
        {
            GameObject instance = Instantiate(go);
            instance.name = go.name;
            DontDestroyOnLoad(instance);
        }

        SceneLoadingService.LoadScene(new SceneLoadingData(SceneIndexToLoadOnBootCompleted));
    }
}