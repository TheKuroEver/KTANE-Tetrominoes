using KModkit;
using UnityEngine;

// * ModKit Documentation: https://github.com/Qkrisi/ktanemodkit/wiki
[RequireComponent(typeof(KMBombModule), typeof(KMSelectable))]
[RequireComponent(typeof(KMAudio), typeof(KMBombInfo))]
public partial class ModuleModule : MonoBehaviour
{
    private KMBombModule _module;
    private KMBombInfo _bombInfo;
    private KMAudio _audio;

    private static int s_moduleCount;
    private int _moduleId;

#pragma warning disable IDE0051
    private void Awake() {
        _moduleId = s_moduleCount++;

        _module = GetComponent<KMBombModule>();
        _bombInfo = GetComponent<KMBombInfo>();
        _audio = GetComponent<KMAudio>();

        Log("Test");
    }
#pragma warning restore IDE0051

    public void Log(string message) => Debug.Log($"[{_module.ModuleDisplayName} #{_moduleId}] {message}");

    public void Strike(string message) {
        Log($"✕ {message}");
        _module.HandleStrike();
    }

    public void Solve() {
        Log("◯ Module solved!");
        _module.HandlePass();
    }
}
