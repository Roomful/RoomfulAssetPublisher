using System;
using net.roomful.api.props;
using UnityEngine;

namespace RF.Room.Teleportation
{
    public class TeleportationPoint
    {
        public Vector3 Position { get; protected set; }
        public Vector3 TargetPosition { get; protected set; }
        public string Text { get; protected set; }
        public bool TeleportByClick { get; protected set; }
        public Action OnClick { get; protected set; }

        protected void Setup(Vector3 position, Vector3 targetPosition, string text, bool teleportByClick, Action onClick) {
            Position = position;
            TargetPosition = targetPosition;
            Text = text;
            TeleportByClick = teleportByClick;
            OnClick = onClick;
        }
    }

    public interface ITeleportationService
    {
        /// <summary>
        /// Set state of a teleportation logic. Circle follows pointer, double click teleports to hit position.
        /// By default active when default Camera behaviour is active.
        /// </summary>
        /// <param name="state">True is active, false - opposite.</param>
        void SetTeleportationLogicState(bool state);


        void AddTeleportationPoint(TeleportationPoint point);
        void RemoveTeleportationPoint(TeleportationPoint point);
    }
}

