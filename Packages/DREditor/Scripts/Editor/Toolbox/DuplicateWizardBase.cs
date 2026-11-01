//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Editor.Utility;
using System.IO;
using UnityEngine;

namespace DREditor.Editor.Toolbox
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DuplicateWizardBase : GenerateWizardBase
    {
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string assetSuffix = "";

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected AffixSeparator affixSeparator = AffixSeparator.Underscore;


        protected DuplicateWizardBase() : base(false) { }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToDuplicate"></param>
        /// <param name="duplicatedObject"></param>
        /// <returns></returns>
        protected virtual bool TryDuplicateAsset<T>(T objectToDuplicate, out T duplicatedObject) where T : UnityEngine.Object =>
            CustomEditorUtility.TryDuplicateAsset(objectToDuplicate, BuildDuplicatedPath, out duplicatedObject);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectToDuplicateName"></param>
        /// <param name="objectToDuplicatePath"></param>
        /// <returns></returns>
        protected virtual string BuildDuplicatedPath(string objectToDuplicateName, string objectToDuplicatePath) =>
            targetDirectory + "/" + BuildDuplicatedAssetName(objectToDuplicateName, objectToDuplicatePath);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectToDuplicateName"></param>
        /// <param name="objectToDuplicatePath"></param>
        /// <returns></returns>
        protected virtual string BuildDuplicatedAssetName(string objectToDuplicateName, string objectToDuplicatePath) =>
            GetPreffix() + objectToDuplicateName + GetSuffix() + Path.GetExtension(objectToDuplicatePath);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual string GetPreffix() => assetPrefix == "" ? "" : assetPrefix + SeparateAffix();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual string GetSuffix() => assetSuffix == "" ? "" : SeparateAffix() + assetSuffix;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual string SeparateAffix() => affixSeparator switch
        {
            AffixSeparator.Space => " ",
            AffixSeparator.Underscore => "_",
            AffixSeparator.None => "",
            _ => "",
        };
    }

    /// <summary>
    /// 
    /// </summary>
    public enum AffixSeparator
    {
        Space,
        Underscore,
        None
    }
}