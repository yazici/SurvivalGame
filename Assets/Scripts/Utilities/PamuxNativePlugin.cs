using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

namespace Pamux.Lib.Utilities
{
    public class PamuxNativePlugin : MonoBehaviour
    {

        [DllImport("Pamux.Unity.NativePlugin", EntryPoint = "TestDivide")]
        public static extern float TestDivide(float a, float b);

        [DllImport("Pamux.Unity.NativePlugin", EntryPoint = "TestMultiply")]
        public static extern float TestMultiply(float a, float b);


        void Start()
        {
            float multiplyResult = TestDivide(3, 5);

            Debug.Log(multiplyResult.ToString());

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}