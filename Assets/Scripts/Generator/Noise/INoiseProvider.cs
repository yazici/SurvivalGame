namespace Pamux.Lib.LevelData.Generator
{
    public interface INoiseMaker
    {
        float GetElevationNoise(float x, float z);
        float GetTemperatureNoise(float x, float z);
        float GetMoistureNoise(float x, float z);
        float GetBiomeNoise(float x, float z);
    }
}