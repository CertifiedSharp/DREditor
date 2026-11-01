//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Gates;
using DREditor.Progressing;
using UnityEngine;

namespace DREditor.DebugTesting
{
    /// <summary>
    ///
    /// </summary>
    public class DebugTesterBase : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [HideInInspector]
        public int state = 0;

        /// <summary>
        ///
        /// </summary>
        [HideInInspector]
        public int chapterIndex = 0;

        /// <summary>
        ///
        /// </summary>
        [HideInInspector]
        public Objective objective = null;

        /// <summary>
        ///
        /// </summary>
        [HideInInspector]
        public int objectiveIndex = 0;

        /// <summary>
        ///
        /// </summary>
        [HideInInspector]
        public string GoToScene = "";

        /// <summary>
        ///
        /// </summary>
        [HideInInspector]
        public Gate toGate = null;

        /// <summary>
        ///
        /// </summary>
        [HideInInspector]
        public int toGateIdx = 0;

        /// <summary>
        ///
        /// </summary>
        [HideInInspector]
        public GateFrom fromGate = null;

        /// <summary>
        ///
        /// </summary>
        [HideInInspector]
        public int fromGateIdx = 0;

        /// <summary>
        ///
        /// </summary>
        [HideInInspector]
        public int invGateIdx = -1;

        /// <summary>
        ///
        /// </summary>
        [HideInInspector]
        public Vector3 position = Vector3.zero;

        /// <summary>
        ///
        /// </summary>
        [HideInInspector]
        public Vector3 rotation = Vector3.zero;


        /// <summary>
        ///
        /// </summary>
        /// <param name="state"></param>
        public virtual void SetState(int state) => this.state = state;

        /// <summary>
        ///
        /// </summary>
        /// <param name="chapterIndex"></param>
        public virtual void SetChapterIndex(int chapterIndex) => this.chapterIndex = chapterIndex;

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        public virtual void SetObjective(Objective value) => objective = value;

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        public virtual void SetObjectiveIndex(int value) => objectiveIndex = value;

        /// <summary>
        ///
        /// </summary>
        /// <param name="toGateIdx"></param>
        /// <param name="toGate"></param>
        public virtual void SetToGate(int toGateIdx, Gate toGate)
        {
            this.toGateIdx = toGateIdx;

            this.toGate = toGate;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fromGateIdx"></param>
        /// <param name="fromGate"></param>
        public virtual void SetFromGate(int fromGateIdx, GateFrom fromGate)
        {
            this.fromGateIdx = fromGateIdx;

            this.fromGate = fromGate;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="GoToScene"></param>
        /// <param name="invGateIdx"></param>
        public virtual void SetInvestigationScene(string GoToScene, int invGateIdx)
        {
            this.GoToScene = GoToScene;

            this.invGateIdx = invGateIdx;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        public virtual void SetPositionAndRotation(Vector3 position, Vector3 rotation)
        {
            this.position = position;

            this.rotation = rotation;
        }
    }
}