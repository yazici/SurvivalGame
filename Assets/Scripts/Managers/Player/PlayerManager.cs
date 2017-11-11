using UnityEngine;
using Pamux.Lib.Utilities;

namespace Pamux.Lib.Managers
{
    [RequireComponent(typeof(Player))]
    [RequireComponent(typeof(InventoryManager))]
    // [RequireComponent(typeof(CharacterFeaturesManager))]
    // [RequireComponent(typeof(CraftingManager))]
    // [RequireComponent(typeof(AchievementManager))]
    // [RequireComponent(typeof(BuildingManager))]
    // [RequireComponent(typeof(QuestManager))]
    // [RequireComponent(typeof(SidekicksManager))]
    // [RequireComponent(typeof(KnowledgeManager))]


    public class PlayerManager : Singleton<PlayerManager>
    {
        public static Player LocalPlayer;

        protected override void Awake()
        {
            base.Awake();

            LocalPlayer = GetComponent<Player>();
        }
    }
}