using System;
using System.Collections.Generic;
using System.Linq;



namespace WpfTetris.Extensions
{
    /// <summary>
    /// IEnumerable&lt;T&gt; の拡張機能を提供します。
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// コレクション要素にインデックスを追加します。
        /// </summary>
        /// <typeparam name="T">コレクション要素の型</typeparam>
        /// <param name="self">コレクション</param>
        /// <returns>インデックスが追加されたコレクション</returns>
        public static IEnumerable<IndexedItem<T>> WithIndex<T>(this IEnumerable<T> self)
        {
            if (self == null)
                throw new ArgumentNullException(nameof(self));
            return self.Select((x, i) => new IndexedItem<T>(i, x));
        }


        /// <summary>
        /// <para>コレクション要素にインデックスを追加します。</para>
        /// <para>インデックスは指定された条件を満たしたときにインクリメントされます。</para>
        /// </summary>
        /// <typeparam name="T">コレクション要素の型</typeparam>
        /// <param name="self">コレクション</param>
        /// <param name="predicate">インクリメント条件</param>
        /// <returns>インデックスが追加されたコレクション</returns>
        public static IEnumerable<IndexedItem<T>> WithIndex<T>(this IEnumerable<T> self, Func<T, bool> predicate)
        {
            if (self == null)       throw new ArgumentNullException(nameof(self));
            if (predicate == null)  throw new ArgumentNullException(nameof(predicate));

            int i = 0;
            foreach (var x in self)
            {
                if (predicate(x))
                    i++;
                yield return new IndexedItem<T>(i, x);
            }
        }


        /// <summary>
        /// コレクション要素にインデックスを追加します。
        /// </summary>
        /// <typeparam name="T">コレクション要素の型</typeparam>
        /// <param name="self">コレクション</param>
        /// <returns>インデックスが追加されたコレクション</returns>
        public static IEnumerable<IndexedItem2<T>> WithIndex<T>(this T[,] self)
        {
            if (self == null)
                throw new ArgumentNullException(nameof(self));

            for (int x = 0; x < self.GetLength(0); x++)
            for (int y = 0; y < self.GetLength(1); y++)
                yield return new IndexedItem2<T>(x, y, self[x, y]);
        }


        /// <summary>
        /// 2 次元の辞書を生成します。
        /// </summary>
        /// <typeparam name="TSource">コレクション要素の型</typeparam>
        /// <typeparam name="TKeyX">X 軸方向のキー</typeparam>
        /// <typeparam name="TKeyY">Y 軸方向のキー</typeparam>
        /// <param name="self">コレクション</param>
        /// <param name="xSelector">X 軸方向のキー選択</param>
        /// <param name="ySelector">Y 軸方向のキー選択</param>
        /// <returns>2 次元の辞書</returns>
        public static Dictionary<TKeyX, Dictionary<TKeyY, TSource>> ToDictionary2<TSource, TKeyX, TKeyY>
            (
                this IEnumerable<TSource> self,
                Func<TSource, TKeyX> xSelector,
                Func<TSource, TKeyY> ySelector
            )
        {
            if (self == null)       throw new ArgumentNullException(nameof(self));
            if (xSelector == null)  throw new ArgumentNullException(nameof(xSelector));
            if (ySelector == null)  throw new ArgumentNullException(nameof(ySelector));
        
            return  self.GroupBy(xSelector)
                    .ToDictionary(x => x.Key, xs => xs.ToDictionary(ySelector));
        }


        /// <summary>
        /// 2 次元の辞書を生成します。
        /// </summary>
        /// <typeparam name="TSource">コレクション要素の型</typeparam>
        /// <typeparam name="TKeyX">X 軸方向のキー</typeparam>
        /// <typeparam name="TKeyY">Y 軸方向のキー</typeparam>
        /// <typeparam name="TElement">結果要素の型</typeparam>
        /// <param name="self">コレクション</param>
        /// <param name="xSelector">X 軸方向のキー選択</param>
        /// <param name="ySelector">Y 軸方向のキー選択</param>
        /// <param name="elementSelector">要素選択</param>
        /// <returns>2 次元の辞書</returns>
        public static Dictionary<TKeyX, Dictionary<TKeyY, TElement>> ToDictionary2<TSource, TKeyX, TKeyY, TElement>
            (
                this IEnumerable<TSource> self,
                Func<TSource, TKeyX> xSelector,
                Func<TSource, TKeyY> ySelector,
                Func<TSource, TElement> elementSelector
            )
        {
            if (self == null)             throw new ArgumentNullException(nameof(self));
            if (xSelector == null)        throw new ArgumentNullException(nameof(xSelector));
            if (ySelector == null)        throw new ArgumentNullException(nameof(ySelector));
            if (elementSelector == null)  throw new ArgumentNullException(nameof(elementSelector));
        
            return  self.GroupBy(xSelector)
                    .ToDictionary(x => x.Key, xs => xs.ToDictionary(ySelector, elementSelector));
        }
    }
}