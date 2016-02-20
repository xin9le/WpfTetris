using System;
using System.Security.Cryptography;
using System.Threading;
using This = WpfTetris.Utilities.RandomProvider;



namespace WpfTetris.Utilities
{
    /// <summary>
    /// 乱数生成機能を提供します。
    /// </summary>
    public static class RandomProvider
    {
        #region プロパティ
        /// <summary>
        /// スレッド単位で独立している乱数生成機能のラッパーを取得します。
        /// </summary>
        private static ThreadLocal<Random> RandomWrapper { get; } = new ThreadLocal<Random>(() =>
        {
            //--- PCL で RNGCryptoServiceProvider が使えないので GUID で代用
            //var @byte = Guid.NewGuid().ToByteArray();
            //var seed = BitConverter.ToInt32(@byte, 0);
            //return new Random(seed);

            var @byte = new byte[sizeof(int)];
            using (var crypto = new RNGCryptoServiceProvider())
                crypto.GetBytes(@byte);
            var seed = BitConverter.ToInt32(@byte, 0);
            return new Random(seed);
        });


        /// <summary>
        /// スレッド単位で独立している乱数生成機能を取得します。
        /// </summary>
        public static Random ThreadRandom => This.RandomWrapper.Value;
        #endregion
    }
}