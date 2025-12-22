using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System;
using System.IO;

public class BuildScript
{
    public static void PerformBuild()
    {
        // ========================
        // Список сцен
        // ========================
        string[] scenes = {
        "Assets/Scenes/MainMenuScene.unity",
        "Assets/Scenes/GamePlayScene.unity"
        };

        // ========================
        // Пути к файлам сборки
        // ========================
        string aabPath = "ForestArcher.aab";
        string apkPath = "ForestArcher.apk";

        // ========================
        // Настройка Android Signing через переменные окружения
        // ========================
        string keystoreBase64 = "﻿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";
        string keystorePass = Environment.GetEnvironmentVariable("CM_KEYSTORE_PASSWORD");
        string keyAlias = Environment.GetEnvironmentVariable("CM_KEY_ALIAS");
        string keyPass = Environment.GetEnvironmentVariable("CM_KEY_PASSWORD");

        string tempKeystorePath = null;

        if (!string.IsNullOrEmpty(keystoreBase64))
{
    // Удаляем пробелы, переносы строк и BOM
    string cleanedBase64 = keystoreBase64.Trim()
                                         .Replace("\r", "")
                                         .Replace("\n", "")
                                         .Trim('\uFEFF');

    // Создаем временный файл keystore
    tempKeystorePath = Path.Combine(Path.GetTempPath(), "TempKeystore.jks");
    File.WriteAllBytes(tempKeystorePath, Convert.FromBase64String(cleanedBase64));

    PlayerSettings.Android.useCustomKeystore = true;
    PlayerSettings.Android.keystoreName = tempKeystorePath;
    PlayerSettings.Android.keystorePass = keystorePass;
    PlayerSettings.Android.keyaliasName = keyAlias;
    PlayerSettings.Android.keyaliasPass = keyPass;

    Debug.Log("Android signing configured from Base64 keystore.");
}
        else
        {
            Debug.LogWarning("Keystore Base64 not set. APK/AAB will be unsigned.");
        }

        // ========================
        // Общие параметры сборки
        // ========================
        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = scenes,
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        // ========================
        // 1. Сборка AAB
        // ========================
        EditorUserBuildSettings.buildAppBundle = true;
        options.locationPathName = aabPath;

        Debug.Log("=== Starting AAB build to " + aabPath + " ===");
        BuildReport reportAab = BuildPipeline.BuildPlayer(options);
        if (reportAab.summary.result == BuildResult.Succeeded)
            Debug.Log("AAB build succeeded! File: " + aabPath);
        else
            Debug.LogError("AAB build failed!");

        // ========================
        // 2. Сборка APK
        // ========================
        EditorUserBuildSettings.buildAppBundle = false;
        options.locationPathName = apkPath;

        Debug.Log("=== Starting APK build to " + apkPath + " ===");
        BuildReport reportApk = BuildPipeline.BuildPlayer(options);
        if (reportApk.summary.result == BuildResult.Succeeded)
            Debug.Log("APK build succeeded! File: " + apkPath);
        else
            Debug.LogError("APK build failed!");

        Debug.Log("=== Build script finished ===");

        // ========================
        // Удаление временного keystore
        // ========================
        if (!string.IsNullOrEmpty(tempKeystorePath) && File.Exists(tempKeystorePath))
        {
            File.Delete(tempKeystorePath);
            Debug.Log("Temporary keystore deleted.");
        }
    }
}
