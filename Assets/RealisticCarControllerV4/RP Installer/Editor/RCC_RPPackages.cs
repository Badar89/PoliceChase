//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2014 - 2024 BoneCracker Games
// https://www.bonecrackergames.com
// Ekrem Bugra Ozdoganlar
//
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// All RP packages.
/// </summary>
public class RCC_RPPackages : ScriptableObject {

    #region singleton
    private static RCC_RPPackages instance;
    public static RCC_RPPackages Instance { get { if (instance == null) instance = Resources.Load("RCC_RPPackages") as RCC_RPPackages; return instance; } }
    #endregion

    public Object URP;
    public Object HDRP;

    public bool dontWarnAgain = false;

    public string GetAssetPath(Object pathObject) {

        string path = UnityEditor.AssetDatabase.GetAssetPath(pathObject);
        return path;

    }

}
