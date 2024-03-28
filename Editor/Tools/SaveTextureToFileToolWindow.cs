#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
 
/// <summary>
/// From https://forum.unity.com/threads/save-rendertexture-or-texture2d-as-image-file-utility.1325130/
/// </summary>
public class SaveTextureToFileToolWindow : EditorWindow
{
    private ObjectField texture;
    private TextField filePath;
    private Vector2IntField size;
    private EnumField format;
    private Button button;
 
    private string uniqueFilePath;
 
 
    [MenuItem("Project Foundation/Tools/Save Texture To File")]
    public static void ShowWindow()
    {
        SaveTextureToFileToolWindow wnd = GetWindow<SaveTextureToFileToolWindow>();
        wnd.minSize = new Vector2(300, 105);
        wnd.titleContent = new GUIContent("Save Texture To File");
    }
 
    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        texture = new ObjectField("Texture") { objectType = typeof(Texture) };
        root.Add(texture);
        filePath = new TextField("File Path") { value = "Assets/texture.png" };
        root.Add(filePath);
        size = new Vector2IntField("Size") { value = new Vector2Int(-1, -1), tooltip = "Negative values mean original width and height." };
        root.Add(size);
        format = new EnumField("Format", SaveTextureToFileTool.SaveTextureFileFormat.PNG);
        root.Add(format);
        button = new Button(Save) { text = "Save" };
        root.Add(button);
    }
 
    private void Save()
    {
        uniqueFilePath = AssetDatabase.GenerateUniqueAssetPath(filePath.value);
        SaveTextureToFileTool.SaveTextureToFile(
            (Texture)texture.value,
            uniqueFilePath,
            size.value.x,
            size.value.y,
            (SaveTextureToFileTool.SaveTextureFileFormat)format.value,
            done: DebugResult);
    }
 
    private void DebugResult(bool success)
    {
        if (success)
        {
            AssetDatabase.Refresh();
            Object file = AssetDatabase.LoadAssetAtPath(uniqueFilePath, typeof(Texture2D));
            Debug.Log($"Texture saved to [{uniqueFilePath}]", file);
        }
        else
        {
            Debug.LogError($"Failed to save texture.");
        }
    }
}

#endif