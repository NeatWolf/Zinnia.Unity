﻿namespace Zinnia.Data.Operation.Extraction
{
    using Malimbe.BehaviourStateRequirementMethod;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Process;

    /// <summary>
    /// Provides the basis for emitting the <see cref="TResultElement"/> that any concrete implementation is residing on.
    /// </summary>
    /// <typeparam name="TResultElement">The element type for the result.</typeparam>
    /// <typeparam name="TSourceElement">The element type for the source.</typeparam>
    /// <typeparam name="TEvent">The event to invoke.</typeparam>
    /// <typeparam name="TEventElement">The event element type that is invoked.</typeparam>
    public abstract class ValueExtractor<TResultElement, TSourceElement, TEvent, TEventElement> : MonoBehaviour, IProcessable where TEvent : UnityEvent<TEventElement>, new()
    {
        /// <summary>
        /// The source to extract from.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public TSourceElement Source { get; set; }

        /// <summary>
        /// The extracted <see cref="TResultElement"/>.
        /// </summary>
        public TResultElement Result { get; protected set; }

        /// <summary>
        /// Emitted when the <see cref="TResultElement"/> is extracted.
        /// </summary>
        [DocumentedByXml]
        public TEvent Extracted = new TEvent();
        /// <summary>
        /// Emitted when the data can't be extracted.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent Failed = new UnityEvent();

        /// <summary>
        /// Extracts each time the process is run in a moment.
        /// </summary>
        public virtual void Process()
        {
            DoExtract();
        }

        /// <summary>
        /// Extracts the <see cref="TResultElement"/>/
        /// </summary>
        /// <returns>The extracted <see cref="TResultElement"/>.</returns>
        [RequiresBehaviourState]
        public virtual TResultElement Extract()
        {
            Result = ExtractValue();
            if (!InvokeResult(Result))
            {
                Failed?.Invoke();
            }

            return Result;
        }

        /// <summary>
        /// Extracts the <see cref="TResultElement"/>.
        /// </summary>
        public virtual void DoExtract()
        {
            Extract();
        }

        /// <summary>
        /// Extracts the <see cref="TResultElement"/> from the given <see cref="TSourceElement"/>.
        /// </summary>
        /// <param name="data">The data to extract from.</param>
        /// <returns>The extracted data.</returns>
        [RequiresBehaviourState]
        public virtual TResultElement Extract(TSourceElement data)
        {
            Source = data;
            return Extract();
        }

        /// <summary>
        /// Extracts the <see cref="TResultElement"/> from the given <see cref="TSourceElement"/>.
        /// </summary>
        /// <param name="data">The data to extract from.</param>
        public virtual void DoExtract(TSourceElement data)
        {
            Extract(data);
        }

        /// <summary>
        /// The mechanism for extracting the property value.
        /// </summary>
        /// <returns>The extracted value.</returns>
        protected abstract TResultElement ExtractValue();
        /// <summary>
        /// Invokes the result via the <see cref="Extracted"/> event.
        /// </summary>
        /// <param name="data">The data to emit with the event.</param>
        /// <returns>Whether the results were invoked.</returns>
        protected abstract bool InvokeResult(TResultElement data);

        protected virtual void OnDisable()
        {
            Result = default;
        }
    }
}