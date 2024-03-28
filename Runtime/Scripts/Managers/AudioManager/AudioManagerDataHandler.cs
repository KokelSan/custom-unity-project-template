using System;

public static class AudioManagerDataHandler
{
        public static float GetVolume() => OnGetVolume?.Invoke() ?? 0f;
        public static event Func<float> OnGetVolume;
        
        public static void UpdateVolume(float value) => OnVolumeUpdated?.Invoke(value);
        public static event Action<float> OnVolumeUpdated;
}