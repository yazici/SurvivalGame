using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AClockworkBerry;
using Pamux.Utilities;
using Pamux.Lib.WorldGen;

namespace Pamux.Lib.Managers
{

    public class GameManager : Singleton<GameManager>
    {
        public WorldGenerator WorldGenerator;
        public WorldGeneratorSettings WorldGeneratorSettings;

        public ScreenLogger ScreenLogger;

        public AssetManager AssetManager;
        public AudioManager AudioManager;
        public ContentManager ContentManager;
        public UIManager UIManager;
        public InputManager InputManager;
        public InventoryManager InventoryManager;
        public PersistenceManager PersistenceManager;
        public PlayerManager PlayerManager;
        public TimeManager TimeManager;
        public WorldManager WorldManager;
        public WeatherManager WeatherManager;
        public PhotographyManager PhotographyManager;
        public CinematographyManager CinematographyManager;
        public ConfigurationManager ConfigurationManager;

        // public CacheManager CacheManager;
        // public NetworkingManager NetworkingManager;
        // public AIManager AIManager;
        // public AchievementManager AchievementManager;
        // public QuestManager QuestManager;
        // public AdsManager AdsManager;
        // public InAppPurchaseManager InAppPurchaseManager;
        // public MonetizationManager MonetizationManager;
        // public InGameEconomyManager InGameEconomyManager;
        // public CharacterFeaturesManager CharacterFeaturesManager;
        // public NPCManager NPCManager;
        // public TerrainManager TerrainManager;
        // public TerrainObjectsManager TerrainObjectsManager;
        // public DatabaseManager DatabaseManager;
        // public CraftingManager CraftingManager;
        // public BuildingManager BuildingManager;
        // public WorldDiscoveryManager WorldDiscoveryManager;
        // public NarrativeManager NarrativeManager;
        // public SidekicksManager SidekicksManager;



        public Camera MainCamera;
        public Player LocalPlayer;

        void Start()
        {
            ScreenLogger.Instance.ShowLog = true;
        }
    }
}