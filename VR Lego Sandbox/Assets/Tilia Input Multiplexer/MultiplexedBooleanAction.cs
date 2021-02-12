using Malimbe.XmlDocumentationAttribute;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zinnia.Action;

namespace TiliaInputMultiplexer
{
    public class MultiplexedBooleanAction : MonoBehaviour
    {
        public BooleanAction[] InputActions;
        public BooleanAction OutputAction;

        private void Start()
        {
            foreach (BooleanAction ba in InputActions)
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

        private void Activated(bool result)
        {
            OutputAction.Activated.Invoke(result);
        }

        private void ValueChanged(bool result)
        {
            OutputAction.ValueChanged.Invoke(result);
        }

        private void ValueUnchanged(bool result)
        {
            OutputAction.ValueUnchanged.Invoke(result);
        }

        private void Deactivated(bool result)
        {
            OutputAction.Deactivated.Invoke(result);
        }
    }
}