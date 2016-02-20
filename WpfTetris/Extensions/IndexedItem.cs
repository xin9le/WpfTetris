namespace WpfTetris.Extensions
{
    /// <summary>
    /// インデックス化された要素を表します。
    /// </summary>
    /// <typeparam name="T">要素の型</typeparam>
    public struct IndexedItem<T>
    {
        #region プロパティ
        /// <summary>
        /// インデックスを取得します。
        /// </summary>
        public int Index { get; }


        /// <summary>
        /// 要素を取得します。
        /// </summary>
        public T Element { get; }
        #endregion


        #region コンストラクタ
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="element">要素</param>
        internal IndexedItem(int index, T element)
        {
            this.Index = index;
            this.Element = element;
        }
        #endregion
    }



    /// <summary>
    /// 2 次元のインデックス化された要素を表します。
    /// </summary>
    /// <typeparam name="T">要素の型</typeparam>
    public struct IndexedItem2<T>
    {
        #region プロパティ
        /// <summary>
        /// X 軸方向インデックスを取得します。
        /// </summary>
        public int X { get; }


        /// <summary>
        /// Y 軸方向インデックスを取得します。
        /// </summary>
        public int Y { get; }


        /// <summary>
        /// 要素を取得します。
        /// </summary>
        public T Element { get; }
        #endregion


        #region コンストラクタ
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="x">X 軸方向のインデックス</param>
        /// <param name="y">Y 軸方向のインデックス</param>
        /// <param name="element">要素</param>
        internal IndexedItem2(int x, int y, T element)
        {
            this.X = x;
            this.Y = y;
            this.Element = element;
        }
        #endregion
    }
}