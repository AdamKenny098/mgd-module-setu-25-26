using UnityEngine;

public static class HardwareBootstrap
{
    enum DeviceTier { Low, Medium, High }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void ApplyHardwareSettings()
    {
        DeviceTier tier = DetectDeviceTier(out int ram, out int vram, out int cores);

        int targetFps;
        int qualityIndex;

        switch (tier)
        {
            case DeviceTier.Low:
                targetFps = 30;
                qualityIndex = 0; // low quality preset
                break;

            case DeviceTier.Medium:
                targetFps = 60;
                qualityIndex = 1; // medium quality preset
                break;

            default:
                targetFps = 60;
                qualityIndex = 2; // high quality preset
                break;
        }

        // Disable vsync so FPS actually applies
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFps;

        qualityIndex = Mathf.Clamp(qualityIndex, 0, QualitySettings.names.Length - 1);
        QualitySettings.SetQualityLevel(qualityIndex, true);

        Debug.Log($"[Bootstrap] Tier={tier} | FPS={targetFps} | Quality={QualitySettings.names[qualityIndex]} | RAM={ram}MB VRAM={vram}MB Cores={cores}");
    }

    static DeviceTier DetectDeviceTier(out int ram, out int vram, out int cores)
    {
        ram = SystemInfo.systemMemorySize;
        vram = SystemInfo.graphicsMemorySize;
        cores = SystemInfo.processorCount;

        // thresholds
        if (ram < 4000 || (vram > 0 && vram < 1000) || cores <= 4)
            return DeviceTier.Low;

        if (ram < 7000 || cores <= 6)
            return DeviceTier.Medium;

        return DeviceTier.High;
    }
}
