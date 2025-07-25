using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.PackageManager;
#endif
using UnityEngine;

namespace RomDev.TestingPackageSpace
{
    [InitializeOnLoad]
    public static class TestingPackage
    {
        private const string SessionDefineCheck = "RomDev_TestingPackage_DefineCheck";
        private const string SelfPackageName = "com.romezos.testingpackage";
        public static readonly string[] installedSymbol = new string[]
        {
        "ROMDEV_TESTINGPACKAGE_INSTALLED"
        };

        static TestingPackage()
        {
            if (!SessionState.GetBool(SessionDefineCheck, false))
            {
                //Debug.Log("Session with name " + SessionDefineCheck + ", isn't exist");
                SessionState.SetBool(SessionDefineCheck, true);
                AddScriptingSymbols();
                Debug.Log("Package : " + SelfPackageName + ", added");
                Events.registeringPackages -= OnPackageWillRemoved;
                Events.registeringPackages += OnPackageWillRemoved;
            }
            else
            {
                //Debug.Log("Session with name " + SessionDefineCheck + ", already exist");
                return;
            }
            //Debug.Log("Loaded or compiled");
        }
        public static void TestMethod()
        {
            Debug.Log("Method run");
        }
        private static void AddScriptingSymbols()
        {
            PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, out string[] defines);
            List<string> scriptingDefinesStringList = defines.ToList();
            scriptingDefinesStringList.AddRange(installedSymbol.Except(scriptingDefinesStringList));
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, scriptingDefinesStringList.ToArray());
        }
        private static void OnPackageWillRemoved(PackageRegistrationEventArgs args)
        {
            Debug.Log("Some package deleted");
            foreach(UnityEditor.PackageManager.PackageInfo packageInfo in args.removed)
            {
                Debug.Log(packageInfo.name + " is deleted");
                if(packageInfo.name == SelfPackageName)
                {
                    RemoveScriptingSymbols();
                    SessionState.EraseBool(SessionDefineCheck);
                    return;
                }
            }
        }
        private static void RemoveScriptingSymbols()
        {
            PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, out string[] defines);
            List<string> scriptingDefinesStringList = defines.ToList();
            scriptingDefinesStringList.Remove(installedSymbol[0]);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, scriptingDefinesStringList.ToArray());
        }
    }
}

