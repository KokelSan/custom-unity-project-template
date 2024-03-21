using System;

public static class AudioServiceDataHandler
{
        public static void UpdateVolume(float value) => OnVolumeUpdated?.Invoke(value);
        public static event Action<float> OnVolumeUpdated;
}