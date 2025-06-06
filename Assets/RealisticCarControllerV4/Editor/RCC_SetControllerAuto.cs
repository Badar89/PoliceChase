﻿//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2014 - 2024 BoneCracker Games
// https://www.bonecrackergames.com
// Ekrem Bugra Ozdoganlar
//
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public static class RCC_SetControllerAuto {

    public static void GetPlatform() {

        BuildTarget currentPlatform = EditorUserBuildSettings.activeBuildTarget;

        switch (currentPlatform) {

            case BuildTarget.Android:
            case BuildTarget.iOS:
                RCC_Settings.Instance.mobileControllerEnabled = true;
                break;

            default:
                RCC_Settings.Instance.mobileControllerEnabled = false;
                break;

        }

    }

}
