using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

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
                
                string globalOutput = "";
                process.OutputDataReceived += (_, args) => { globalOutput += args.Data + "\n"; }; 
                process.ErrorDataReceived += (_, args) => { globalOutput += args.Data + "\n"; }; 
                process.Start();
                
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    UnityEngine.Debug.Log($"Pull command succeeded \n{globalOutput}");
                    AssetDatabase.Refresh();
                }
                else
                {
                    UnityEngine.Debug.LogError($"Git command failed \n{globalOutput}");
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"Git process failed \n{e}");
            }
        }
    }
}