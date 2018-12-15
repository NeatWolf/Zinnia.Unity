﻿namespace VRTK.Core.Prefabs.Pointers
{
    using UnityEngine;
    using VRTK.Core.Action;
    using VRTK.Core.Cast;
    using VRTK.Core.Extension;
    using VRTK.Core.Tracking.Follow;

    /// <summary>
    /// Sets up the Pointer Prefab based on the provided user settings.
    /// </summary>
    public class PointerInternalSetup : MonoBehaviour
    {
        #region Facade Settings
        /// <summary>
        /// The public interface facade.
        /// </summary>
        [Header("Facade Settings"), Tooltip("The public interface facade.")]
        public PointerFacade facade;
        #endregion

        #region Object Follow Settings
        /// <summary>
        /// The <see cref="ObjectFollower"/> component for the Pointer.
        /// </summary>
        [Header("Object Follow Settings"), Tooltip("The ObjectFollower component for the Pointer.")]
        public ObjectFollower objectFollow;
        #endregion

        #region Cast Settings
        /// <summary>
        /// The <see cref="PointsCast"/> component for the Pointer.
        /// </summary>
        [Header("Cast Settings"), Tooltip("The PointsCast component for the Pointer.")]
        public PointsCast caster;
        #endregion

        #region Action Settings
        /// <summary>
        /// The <see cref="BooleanAction"/> that will activate/deactivate the pointer.
        /// </summary>
        [Header("Action Settions"), Tooltip("The BooleanAction that will activate/deactivate the pointer.")]
        public BooleanAction activationAction;
        /// <summary>
        /// The <see cref="BooleanAction"/> that initiates the pointer selection when the action is activated.
        /// </summary>
        [Tooltip("The BooleanAction that initiates the pointer selection when the action is activated.")]
        public BooleanAction selectOnActivatedAction;
        /// <summary>
        /// The <see cref="BooleanAction"/> that initiates the pointer selection when the action is deactivated.
        /// </summary>
        [Tooltip("The BooleanAction that initiates the pointer selection when the action is deactivated.")]
        public BooleanAction selectOnDeactivatedAction;
        #endregion

        /// <summary>
        /// Sets up the Pointer prefab with the specified settings.
        /// </summary>
        public virtual void Setup()
        {
            if (InvalidParameters())
            {
                return;
            }

            caster.targetValidity = facade.targetValidity;

            if (facade.followSource != null)
            {
                objectFollow.ClearSources();
                objectFollow.AddSource(facade.followSource);
            }

            if (facade.selectionAction != null)
            {
                selectOnActivatedAction.ClearSources();
                selectOnActivatedAction.AddSource(facade.selectionAction);
                selectOnDeactivatedAction.ClearSources();
                selectOnDeactivatedAction.AddSource(facade.selectionAction);
            }

            if (facade.activationAction != null)
            {
                activationAction.ClearSources();
                activationAction.AddSource(facade.activationAction);
            }

            switch (facade.selectionType)
            {
                case PointerFacade.SelectionType.SelectOnActivate:
                    selectOnActivatedAction.gameObject.SetActive(true);
                    selectOnDeactivatedAction.gameObject.SetActive(false);
                    break;
                case PointerFacade.SelectionType.SelectOnDeactivate:
                    selectOnActivatedAction.gameObject.SetActive(false);
                    selectOnDeactivatedAction.gameObject.SetActive(true);
                    break;
            }
        }

        /// <summary>
        /// Clears all of the settings from the Pointer prefab.
        /// </summary>
        public virtual void Clear()
        {
            if (InvalidParameters())
            {
                return;
            }

            objectFollow.ClearSources();
            activationAction.ClearSources();
            selectOnActivatedAction.ClearSources();
            selectOnDeactivatedAction.ClearSources();
        }

        protected virtual void OnEnable()
        {
            Setup();
        }

        protected virtual void OnDisable()
        {
            Clear();
        }

        /// <summary>
        /// Determines if the setup parameters are invalid.
        /// </summary>
        /// <returns><see langword="true"/> if the parameters are invalid.</returns>
        protected virtual bool InvalidParameters()
        {
            return (objectFollow == null || caster == null || facade == null);
        }
    }
}