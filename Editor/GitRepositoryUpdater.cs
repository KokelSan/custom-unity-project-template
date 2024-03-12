using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace custom_unity_project_template.Editor
{
    public class GitRepositoryUpdater : MonoBehaviour
    {
        [MenuItem("Custom Project Template/Update Git Repository")]
        public static void UpdateGitRepository()
        {
            string repositoryPath = Application.dataPath + "/custom-unity-project-template";
            string gitCommand = "pull";

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = gitCommand,
                WorkingDirectory = repositoryPath,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            try
            {
                Process process = new Process { StartInfo = startInfo };
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                if (!string.IsNullOrEmpty(error))
                {
                    UnityEngine.Debug.LogError($"Git command failed \n{error}");
                }
                else
                {
                    UnityEngine.Debug.Log($"Pull command succeeded \n{output}");
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"Git process failed \n{e}");
            }
        }
    }
}