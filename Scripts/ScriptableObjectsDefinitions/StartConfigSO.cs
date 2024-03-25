using CustomAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "Project Template/New Start Config",  fileName = "_StartConfigSO")]
public class StartConfigSO : ScriptableObject
{
        public int MainMenuSceneIndex = 1;
        
        public bool LoadSceneOnStartGame = true;
        [ShowIf(nameof(LoadSceneOnStartGame))]
        public int SceneIndexToLoadOnStart = 2;
}