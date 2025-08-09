using UnityEngine;

namespace T_Library
{
    public static class Utility
    {
        #region bool系

        /// <summary>
        /// ランダムにtrue/falseを返す
        /// </summary>
        public static bool RandomBool()
        {
            return Random.Range(0, 2) == 0;
        }

        #endregion
    }
}