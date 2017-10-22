using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(SceneConfig))]
public class SceneConfigEditor : Editor
{

    SceneConfig mScript;

    /// <summary>
    /// 脚本激活的时候进入，target就是对应[CustomEditor(typeof(SceneConfig))]的SceneConfig类
    /// </summary>
    public void OnEnable()
    {
        mScript = target as SceneConfig;
        if (mScript.mInfo == null)
        {
            mScript.mInfo = new sceneConfigObject();
        }
    }

    /// <summary>
    /// 重载脚本的界面
    /// </summary>
    public override void OnInspectorGUI()
    {
        mScript.mInfo.mIndex = EditorGUILayout.TextField("场景配置名", mScript.mInfo.mIndex);

        mScript.mInfo.spawnPos = EditorGUILayout.Vector3Field("出生点位置", mScript.mInfo.spawnPos);


        if (GUILayout.Button("导入"))
        {
            if (string.IsNullOrEmpty(mScript.mInfo.mIndex))
            {
                Debug.LogError("未输入配置名");
                return;
            }

            string path = "config/" + mScript.mInfo.mIndex;

            var configObj = Resources.Load(path) as sceneConfigObject;
            if (configObj != null)
            {
                configObj = Instantiate(configObj);
                configObj.name = mScript.mInfo.mIndex;
            }
            mScript.mInfo = configObj;
        }

        if (GUILayout.Button("导出"))
        {
            if (string.IsNullOrEmpty(mScript.mInfo.mIndex))
            {
                Debug.LogError("未输入配置名");
                return;
            }

            string path = "Assets/Resources/config/" + mScript.mInfo.mIndex + ".asset";

            if (File.Exists(path))
            {
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.SaveAssets();
            }


            AssetDatabase.CreateAsset(Instantiate(mScript.mInfo), "Assets/Resources/config/" + mScript.mInfo.mIndex + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

}
