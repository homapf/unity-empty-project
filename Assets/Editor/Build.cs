using System;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEditor.PackageManager;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

public class Build
{
    public static void ExportUnityPackage(PackageInfo packageInfo)
    {
        string packageDirectory = Path.Combine(Application.dataPath,
            $"../Build/{packageInfo.name}/{packageInfo.version}");

        if (!Directory.Exists(packageDirectory))
        {
            Directory.CreateDirectory(packageDirectory);
        }

        // Export package
        string finalPackageName =
            $"{packageDirectory}/{packageInfo.name.Split('.').Last()}_v{packageInfo.version}.unitypackage";
        AssetDatabase.ExportPackage(packageInfo.assetPath, finalPackageName, ExportPackageOptions.Recurse);
        Debug.Log($"[Package Exporter] {packageInfo.name} successfully exported to {finalPackageName}");
    }

    [MenuItem("Homa Games/CreateAllEmbeddedUnityPackages")]
    public static void CreateAllEmbeddedUnityPackages()
    {
        var list = Client.List(false);
        while (!list.IsCompleted)
        {
            Thread.Sleep(25);
        }

        foreach (var packageInfo in list.Result)
        {
            if (packageInfo.source == PackageSource.Embedded)
            {
                ExportUnityPackage(packageInfo);
            }
        }
    }
}