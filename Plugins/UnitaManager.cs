//developedBy: Sajad Ahmadi Niat (IceRose Studio)
using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Unita
{
    public class UnitaManager : MonoBehaviour
{
    public static UnitaManager Instance { get; private set; }
        
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void EitaaReady();

    [DllImport("__Internal")]
    private static extern void EitaaExpand();

    [DllImport("__Internal")]
    private static extern void EitaaClose();

    [DllImport("__Internal")]
    private static extern void EitaaGetInitData(string objectName, string callbackName);

    [DllImport("__Internal")]
    private static extern void EitaaGetVersion(string objectName, string callbackName);

    [DllImport("__Internal")]
    private static extern void EitaaSetHeaderColor(string colorHex);

    [DllImport("__Internal")]
    private static extern void EitaaSetBackgroundColor(string colorHex);

    [DllImport("__Internal")]
    private static extern void EitaaShowAlert(string message);

    [DllImport("__Internal")]
    private static extern void EitaaSetMainButton(string text, string colorHex, string textColorHex, bool isVisible, bool isActive);

    [DllImport("__Internal")]
    private static extern void EitaaHapticImpact(string style);
#endif
    
    public static event Action<string> OnInitDataReceived;
    public static event Action<string> OnVersionReceived;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Ready();
        RequestInitData();
    }

    public void Ready()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        EitaaReady();
#else
        Debug.Log("[Eitaa] Ready called (Editor Mode)");
#endif
    }

    public void Expand()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        EitaaExpand();
#else
        Debug.Log("[Eitaa] Expand called");
#endif
    }

    public void CloseWebApp()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        EitaaClose();
#else
        Debug.Log("[Eitaa] Close WebApp called");
#endif
    }
    
    public void RequestInitData()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        EitaaGetInitData(gameObject.name, nameof(OnReceiveInitData));
#else
        Debug.Log("[Eitaa] RequestInitData called");
        OnReceiveInitData("mock_init_data_for_editor");
#endif
    }
    
    public void RequestVersion()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        EitaaGetVersion(gameObject.name, nameof(OnReceiveVersion));
#else
        Debug.Log("[Eitaa] RequestVersion called");
        OnReceiveVersion("1.0.editor");
#endif
    }

    public void SetHeaderColor(string colorHex)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        EitaaSetHeaderColor(colorHex);
#else
        Debug.Log($"[Eitaa] Header Color set to: {colorHex}");
#endif
    }

    public void SetBackgroundColor(string colorHex)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        EitaaSetBackgroundColor(colorHex);
#else
        Debug.Log($"[Eitaa] Background Color set to: {colorHex}");
#endif
    }
    
    public void ShowAlert(string message)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        EitaaShowAlert(message);
#else
        Debug.Log($"[Eitaa Alert]: {message}");
#endif
    }

    public void ConfigureMainButton(string text, string colorHex, string textColorHex, bool isVisible, bool isActive)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        EitaaSetMainButton(text, colorHex, textColorHex, isVisible, isActive);
#else
        Debug.Log($"[Eitaa MainButton] Text: {text}, Visible: {isVisible}, Active: {isActive}");
#endif
    }

    public void TriggerHaptic(string style = "light")
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        EitaaHapticImpact(style);
#else
        Debug.Log($"[Eitaa Haptic] Style: {style}");
#endif
    }

    #region JS Callbacks
    
    [Obsolete("This method is called directly by JavaScript.")]
    public void OnReceiveInitData(string rawData)
    {
        Debug.Log($"[Eitaa Data Received]: {rawData}");
        OnInitDataReceived?.Invoke(rawData);
    }

    [Obsolete("This method is called directly by JavaScript.")]
    public void OnReceiveVersion(string version)
    {
        Debug.Log($"[Eitaa Version Received]: {version}");
        OnVersionReceived?.Invoke(version);
    }

    #endregion
}
}
