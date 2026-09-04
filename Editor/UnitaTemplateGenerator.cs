#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Unita.Editor
{
    public class UnitaTemplateGenerator : EditorWindow
    {
        private string appTitle = "EitaaApp";
        private string loadingText = "Loading";
        private string fontFamily = "Arial, sans-serif";
        private int alignmentIndex = 2;
        private Color backgroundColor = new Color(0.039f, 0.039f, 0.051f);
        private Color loadingTextColor = Color.white;
        private string backgroundImageUrl = "";
        private UnityEngine.Object selectedFontFile;
        private UnityEngine.Object selectedBgImage;
        private bool showLoadingText = true;
        private bool showFullscreenButton = true;
        private string customHtmlSourcePath = "";

        private readonly string[] alignmentOptions = { "Left", "Right", "Middle" };

        [MenuItem("Unita/Template Configurator")]
        public static void ShowWindow()
        {
            GetWindow<UnitaTemplateGenerator>("Unita Template");
        }

        private void OnEnable()
        {
            appTitle = EditorPrefs.GetString("Unita_AppTitle", "برنامک ایتا");
            loadingText = EditorPrefs.GetString("Unita_LoadingText", "در حال بارگذاری...");
            fontFamily = EditorPrefs.GetString("Unita_FontFamily", "Arial, sans-serif");
            alignmentIndex = EditorPrefs.GetInt("Unita_Alignment", 2);
            string colorHex = EditorPrefs.GetString("Unita_BgColor", "#0a0a0d");
            ColorUtility.TryParseHtmlString(colorHex, out backgroundColor);
            string textColorHex = EditorPrefs.GetString("Unita_TextColor", "#ffffff");
            ColorUtility.TryParseHtmlString(textColorHex, out loadingTextColor);
            backgroundImageUrl = EditorPrefs.GetString("Unita_BgImage", "");
            showLoadingText = EditorPrefs.GetBool("Unita_ShowText", true);
            showFullscreenButton = EditorPrefs.GetBool("Unita_ShowButton", true);
            customHtmlSourcePath = EditorPrefs.GetString("Unita_CustomHtml", "");
            string fontPath = EditorPrefs.GetString("Unita_FontAssetPath", "");
            if (!string.IsNullOrEmpty(fontPath))
                selectedFontFile = AssetDatabase.LoadAssetAtPath<TextAsset>(fontPath);

            string bgPath = EditorPrefs.GetString("Unita_BgImageAssetPath", "");
            if (!string.IsNullOrEmpty(bgPath))
                selectedBgImage = AssetDatabase.LoadAssetAtPath<Texture2D>(bgPath);
        }

        private void OnGUI()
        {
            GUILayout.Label("Unita MiniApp Settings", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            appTitle = EditorGUILayout.TextField("MiniApp Title: ", appTitle);
            loadingText = EditorGUILayout.TextField("Loading Text: ", loadingText);
            fontFamily = EditorGUILayout.TextField("Font Name (CSS font-family):", fontFamily);

            alignmentIndex = EditorGUILayout.Popup("Text Aligment:", alignmentIndex, alignmentOptions);
            loadingTextColor = EditorGUILayout.ColorField("Loading text color:", loadingTextColor);
            backgroundColor = EditorGUILayout.ColorField("Background color:", backgroundColor);
            backgroundImageUrl = EditorGUILayout.TextField("Background image URL (optional):", backgroundImageUrl);
            selectedFontFile = EditorGUILayout.ObjectField("Font File (woff2):", selectedFontFile, typeof(TextAsset), false);
            selectedBgImage = EditorGUILayout.ObjectField("Background Image (png):", selectedBgImage, typeof(Texture2D), false);

            showLoadingText = EditorGUILayout.Toggle("Show loading text", showLoadingText);
            showFullscreenButton = EditorGUILayout.Toggle("Show fullscreen button", showFullscreenButton);

            EditorGUILayout.Space();

            GUILayout.Label("Or directly select a custom HTML file:", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();

            customHtmlSourcePath = EditorGUILayout.TextField("Path to HTML file:", customHtmlSourcePath);
            if (GUILayout.Button("Browse...", GUILayout.Width(80)))
            {
                customHtmlSourcePath = EditorUtility.OpenFilePanel("Select Custom HTML", "", "html");
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(20);

            if (GUILayout.Button("Generate Template", GUILayout.Height(35)))
            {
                GenerateTemplate();
            }
        }

private void GenerateTemplate()
{
    EditorPrefs.SetString("Unita_AppTitle", appTitle);
    EditorPrefs.SetString("Unita_LoadingText", loadingText);
    EditorPrefs.SetString("Unita_FontFamily", fontFamily);
    EditorPrefs.SetInt("Unita_Alignment", alignmentIndex);
    EditorPrefs.SetString("Unita_BgColor", "#" + ColorUtility.ToHtmlStringRGB(backgroundColor));
    EditorPrefs.SetString("Unita_TextColor", "#" + ColorUtility.ToHtmlStringRGB(loadingTextColor));
    EditorPrefs.SetString("Unita_BgImage", backgroundImageUrl);
    EditorPrefs.SetBool("Unita_ShowText", showLoadingText);
    EditorPrefs.SetBool("Unita_ShowButton", showFullscreenButton);
    EditorPrefs.SetString("Unita_CustomHtml", customHtmlSourcePath);
    
    string targetFolder = Path.Combine(Application.dataPath, "WebGLTemplates", "UnitaTemplate");
    if (!Directory.Exists(targetFolder))
        Directory.CreateDirectory(targetFolder);
    
    string fontFileName = "";
    string bgImageFileName = "";
    
    if (selectedFontFile != null)
    {
        string fontPath = AssetDatabase.GetAssetPath(selectedFontFile);
        fontFileName = Path.GetFileName(fontPath);
        string destFontPath = Path.Combine(targetFolder, fontFileName);
        if (File.Exists(destFontPath)) File.Delete(destFontPath);
        File.Copy(fontPath, destFontPath, true);
        EditorPrefs.SetString("Unita_FontAssetPath", fontPath);
    }
    else
    {
        EditorPrefs.SetString("Unita_FontAssetPath", "");
    }
    
    if (selectedBgImage != null)
    {
        string imagePath = AssetDatabase.GetAssetPath(selectedBgImage);
        bgImageFileName = Path.GetFileName(imagePath);
        string destImagePath = Path.Combine(targetFolder, bgImageFileName);
        if (File.Exists(destImagePath)) File.Delete(destImagePath);
        File.Copy(imagePath, destImagePath, true);
        EditorPrefs.SetString("Unita_BgImageAssetPath", imagePath);
    }
    else
    {
        EditorPrefs.SetString("Unita_BgImageAssetPath", "");
    }
    
    string targetHtml = Path.Combine(targetFolder, "index.html");

    if (!string.IsNullOrEmpty(customHtmlSourcePath) && File.Exists(customHtmlSourcePath))
    {
        File.Copy(customHtmlSourcePath, targetHtml, true);
        Debug.Log("<color=green>[Unita]</color> Your custom HTML file was successfully replaced.");
    }
    else
    {
        string hexBg = "#" + ColorUtility.ToHtmlStringRGB(backgroundColor);
        string hexText = "#" + ColorUtility.ToHtmlStringRGB(loadingTextColor);
        string align = alignmentIndex == 0 ? "left" : (alignmentIndex == 1 ? "right" : "center");
        string htmlContent = BuildHtmlContent(
            appTitle, loadingText, hexBg, hexText, fontFamily, align, backgroundImageUrl,
            showLoadingText, showFullscreenButton,
            fontFileName, bgImageFileName
        );
        File.WriteAllText(targetHtml, htmlContent);
        Debug.Log("<color=green>[Unita]</color> template generated with desired specifications.");
    }

    PlayerSettings.WebGL.template = "PROJECT:UnitaTemplate";
    AssetDatabase.Refresh();
}

    private string BuildHtmlContent(
    string title, string loading, string bg, string textColor, string font, string align, 
    string bgImage, bool showText, bool showButton,
    string fontFileName, string bgImageFileName)
{
    string template = @"
<!DOCTYPE html>
<html lang=""fa"" dir=""rtl"">
<head>
    <meta charset=""utf-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no, viewport-fit=cover"">
    <title>%%TITLE%%</title>
    <script src=""https://developer.eitaa.com/eitaa-web-app.js""></script>
    <style>
        %%FONT_FACE%%

        * {
            box-sizing: border-box;
            -webkit-tap-highlight-color: transparent;
        }
        html, body {
            width: 100%;
            height: 100%;
            margin: 0;
            padding: 0;
            overflow: hidden;
            %%BG_STYLE%%
        }
        body {
            display: flex;
            align-items: center;
            justify-content: center;
        }
        #unita-root {
            position: relative;
            width: 100vw;
            height: 100vh;
            overflow: hidden;
            background-color: transparent;
        }
        #unity-container {
            position: absolute;
            inset: 0;
            width: 100%;
            height: 100%;
        }
        #unity-canvas {
            display: block;
            width: 100%;
            height: 100%;
            background: transparent;
        }
        #unita-loading-screen {
            position: absolute;
            inset: 0;
            z-index: 10;
            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: center;
            gap: 18px;
            background-color: transparent;
            transition: opacity 0.25s ease;
        }
        #unita-loading-text {
            display: %%LOADING_DISPLAY%%;
            align-items: center;
            justify-content: center;
            max-width: 90%;
            color: %%TEXT_COLOR%%;
            font-family: %%FONT_FAMILY%%;
            font-size: clamp(14px, 2.5vw, 22px);
            line-height: 1.8;
            text-align: %%TEXT_ALIGN%%;
            direction: rtl;
        }
        #unita-progress-wrapper {
            display: block;
            width: min(72vw, 360px);
            height: 8px;
            overflow: hidden;
            border-radius: 999px;
            background-color: rgba(255,255,255,0.2);
        }
        #unita-progress-bar {
            width: 0%;
            height: 100%;
            border-radius: inherit;
            background-color: #26a7ff;
            transition: width 0.15s ease;
        }
        #unita-fullscreen-button {
            display: %%BUTTON_DISPLAY%%;
            align-items: center;
            justify-content: center;
            min-width: 160px;
            min-height: 42px;
            padding: 8px 18px;
            border: 0;
            border-radius: 10px;
            color: %%TEXT_COLOR%%;
            background-color: rgba(255,255,255,0.14);
            font-family: %%FONT_FAMILY%%;
            font-size: 14px;
            cursor: pointer;
        }
        #unita-fullscreen-button:active {
            transform: scale(0.98);
        }
    </style>
