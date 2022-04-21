using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System.IO;

public class NowUIFontCompiler : Editor
{
    [MenuItem("Assets/NowUI/Compile Font")]
    public static void CompileFonts()
    {
        var msdf = Resources.Load<TextAsset>("msdf-atlas-gen");
        var selection = Selection.objects;

        Process p = new Process();
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.RedirectStandardError = true;
        p.StartInfo.CreateNoWindow = true;
        p.StartInfo.FileName = AssetDatabase.GetAssetPath(msdf);

        for (int i = 0; i < selection.Length; ++i)
        {
            var target = selection[i];
            if(target is Font)
            {
                var fontPath = AssetDatabase.GetAssetPath(target);

                p.StartInfo.Arguments = $"-type mtsdf -font {fontPath} -format png -imageout {fontPath}.png -json {fontPath}.json -dimensions 1024 1024";
                p.Start();

                EditorUtility.DisplayProgressBar("Compile Font", target.name, i / (float)selection.Length * 100f);

                p.WaitForExit();

                AssetDatabase.Refresh();

                if (p.ExitCode != 0) 
                {
                    Debug.LogError("Failed to compile " + target.name);
                    continue;
                }

                try
                {
                    var newFontPath = $"{fontPath}.asset";

                    if (File.Exists(newFontPath)) File.Delete(newFontPath);

                    NowFont font = CreateInstance(typeof(NowFont)) as NowFont;
                    AssetDatabase.CreateAsset(font, newFontPath);

                    var otexture = AssetDatabase.LoadAssetAtPath($"{fontPath}.png", typeof(Texture2D)) as Texture2D;
                    var ojson = AssetDatabase.LoadAssetAtPath($"{fontPath}.json", typeof(TextAsset)) as TextAsset;

                    Texture2D texture = new Texture2D(otexture.width, otexture.height);
                    texture.name = "Font Atlas Texture";
                    texture.LoadImage(File.ReadAllBytes($"{fontPath}.png"), true);

                    Material fontMat = Instantiate(Resources.Load("NowUI/TxtMaterial")) as Material;

                    fontMat.mainTexture = texture;

                    AssetDatabase.AddObjectToAsset(texture, newFontPath);
                    AssetDatabase.AddObjectToAsset(fontMat, newFontPath);
                    AssetDatabase.Refresh();

                    font.Atlas = texture;
                    font.Material = fontMat;
                    font.AtlasInfo = JsonUtility.FromJson<NowFontAtlasInfo>(ojson.text);

                    AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(texture));

                    AssetDatabase.DeleteAsset($"{fontPath}.png");
                    AssetDatabase.DeleteAsset($"{fontPath}.json");
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("Failed to compile " + target.name + "\n" + ex.Message + "\n" + ex.StackTrace);
                }
            }
        }

        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();
    }
}
