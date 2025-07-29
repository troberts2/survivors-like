using System.Collections.Generic;

[System.Serializable]
public class SettingsData
{
    public float masterVolume = .7f;
    public float musicVolume = .7f;
    public float sfxVolume = .7f;
    public int resolutionIndex = 0;
    public bool isFullscreen = false;
}

[System.Serializable]
public class PlayerData
{
    public string playerName;
    public int level;
    public float health;
}

[System.Serializable]
public class LevelData
{
    public bool levelOneBeat = false;
}

[System.Serializable]
public class SaveFile
{
    public SettingsData settings = new SettingsData();
    public PlayerData player = new PlayerData();
    public LevelData level = new LevelData();
}