</head>
<body>
    <main id=""unita-root"">
        <div id=""unity-container"">
            <canvas id=""unity-canvas""></canvas>
        </div>

        <section id=""unita-loading-screen"">
            <div id=""unita-loading-text"">%%LOADING_TEXT%%</div>
            <div id=""unita-progress-wrapper"">
                <div id=""unita-progress-bar""></div>
            </div>
            <button id=""unita-fullscreen-button"" type=""button"">
                ورود به حالت تمام‌صفحه
            </button>
        </section>
    </main>

    <script>
        const canvas = document.querySelector('#unity-canvas');
        const loadingScreen = document.querySelector('#unita-loading-screen');
        const progressBar = document.querySelector('#unita-progress-bar');
        const fullscreenButton = document.querySelector('#unita-fullscreen-button');

        const buildUrl = 'Build';
        const loaderUrl = buildUrl + '/%%LOADER_FILENAME%%';

        const config = {
            dataUrl: buildUrl + '/%%DATA_FILENAME%%',
            frameworkUrl: buildUrl + '/%%FRAMEWORK_FILENAME%%',
            codeUrl: buildUrl + '/%%CODE_FILENAME%%',
            streamingAssetsUrl: 'StreamingAssets',
            companyName: '%%COMPANY_NAME%%',
            productName: '%%PRODUCT_NAME%%',
            productVersion: '%%PRODUCT_VERSION%%',
            showBanner: function(message, type) {
                console.log('[Unity]', type, message);
            }
        };

        if (fullscreenButton) {
            fullscreenButton.addEventListener('click', function() {
                const root = document.documentElement;
                if (root.requestFullscreen) {
                    root.requestFullscreen();
                } else if (root.webkitRequestFullscreen) {
                    root.webkitRequestFullscreen();
                }
            });
        }

        function hideLoadingScreen() {
            if (!loadingScreen) return;
            loadingScreen.style.opacity = '0';
            setTimeout(function() {
                loadingScreen.style.display = 'none';
            }, 260);
        }

        function updateProgress(value) {
            if (progressBar) {
                progressBar.style.width = Math.round(value * 100) + '%';
            }
        }

        const script = document.createElement('script');
        script.src = loaderUrl;
        script.onload = function() {
            createUnityInstance(canvas, config, function(progress) {
                updateProgress(progress);
            }).then(function(unityInstance) {
                window.unityInstance = unityInstance;

                if (window.Eitaa && window.Eitaa.WebApp) {
                    window.Eitaa.WebApp.ready();
                    window.Eitaa.WebApp.expand();
                }

                updateProgress(1);
                hideLoadingScreen();
            }).catch(function(message) {
                console.error(message);
            });
        };
        document.body.appendChild(script);
    </script>
