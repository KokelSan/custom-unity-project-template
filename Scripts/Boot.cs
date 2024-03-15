using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for instantiating the needed managers and loading the game's starting scene.
/// This class should only be used in the scene 0 which should contain only one object, the one holding this script.
/// </summary>
public class Boot : MonoBehaviour
{
    public List<Manager> ManagersToInstantiate;
    public int SceneToLoadOnBootCompleted = 1;
    public TransitionType ScreenTransitionForFirstScene;
    
    void Start()
    {
        foreach (Manager manager in ManagersToInstantiate)
        {
            Manager instance = Instantiate(manager);
            instance.name = manager.name;
            instance.Initialize();
            DontDestroyOnLoad(instance);
        }

        ScreenTransitionManagerHandlerData.ShowTransition(ScreenTransitionForFirstScene, true);
        SceneLoadingManagerHandlerData.OnSceneLoaded += OnSceneLoaded;
        SceneLoadingManagerHandlerData.LoadScene(SceneToLoadOnBootCompleted);
    }

    private void OnSceneLoaded(int index)
    {
        if(index != SceneToLoadOnBootCompleted) return;
        SceneLoadingManagerHandlerData.OnSceneLoaded -= OnSceneLoaded;
        ScreenTransitionManagerHandlerData.HideTransition(ScreenTransitionForFirstScene);
    }
}