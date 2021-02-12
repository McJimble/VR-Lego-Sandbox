using System.Collections;
using System.Collections.Generic;
using Tilia.Input.UnityInputManager;
using UnityEngine;
using Zinnia.Action;

namespace TiliaInputMultiplexer
{
    public class MultiplexedAxis1DAction : MonoBehaviour
    {
        public FloatAction[] Axis1DActions;

        public FloatAction OutputAction;

        private void Start()
        {
            foreach (FloatAction ba in Axis1DActions)
            {
                ba.ActivationStateChanged.AddListener(ActivationStateChanged);
                ba.Activated.AddListener(Activated);
                ba.ValueChanged.AddListener(ValueChanged);
                ba.ValueUnchanged.AddListener(ValueUnchanged);
                ba.Deactivated.AddListener(Deactivated);
            }
        }

        private void ActivationStateChanged(bool result)
        {
            OutputAction.ActivationStateChanged.Invoke(result);
        }

        private void Activated(float result)
        {
            OutputAction.Activated.Invoke(result);
        }

        private void ValueChanged(float result)
        {
            OutputAction.ValueChanged.Invoke(result);
        }

        private void ValueUnchanged(float result)
        {
            OutputAction.ValueUnchanged.Invoke(result);
        }

        private void Deactivated(float result)
        {
            OutputAction.Deactivated.Invoke(result);
        }
    }
}