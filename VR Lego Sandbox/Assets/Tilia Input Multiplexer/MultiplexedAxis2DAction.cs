using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zinnia.Action;

namespace TiliaInputMultiplexer
{
    public class MultiplexedAxis2DAction : MonoBehaviour
    {
        public Vector2Action[] Axis2DActions;

        public Vector2Action OutputAction;

        private void Start()
        {
            foreach (Vector2Action ba in Axis2DActions)
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

        private void Activated(Vector2 result)
        {
            OutputAction.Activated.Invoke(result);
        }

        private void ValueChanged(Vector2 result)
        {
            OutputAction.ValueChanged.Invoke(result);
        }

        private void ValueUnchanged(Vector2 result)
        {
            OutputAction.ValueUnchanged.Invoke(result);
        }

        private void Deactivated(Vector2 result)
        {
            OutputAction.Deactivated.Invoke(result);
        }
    }
}