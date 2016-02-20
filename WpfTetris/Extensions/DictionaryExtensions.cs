using System;
using System.Collections.Generic;



namespace WpfTetris.Extensions
{
    /// <summary>
    /// System.Collection.Generic.Dictionary の拡張機能を提供します。
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// 値を取得します。値が存在しない場合は既定値を返します。
        /// </summary>
        /// <typeparam name="TKey">キーの型</typeparam>
        /// <typeparam name="TValue">値の型</typeparam>
        /// <param name="self">対象となる辞書</param>
        /// <param name="key">キー</param>
        /// <param name="defaultValue">既定値</param>
        /// <returns>値</returns>
        public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> self, TKey key, TValue defaultValue = default(TValue))
        {
            if (self == null)
                throw new ArgumentNullException(nameof(self));

            TValue result;
            return  self.TryGetValue(key, out result)
                ?   result
                :   defaultValue;
        }
    }
}