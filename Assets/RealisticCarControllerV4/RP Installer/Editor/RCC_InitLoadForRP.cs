//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2014 - 2024 BoneCracker Games
// https://www.bonecrackergames.com
// Ekrem Bugra Ozdoganlar
//
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

public class RCC_InitLoadForRP {

    [InitializeOnLoadMethod]
    public static void InitOnLoad() {

        EditorApplication.delayCall += EditorDelayedUpdate;
        EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
        EditorApplication.projectChanged += OnProjectChanged;

    }

    public static void EditorDelayedUpdate() {

        if (RCC_RPPackages.Instance.dontWarnAgain)
            return;

        if (EditorApplication.isPlayingOrWillChangePlaymode)
            return;

        Check();

    }

    private static void EditorApplication_playModeStateChanged(PlayModeStateChange obj) {

        if (RCC_RPPackages.Instance.dontWarnAgain)
            return;

        if (obj == PlayModeStateChange.EnteredEditMode)
            Check();

    }

    public static void Check() {

        if (RCC_RPPackages.Instance.dontWarnAgain)
            return;

        bool installedRP = false;

#if RCC_RP
        installedRP = true;
#endif

        if (!installedRP) {

            int decision = EditorUtility.DisplayDialogComplex("Realistic Car Controller | Select Render Pipeline", "Which render pipeline will be imported?\n\nThis process can't be repeated, once you select the render pipeline, compatible version of RCC will be imported. Switching between render pipelines are not supported.\n\nIf you want to change the render pipeline after this step, you'll need to delete ''Realistic Car Controller'' folder from the project and import the new render pipeline. Be sure your project has proper configuration setup for the selected render pipeline. ", "Import [Universal Render Pipeline] (URP)", "Ignore", "Import [High Definition Render Pipeline] (HDRP)");

            if (decision == 0) {

                SessionState.SetBool("RCC_INSTALLINGRP", true);
                AssetDatabase.ImportPackage(RCC_RPPackages.Instance.GetAssetPath(RCC_RPPackages.Instance.URP), true);

            }

            if (decision == 2) {

                SessionState.SetBool("RCC_INSTALLINGRP", true);
                AssetDatabase.ImportPackage(RCC_RPPackages.Instance.GetAssetPath(RCC_RPPackages.Instance.HDRP), true);

            }

            if (decision == 1) {

                EditorUtility.DisplayDialog("Realistic Car Controller | Not Imported Yet", "Realistic Car Controller is not installed yet, because you've clicked the ignore button. You can manually import the URP / HDRP packages located in RealisticCarControllerV4/RP Installer.", "Dismiss");

                SessionState.EraseBool("RCC_INSTALLINGRP");
                RCC_SetScriptingSymbolForRP.SetEnabled("RCC_RP", true);

            }

            RCC_RPPackages.Instance.dontWarnAgain = true;

            EditorUtility.SetDirty(RCC_RPPackages.Instance);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

        }

    }

    public static void OnProjectChanged() {

        if (SessionState.GetBool("RCC_INSTALLINGRP", false)) {

            SessionState.EraseBool("RCC_INSTALLINGRP");
            RCC_SetScriptingSymbolForRP.SetEnabled("RCC_RP", true);

        }

    }

}
