using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.Animations;

public class AnimtorTool : Editor {

 
    public static string modePrefabsPath = "Assets/Resources/animator/";

    [MenuItem("Tools/动画查错", priority = 0)]
    public static void FreshAnimtor()
    {
        FileInfo[] modeDirect = new DirectoryInfo(modePrefabsPath).GetFiles();

        for (int i = 0; i < modeDirect.Length; i++)
        {
            if (modeDirect[i].Name.Contains("meta"))
            {
                continue;
            }
            string modelName = modeDirect[i].Name;

            string animtorPath = modePrefabsPath + modelName;

            //设置动画控制器内参数
            AnimatorController AnimatorTemplate = AssetDatabase.LoadAssetAtPath(animtorPath, typeof(AnimatorController)) as AnimatorController;

            if (AnimatorTemplate == null)
            {
                if (EditorUtility.DisplayDialog("错误的路径", "寻找动画路径失败：" + animtorPath + ",检查动画控制器名字是否和模型名字匹配", "继续"))
                {
                    return;
                }
            }

            foreach (var obj in AnimatorTemplate.layers[0].stateMachine.states)
            {
                if (obj.state.motion == null)
                {
                    if (EditorUtility.DisplayDialog("存在空的动画Clip", "动画" + animtorPath + "的状态" + obj.state.name + "为空", "继续"))
                    {
                        continue;
                    }
                }
            }

        }
    }
}
