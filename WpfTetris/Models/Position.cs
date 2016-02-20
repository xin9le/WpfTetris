using This = WpfTetris.Models.Position;



namespace WpfTetris.Models
{
    /// <summary>
    /// 座標を表します。
    /// </summary>
    public struct Position
    {
        #region プロパティ
        /// <summary>
        /// 行を取得します。
        /// </summary>
        public int Row { get; }


        /// <summary>
        /// 列を取得します。
        /// </summary>
        public int Column { get; }
        #endregion


        #region コンストラクタ
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="column">列</param>
        public Position(int row, int column)
        {
            this.Row = row;
            this.Column = column;
        }
        #endregion


        #region オーバーライド
        /// <summary>
        /// 指定されたインスタンスと等価かどうかを判定します。
        /// </summary>
        /// <param name="obj">比較対象インスタンス</param>
        /// <returns>等価な場合true</returns>
        public override bool Equals(object obj)
            => this == (This)obj;


        /// <summary>
        /// ハッシュ値を取得します。
        /// </summary>
        /// <returns>ハッシュ値</returns>
        public override int GetHashCode()
            => this.Row.GetHashCode() ^ this.Column.GetHashCode();


        /// <summary>
        /// インスタンスを文字列かします。
        /// </summary>
        /// <returns>文字列</returns>
        public override string ToString()
            => $"({this.Row}, {this.Column})";
        #endregion


        #region オペレーターオーバーロード
        /// <summary>
        /// 等号演算子を定義します。
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>等価であればtrue</returns>
        public static bool operator ==(This left, This right)
            => left.Row == right.Row
            && left.Column == right.Column;


        /// <summary>
        /// 等号でない演算子を定義します。
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>等価であればtrue</returns>
        public static bool operator !=(This left, This right)
            => !(left == right);
        #endregion
    }
}