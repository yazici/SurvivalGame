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
        public static Player localPlayer;
        public static Player LocalPlayer => localPlayer;

        private static Player thirdPersonPlayer;
        private static Player firstPersonPlayer;

        private GameObject thirdPersonCamera;


        public void ActivateLocalPlayerCameraType(PlayerCameraTypes playerCameraType)
        {
            Player inactivePlayer = null;

            switch (playerCameraType)
            {
                case PlayerCameraTypes.FirstPerson:
                    localPlayer = firstPersonPlayer;
                    inactivePlayer = thirdPersonPlayer;
                    thirdPersonCamera.SetActive(false);
                    break;

                case PlayerCameraTypes.ThirdPerson:
                    localPlayer = thirdPersonPlayer;
                    inactivePlayer = firstPersonPlayer;

                    var cameraFollow = thirdPersonCamera.GetComponent<SmoothFollow>();
                    cameraFollow.target = thirdPersonPlayer.transform;
                    thirdPersonCamera.SetActive(true);

                    break;
            }

            inactivePlayer.gameObject.SetActive(false);
            localPlayer.gameObject.SetActive(true);
        }

        private void OnChunkIsReady(string chunkKey)
        {
            ActivateLocalPlayerCameraType(PlayerCameraTypes.ThirdPerson);
        }

        protected override void Awake()
        {
            base.Awake();

            var res = Resources.Load("Prefabs/FirstPersonController") as GameObject;
            var go = Instantiate(res);
            go.transform.parent = this.transform;
            firstPersonPlayer = go.AddComponent<Player>();

            res = Resources.Load("Prefabs/ThirdPersonController") as GameObject;
            go = Instantiate(res);
            go.transform.parent = this.transform;
            thirdPersonPlayer = go.AddComponent<Player>();

            res = Resources.Load("Prefabs/CameraWithWeather") as GameObject;
            thirdPersonCamera = Instantiate(res);
            thirdPersonCamera.transform.parent = thirdPersonPlayer.transform;
            thirdPersonCamera.SetActive(false);
        }

        protected void Update()
        {
            if (!WorldManager.Instance.ChunkIsReady)
            {
                return;
            }

            if (localPlayer == null)
            { 
                ActivateLocalPlayerCameraType(PlayerCameraTypes.ThirdPerson);

                localPlayer.transform.position = new Vector3(localPlayer.transform.position.x, ChunkCache.GetElevation(localPlayer.transform.position) + 0.5f, localPlayer.transform.position.z);
                localPlayer.GetComponent<Rigidbody>().useGravity = true;

            }
        }
    }
}