</body>
</html>";
    
    string fontFace = "";
    if (!string.IsNullOrEmpty(fontFileName) && File.Exists(Path.Combine(Path.GetDirectoryName(Application.dataPath), "Assets/WebGLTemplates/UnitaTemplate", fontFileName)))
    {
        fontFace = $"@font-face {{ font-family: 'CustomFont'; src: url('./{fontFileName}') format('woff2'); }}";
        font = $"'CustomFont', {font}";
    }
    
    string bgStyle = $"background-color: {bg};";
    if (!string.IsNullOrEmpty(bgImageFileName) && File.Exists(Path.Combine(Path.GetDirectoryName(Application.dataPath), "Assets/WebGLTemplates/UnitaTemplate", bgImageFileName)))
    {
        bgStyle = $"background-image: url('./{bgImageFileName}'); background-size: cover; background-position: center;";
        bgStyle += $" background-color: {bg};";
    }
    else if (!string.IsNullOrEmpty(bgImage))
    {
        bgStyle += $" background-image: url('{bgImage}'); background-size: cover; background-position: center;";
    }

    string loadingDisplay = showText ? "flex" : "none";
    string buttonDisplay = showButton ? "flex" : "none";
    
    string result = template
        .Replace("%%TITLE%%", title)
        .Replace("%%LOADING_TEXT%%", loading)
        .Replace("%%FONT_FAMILY%%", font)
        .Replace("%%TEXT_ALIGN%%", align)
        .Replace("%%BG_STYLE%%", bgStyle)
        .Replace("%%TEXT_COLOR%%", textColor)
        .Replace("%%LOADING_DISPLAY%%", loadingDisplay)
        .Replace("%%BUTTON_DISPLAY%%", buttonDisplay)
        .Replace("%%FONT_FACE%%", fontFace)  // اضافه شد
        .Replace("%%LOADER_FILENAME%%", "{{{ LOADER_FILENAME }}}")
        .Replace("%%DATA_FILENAME%%", "{{{ DATA_FILENAME }}}")
        .Replace("%%FRAMEWORK_FILENAME%%", "{{{ FRAMEWORK_FILENAME }}}")
        .Replace("%%CODE_FILENAME%%", "{{{ CODE_FILENAME }}}")
        .Replace("%%COMPANY_NAME%%", "{{{ COMPANY_NAME }}}")
        .Replace("%%PRODUCT_NAME%%", "{{{ PRODUCT_NAME }}}")
        .Replace("%%PRODUCT_VERSION%%", "{{{ PRODUCT_VERSION }}}");

    return result;
}
    }
}
#endif