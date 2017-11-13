using UnityEngine;
using Pamux.Lib.Utilities;
using Pamux.Lib.Enums;
using Pamux.Lib.GameObjects;
using Pamux.Lib.WorldGen;

namespace Pamux.Lib.Managers
{
    //[RequireComponent(typeof(Player))]
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
        public static PlayerPointOfView LocalPlayerPointOfView => localPlayerPointOfViewType == PlayerPointOfViewTypes.FirstPerson
                                                ? firstPersonPlayerPointOfView as PlayerPointOfView
                                                : thirdPersonPlayerPointOfView as PlayerPointOfView;

        private static ThirdPersonPlayerPointOfView thirdPersonPlayerPointOfView;
        private static FirstPersonPlayerPointOfView firstPersonPlayerPointOfView;        

        private static PlayerPointOfViewTypes localPlayerPointOfViewType = PlayerPointOfViewTypes.Unset;
        public static PlayerPointOfViewTypes LocalPlayerPointOfViewType
        {
            get
            {
                return localPlayerPointOfViewType;
            }

            set
            {
                if (value == localPlayerPointOfViewType)
                {
                    return;
                }

                localPlayerPointOfViewType = value;
                LocalPlayerPointOfView.Activate();
            }
        }

        protected override void Awake()
        {
            base.Awake();

            var res = Resources.Load("Prefabs/FirstPersonController") as GameObject;
            var go = Instantiate(res);
            go.SetActive(false);
            go.transform.parent = this.transform;
            firstPersonPlayerPointOfView = go.AddComponent<FirstPersonPlayerPointOfView>();

            res = Resources.Load("Prefabs/ThirdPersonController") as GameObject;
            go = Instantiate(res);
            go.SetActive(false);
            go.transform.parent = this.transform;
            thirdPersonPlayerPointOfView = go.AddComponent<ThirdPersonPlayerPointOfView>();

            thirdPersonPlayerPointOfView.OtherLocalPlayerPointOfView = firstPersonPlayerPointOfView;
            firstPersonPlayerPointOfView.OtherLocalPlayerPointOfView = thirdPersonPlayerPointOfView;
        }

        protected void Update()
        {
            if (!WorldManager.Instance.ChunkIsReady)
            {
                return;
            }

            if (localPlayerPointOfViewType == PlayerPointOfViewTypes.Unset)
            { 
                firstPersonPlayerPointOfView.transform.position 
                    = new Vector3(firstPersonPlayerPointOfView.transform.position.x, 
                                    ChunkCache.GetElevation(firstPersonPlayerPointOfView.transform.position) + 0.5f,
                                    firstPersonPlayerPointOfView.transform.position.z);

                firstPersonPlayerPointOfView.GetComponent<Rigidbody>().useGravity = true;

                LocalPlayerPointOfViewType = PlayerPointOfViewTypes.ThirdPerson;
            }
        }
    }
}