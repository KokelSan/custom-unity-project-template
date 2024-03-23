using UnityEngine;

[CreateAssetMenu(menuName = "Project Template/New Start Config",  fileName = "_StartConfigSO")]
public class StartConfigSO : ScriptableObject
{
        public int MainMenuSceneIndex = 1;
        
        public bool LoadSceneOnStartGame = true;
        public int SceneIndexToLoadOnStart = 2;
}