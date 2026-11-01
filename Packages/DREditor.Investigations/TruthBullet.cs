//Truth Bullet script assumed to be made Author: SeleniumSoul for DREditor.
//Rework Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using DREditor.Items;
using UnityEngine;

namespace DREditor.Investigations
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Investigations/Truth Bullet", fileName = "New Truth Bullet")]
    public class TruthBullet : AttributableInventoryItem<ITruthBulletData> { }
}