using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

namespace custom_unity_project_template.Editor
{
    public class DependenciesInstaller
    {
        static AddRequest _request;
        
        private static int _currentDependencyIndex;
        private static List<string> _successes = new List<string>();
        private static List<string> _fails = new List<string>();
        
        static List<string> _dependencies = new List<string>() 
        {
            "com.unity.inputsystem",
        };

        [MenuItem("Custom Project Template/Install Dependencies")]
        static void InstallDependencies()
        {
            _currentDependencyIndex = 0;
            _successes.Clear();
            _fails.Clear();
            InstallDependency();
        }

        static void InstallDependency()
        {
            _request = Client.Add(_dependencies[_currentDependencyIndex]);
            EditorApplication.update += Progress;
        }

        static void Progress()
        {
            if (_request.IsCompleted)
            {
                if (_request.Status == StatusCode.Success)
                {
                    _successes.Add(_request.Result.packageId);
                }
                else if (_request.Status == StatusCode.Failure)
                {
                    _fails.Add($"{_dependencies[_currentDependencyIndex]}");
                }
                
                EditorApplication.update -= Progress;

                if (_currentDependencyIndex < _dependencies.Count - 1)
                {
                    _currentDependencyIndex++;
                    InstallDependency();
                }
                else
                {
                    string debugMsg = $"Installation of {_dependencies.Count} {(_dependencies.Count > 1 ? "packages" : "package")} completed";
                    
                    if (_fails.Count > 0)
                    {
                        debugMsg += $" with {_fails.Count} ";
                        debugMsg += _fails.Count > 1 ? "fails" : "fail";
                        debugMsg += "\n\nFAILED: \n";
                        foreach (string fail in _fails)
                        {
                            debugMsg += $"  {fail}\n";
                        }
                    }
                    else
                    {
                        debugMsg += "\n";
                    }
                    
                    if (_successes.Count > 0)
                    {
                        debugMsg += "\nINSTALLED: \n";
                        foreach (string success in _successes)
                        {
                            debugMsg += $"  {success}\n";
                        }
                    }

                    Debug.Log(debugMsg);
                }
            }
        }
    }
}