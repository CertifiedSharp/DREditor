//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Minigames.Debates.Cycling
{
    /// <summary>
    ///
    /// </summary>
    public class TrackPathCalculator
    {
        /// <summary>
        ///
        /// </summary>
        protected readonly List<CyclePosition> positions;

        /// <summary>
        ///
        /// </summary>
        protected int firstVisible;

        /// <summary>
        ///
        /// </summary>
        protected int notVisible;


        public TrackPathCalculator(List<CyclePosition> positions, int firstVisible, int notVisible)
        {
            this.positions = positions;

            this.firstVisible = firstVisible;

            this.notVisible = notVisible;
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="bulletCount"></param>
        /// <param name="maxVisible"></param>
        public virtual void InitializeVisibility(int bulletCount, int maxVisible)
        {
            int diff = positions.Count - bulletCount - 1;

            notVisible += diff;

            if (notVisible + firstVisible > maxVisible)
                firstVisible--;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentPos"></param>
        /// <returns></returns>
        public virtual bool InVisibleRange(CyclePosition currentPos)
        {
            int positionIndex = positions.IndexOf(currentPos);

            return positionIndex > (firstVisible - 1) &&
                   positionIndex <= (firstVisible - 1) + notVisible;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentPos"></param>
        /// <param name="forward"></param>
        /// <param name="trackCount"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual (Vector3[] path, CyclePosition nextPosition) CalculateCyclePath(CyclePosition currentPos, bool forward, int trackCount)
        {
            int currentPosIndex = positions.IndexOf(currentPos);

            int evalOffset = forward ? -1 : notVisible;

            if (trackCount < positions.Count && currentPosIndex == firstVisible + evalOffset)
                return forward ? GetEndMoveForwardPath(currentPosIndex, currentPos)
                               : GetEndMoveBackwardPath(currentPosIndex);
            else
                return GetLatterMovePath(currentPos, forward);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentPosIndex"></param>
        /// <param name="currentPos"></param>
        /// <returns></returns>
        protected virtual (Vector3[], CyclePosition) GetEndMoveForwardPath(int currentPosIndex, CyclePosition currentPos)
        {
            List<Vector3> paths = new(currentPos.GetSubPositions());

            int targetIndex = firstVisible + notVisible;

            for (int i = currentPosIndex + 1; i != targetIndex; i++)
            {
                paths.Add(positions[i].transform.position);

                paths.AddRange(positions[i].GetSubPositions());
            }

            int finalIndex = targetIndex < positions.Count ? targetIndex : positions.Count - targetIndex;

            paths.Add(positions[finalIndex].transform.position);

            return (paths.ToArray(), positions[finalIndex]);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentPosIndex"></param>
        /// <returns></returns>
        protected virtual (Vector3[], CyclePosition) GetEndMoveBackwardPath(int currentPosIndex)
        {
            List<Vector3> paths = new();

            int targetIndex = firstVisible - 1;

            for (int i = currentPosIndex; i != targetIndex; i--)
                UpdatePath(paths, i);

            UpdatePath(paths, targetIndex);

            return (paths.ToArray(), positions[targetIndex]);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="paths"></param>
        /// <param name="targetIndex"></param>
        protected virtual void UpdatePath(List<Vector3> paths, int targetIndex)
        {
            var targetSub = positions[targetIndex].GetSubPositions();

            targetSub.Reverse();

            paths.AddRange(targetSub);

            paths.Add(positions[targetIndex].transform.position);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentPos"></param>
        /// <param name="forward"></param>
        /// <returns></returns>
        protected virtual (Vector3[], CyclePosition) GetLatterMovePath(CyclePosition currentPos, bool forward)
        {
            int i = GetCalcIndex(currentPos, forward);

            CyclePosition sourcePosition = forward ? currentPos : positions[i];

            List<Vector3> paths = new(sourcePosition.GetSubPositions())
            {
                positions[i].transform.position
            };

            return (paths.ToArray(), positions[i]);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentPos"></param>
        /// <param name="forward"></param>
        /// <returns></returns>
        protected virtual int GetCalcIndex(CyclePosition currentPos, bool forward)
        {
            int index = positions.IndexOf(currentPos);

            if (forward)
                return (index + 1) % positions.Count;

            return index - 1 < 0 ? positions.Count - 1 : index - 1;
        }
    }
}