using System;
using System.Reactive.Linq;
using System.Timers;



namespace WpfTetris.Extensions
{
    /// <summary>
    /// System.Timers.Timer の拡張機能を提供します。
    /// </summary>
    public static class TimerExtensions
    {
        /// <summary>
        /// Elapsed イベントを IObservable&lt;T&gt; として返します。
        /// </summary>
        /// <param name="self">タイマー</param>
        /// <returns>イベントシーケンス</returns>
        public static IObservable<ElapsedEventArgs> ElapsedAsObservable(this Timer self)
        {
            if (self == null)
                throw new ArgumentNullException(nameof(self));

            return Observable.FromEvent<ElapsedEventHandler, ElapsedEventArgs>
            (
                h => (s, e) => h(e),
                h => self.Elapsed += h,
                h => self.Elapsed -= h
            );
        }
    }
}