using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

namespace Pamux.Lib.Utilities
{
    [RequireComponent(typeof(ThirdPersonCharacter))]
    [RequireComponent(typeof(ThirdPersonUserControl))]
    [RequireComponent(typeof(ClickToMove))]
    public class Player : MonoBehaviour
    {
    }
}