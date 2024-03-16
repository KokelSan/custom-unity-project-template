using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace custom_unity_project_template.Editor
{
    public static class DependenciesInstaller
    {
        static AddAndRemoveRequest _request;
        private static List<string> _startingPackages = new List<string>();
        
        static string[] _dependencies = 
        {
            "com.unity.inputsystem",
        };

        [MenuItem("Custom Project Template/Install Dependencies")]
        static void InstallDependencies()
        {
            _startingPackages = PackageInfo.GetAllRegisteredPackages().Select(info => info.packageId).ToList();
            _request = Client.AddAndRemove(_dependencies);
            EditorApplication.update += OnEditorUpdate;
        }

        static void OnEditorUpdate()
        {
            if (_request.IsCompleted)
            {
                EditorApplication.update -= OnEditorUpdate;
                
                if (_request.Status == StatusCode.Success)
                {
                    List<string> finalPackages = _request.Result.Select(info => info.packageId).ToList();
                    var newPackages = finalPackages.Except(_startingPackages).ToList();
                    int newCount = newPackages.Count;
                    if (newCount == 0)
                    {
                        Debug.Log($"All the required packages are already installed \n");
                        return;
                    }

                    string debug = $"Dependencies installation completed, {newCount} {(newCount > 1 ? "packages" : "package")} installed";
                    foreach (var package in newPackages)
                    {
                        debug += "\n  " + package;
                    }
                    Debug.Log($"{debug} \n");
                }
                else if (_request.Status == StatusCode.Failure)
                {
                    Debug.LogError($"Dependencies installation failed \n\n{_request.Error.message}\n");
                }
            }
        }
    }
}