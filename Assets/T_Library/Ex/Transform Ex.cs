using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

//=============================
#region コピペ用
//=============================

#endregion

namespace T_Library.Ex
{

    public static class TransformEx
    {
        //=============================
        #region Position操作系（ワールド）
        //=============================

        /// <summary>
        /// [ワールド座標のXのみ変更]
        /// </summary>
        public static void SetPositionX(this Transform posA, float x)
        {
            Vector3 pos = posA.position;
            pos.x = x;
            posA.position = pos;
        }

        /// <summary>
        /// [ワールド座標のYのみ変更]
        /// </summary>
        public static void SetPositionY(this Transform PosA, float y)
        {
            Vector3 pos = PosA.position;
            pos.y = y;
            PosA.position = pos;
        }

        /// <summary>
        /// [ワールド座標のZのみ変更]
        /// </summary>
        public static void SetPositionZ(this Transform PosA, float z)
        {
            Vector3 pos = PosA.position;
            pos.z = z;
            PosA.position = pos;
        }

        #endregion

        //=============================
        #region Position操作系（ローカル）
        //=============================

        /// <summary>
        /// [ローカル座標のXのみ変更]
        /// </summary>
        public static void SetLocalPositionX(this Transform PosA, float x)
        {
            Vector3 pos = PosA.localPosition;
            pos.x = x;
            PosA.localPosition = pos;
        }

        /// <summary>
        /// [ローカル座標のYのみ変更]
        /// </summary>
        public static void SetLocalPositionY(this Transform PosA, float y)
        {
            Vector3 pos = PosA.localPosition;
            pos.y = y;
            PosA.localPosition = pos;
        }

        /// <summary>
        /// [ローカル座標のZのみ変更]
        /// </summary>
        public static void SetLocalPositionZ(this Transform PosA, float z)
        {
            Vector3 pos = PosA.localPosition;
            pos.z = z;
            PosA.localPosition = pos;
        }

        #endregion

        //=============================
        #region リセット・変換系
        //=============================

        /// <summary>
        /// [position, rotationをすべてリセット]
        /// </summary>
        public static void ResetTransform(this Transform PosA)
        {
            PosA.position = Vector3.zero;
            PosA.rotation = Quaternion.identity;
        }

        /// <summary>
        /// [ワールド座標を直接指定して位置を設定]
        /// </summary>
        public static void SetWorldPosition(this Transform PosA, Vector3 pos)
        {
            PosA.position = pos;
        }

        #endregion

        //=============================
        #region DoTweenアニメーション系
        //=============================

        /// <summary>
        /// [現在の位置から指定した分だけ移動]
        /// </summary>
        public static async UniTask DOMoveOffset(this Transform PosA, Vector3 offset, float duration = 1f)
        {
            await PosA.DOMove(PosA.position + offset, duration).ToUniTask();
        }

        /// <summary>
        /// [震える演出]
        /// </summary>
        public static async UniTask DOPunch(this Transform PosA, Vector3 power, float duration = 1f)
        {
            await PosA.DOPunchPosition(power, duration).ToUniTask();
        }

        /// <summary>
        /// [指定位置まで移動して元の位置に戻るアニメーション]
        /// </summary>
        public static async UniTask DOPingPong(this Transform PosA, Vector3 target, float duration = 1f)
        {
            Vector3 original = PosA.position;
            await PosA.DOMove(target, duration).SetEase(Ease.Linear).ToUniTask();
            await PosA.DOMove(original, duration).SetEase(Ease.Linear).ToUniTask();
        }

        /// <summary>
        /// [他のTransformと position / rotation を交換するアニメーション]
        /// </summary>
        public static UniTask DOSwapTransform(this Transform PosA, Transform other, float duration = 1f)
        {
            Vector3 pos = PosA.position;
            Quaternion rot = PosA.rotation;

            var t1 = PosA.DOMove(other.position, duration).ToUniTask();
            var t2 = PosA.DORotateQuaternion(other.rotation, duration).ToUniTask();

            var t3 = other.DOMove(pos, duration).ToUniTask();
            var t4 = other.DORotateQuaternion(rot, duration).ToUniTask();

            return UniTask.WhenAll(t1, t2, t3, t4);
        }

        #endregion

        //=============================
        #region Transform相互操作系
        //=============================

        /// <summary>
        /// [他のTransformと position / rotation を交換]
        /// </summary>
        public static void SwapTransform(this Transform PosA, Transform other)
        {
            Vector3 pos = PosA.position;
            Quaternion rot = PosA.rotation;
            Vector3 scl = PosA.localScale;

            PosA.position = other.position;
            PosA.rotation = other.rotation;
            PosA.localScale = other.localScale;

            other.position = pos;
            other.rotation = rot;
            other.localScale = scl;
        }

        /// <summary>
        /// [他のTransformの position / rotation をコピー]
        /// </summary>
        public static void CopyTransformFrom(this Transform PosA, Transform source)
        {
            PosA.position = source.position;
            PosA.rotation = source.rotation;
            PosA.localScale = source.localScale;
        }

        #endregion

        //=============================
        #region 距離・判定系
        //=============================

        /// <summary>
        /// [指定したTransformとの距離が一定以下かどうかを判定]
        /// </summary>
        public static bool IsCloseTo(this Transform PosA, Transform other, float threshold)
        {
            return Vector3.Distance(PosA.position, other.position) <= threshold;
        }

        /// <summary>
        /// [指定したTransformとの距離を取得]
        /// </summary>
        public static float DistanceTo(this Transform PosA, Transform other)
        {
            return Vector3.Distance(PosA.position, other.position);
        }

        /// <summary>
        /// [指定したレイヤーの一番近いTransformを取得する]
        /// </summary>
        public static float DistanceToLayer(this Transform a, Transform[] targets, int layerMask)
        {
            float minDistance = float.MaxValue;
            foreach (var t in targets)
            {
                if (((1 << t.gameObject.layer) & layerMask) != 0)
                {
                    float dist = Vector3.Distance(a.position, t.position);
                    if (dist < minDistance) minDistance = dist;
                }
            }
            return minDistance;
        }

        /// <summary>
        /// [指定Transformとの角度差を取得]
        /// </summary>
        public static float AngleTo(this Transform PosA, Transform other)
        {
            return Vector3.Angle(PosA.forward, other.position - PosA.position);
        }
        #endregion

        //=============================
        #region 親子・階層操作系
        //=============================

        /// <summary>
        /// [すべての子オブジェクトを破棄]
        /// </summary>
        public static void DestroyAllChildren(this Transform PosA)
        {
            foreach (Transform child in PosA)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        /// <summary>
        /// [すべての子オブジェクトを アクティブ/非アクティブ に切り替え]
        /// </summary>
        public static void SetAllChildrenActive(this Transform parent, bool active)
        {
            foreach (Transform child in parent)
            {
                child.gameObject.SetActive(active);
            }
        }

        /// <summary>
        /// [ワールド座標を維持したまま親Transformを設定]
        /// </summary>
        public static void SetParentKeepWorld(this Transform PosA, Transform parent)
        {
            PosA.SetParent(parent, true);
        }

        /// <summary>
        /// [指定された名前の子オブジェクトを検索して返す]
        /// </summary>
        public static Transform FindChildByName(this Transform PosA, string name)
        {
            foreach (Transform child in PosA)
            {
                if (child.name == name)
                    return child;
            }
            return null;
        }

        #endregion

    }
}