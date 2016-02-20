namespace WpfTetris.Models
{
    /// <summary>
    /// 方向を表します。
    /// </summary>
    public enum Direction
    {
        /// <summary>
        /// 上向き
        /// </summary>
        Up = 0,

        /// <summary>
        /// 右向き
        /// </summary>
        Right,

        /// <summary>
        /// 下向き
        /// </summary>
        Down,

        /// <summary>
        /// 左向き
        /// </summary>
        Left,
    }



    /// <summary>
    /// 回転方向を表します。
    /// </summary>
    public enum RotationDirection
    {
        /// <summary>
        /// 右回転
        /// </summary>
        Right = 0,

        /// <summary>
        /// 左回転
        /// </summary>
        Left,
    }



    /// <summary>
    /// 移動方向を表します。
    /// </summary>
    public enum MoveDirection
    {
        /// <summary>
        /// 右方向
        /// </summary>
        Right = 0,

        /// <summary>
        /// 下方向
        /// </summary>
        Down,

        /// <summary>
        /// 左方向
        /// </summary>
        Left,
    }
}