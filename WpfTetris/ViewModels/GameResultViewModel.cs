using Reactive.Bindings;
using WpfTetris.Models;



namespace WpfTetris.ViewModels
{
    /// <summary>
    /// ゲーム結果を表します。
    /// </summary>
    public class GameResultViewModel
    {
        #region プロパティ
        /// <summary>
        /// ゲーム結果を取得します。
        /// </summary>
        private GameResult Result { get; }


        /// <summary>
        /// 消した合計行数を取得します。
        /// </summary>
        public IReadOnlyReactiveProperty<int> TotalRowCount => this.Result.TotalRowCount;


        /// <summary>
        /// 1 行で消した回数を取得します。
        /// </summary>
        public IReadOnlyReactiveProperty<int> RowCount1 => this.Result.RowCount1;


        /// <summary>
        /// 2 行で消した回数を取得します。
        /// </summary>
        public IReadOnlyReactiveProperty<int> RowCount2 => this.Result.RowCount2;


        /// <summary>
        /// 3 行で消した回数を取得します。
        /// </summary>
        public IReadOnlyReactiveProperty<int> RowCount3 => this.Result.RowCount3;


        /// <summary>
        /// 4 行で消した回数を取得します。
        /// </summary>
        public IReadOnlyReactiveProperty<int> RowCount4 => this.Result.RowCount4;
        #endregion


        #region コンストラクタ
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="result">ゲーム結果</param>
        public GameResultViewModel(GameResult result)
        {
            this.Result = result;
        }
        #endregion
    }
}