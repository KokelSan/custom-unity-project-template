using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class GitRepositoryUpdater : MonoBehaviour
{
    [MenuItem("Custom Project Template/Update Git Repository")]
    public static void UpdateGitRepository()
    {
        
#if !UNITY_EDITOR
        return;
#endif
        
        string repositoryPath = Application.dataPath + "/custom-unity-project-template";

        if (!System.IO.Directory.Exists(repositoryPath))
        {
            UnityEngine.Debug.LogError("Repository's update failed: repositoryPath is not valid.");
            return;
        }

        string command = "git pull";
        string script = $"cd \"{repositoryPath}\"; {command}";

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "powershell.exe",
            Arguments = $"-NoProfile -ExecutionPolicy unrestricted -Command \"{script}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        try
        {
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            if (process.ExitCode == 0)
            {
                string output = process.StandardOutput.ReadToEnd();
                UnityEngine.Debug.Log($"Repository successfully updated. Git output: {output}");
            }
            else
            {
                string error = process.StandardError.ReadToEnd();
                UnityEngine.Debug.LogError($"Repository's update failed. Git output: {error}");
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError($"Repository's update failed. System output: {e}");
        }
    }
}