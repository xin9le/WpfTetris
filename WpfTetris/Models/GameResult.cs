using System;
using System.Reactive.Linq;
using Reactive.Bindings;



namespace WpfTetris.Models
{
    /// <summary>
    /// ゲーム結果を表します。
    /// </summary>
    public class GameResult
    {
        #region プロパティ
        /// <summary>
        /// 消した合計行数を取得します。
        /// </summary>
        public IReadOnlyReactiveProperty<int> TotalRowCount { get; }


        /// <summary>
        /// 1 行で消した回数を取得します。
        /// </summary>
        public IReadOnlyReactiveProperty<int> RowCount1 => this.rowCount1;
        private readonly ReactiveProperty<int> rowCount1 = new ReactiveProperty<int>();


        /// <summary>
        /// 2 行で消した回数を取得します。
        /// </summary>
        public IReadOnlyReactiveProperty<int> RowCount2 => this.rowCount2;
        private readonly ReactiveProperty<int> rowCount2 = new ReactiveProperty<int>();


        /// <summary>
        /// 3 行で消した回数を取得します。
        /// </summary>
        public IReadOnlyReactiveProperty<int> RowCount3 => this.rowCount3;
        private readonly ReactiveProperty<int> rowCount3 = new ReactiveProperty<int>();


        /// <summary>
        /// 4 行で消した回数を取得します。
        /// </summary>
        public IReadOnlyReactiveProperty<int> RowCount4 => this.rowCount4;
        private readonly ReactiveProperty<int> rowCount4 = new ReactiveProperty<int>();
        #endregion


        #region コンストラクタ
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public GameResult()
        {
            this.TotalRowCount
                = this.RowCount1.CombineLatest
                (
                    this.RowCount2,
                    this.RowCount3,
                    this.RowCount4,
                    (x1, x2, x3, x4) => x1 * 1
                                    +   x2 * 2
                                    +   x3 * 3
                                    +   x4 * 4
                )
                .ToReadOnlyReactiveProperty();
        }
        #endregion


        #region メソッド
        /// <summary>
        /// 消した行数を追加します。
        /// </summary>
        /// <param name="count">行数</param>
        public void AddRowCount(int count)
        {
            switch (count)
            {
                case 1:  this.rowCount1.Value++;  break;
                case 2:  this.rowCount2.Value++;  break;
                case 3:  this.rowCount3.Value++;  break;
                case 4:  this.rowCount4.Value++;  break;
                default: throw new ArgumentOutOfRangeException(nameof(count));
            }
        }


        /// <summary>
        /// 結果をクリアします。
        /// </summary>
        public void Clear()
        {
            this.rowCount1.Value = 0;
            this.rowCount2.Value = 0;
            this.rowCount3.Value = 0;
            this.rowCount4.Value = 0;
        }
        #endregion
    }
}