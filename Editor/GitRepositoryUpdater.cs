using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace custom_unity_project_template.Editor
{
    public class GitRepositoryUpdater : MonoBehaviour
    {
        [MenuItem("Custom Project Template/Git Commands/Pull Repository")]
        public static void PullRepository()
        {
            ExecuteGitCommand("pull");
        }

        private static void ExecuteGitCommand(string gitCommand)
        {
            string repositoryPath = Application.dataPath + "/custom-unity-project-template";

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
                process.OutputDataReceived += (_, args) => { globalOutput += "\n" + args.Data; }; 
                process.ErrorDataReceived += (_, args) => { globalOutput += "\n" + args.Data; }; 
                process.Start();
                
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    Debug.Log($"Git {gitCommand} succeeded \n{globalOutput}");
                    AssetDatabase.Refresh();
                }
                else
                {
                    Debug.LogError($"Git command failed \n{globalOutput}");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Git process failed \n\n{e}");
            }
        }
    }
}