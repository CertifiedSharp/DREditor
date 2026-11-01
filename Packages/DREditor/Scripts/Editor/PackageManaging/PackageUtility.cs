//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor.
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager.Requests;

using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace DREditor.Editor.PackageManaging
{
    /// <summary>
    ///
    /// </summary>
    public static class PackageUtility
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public static List<PackageInfo> GetMatchingPackagesFrom(Request<PackageInfo[]> request, string match) => 
            request.Result.Where(package => PackageContainsSearchTerm(package, match)).ToList();

        /// <summary>
        ///
        /// </summary>
        /// <param name="package"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public static bool PackageContainsSearchTerm(PackageInfo package, string match) =>
            package.name.ToLower().Contains(match.ToLower());
    }
}