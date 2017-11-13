using Pamux.Lib.Enums;
using UnityEngine;

namespace Pamux.Lib.Utilities
{
    public abstract class PlayerPointOfView : MonoBehaviour
    {
        internal PlayerPointOfView OtherLocalPlayerPointOfView;

        internal void Activate()
        {
            OtherLocalPlayerPointOfView.gameObject.SetActive(false);

            this.transform.position = OtherLocalPlayerPointOfView.transform.position;
            this.transform.rotation = OtherLocalPlayerPointOfView.transform.rotation;
            this.transform.localScale = OtherLocalPlayerPointOfView.transform.localScale;

            gameObject.SetActive(true);
        }
    }
}