using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Media;
using Reactive.Bindings;
using WpfTetris.Extensions;
using WpfTetris.Models;



namespace WpfTetris.ViewModels
{
    /// <summary>
    /// フィールド描画用のモデルを提供します。
    /// </summary>
    public class FieldViewModel
    {
        #region プロパティ
        /// <summary>
        /// テトリスの場を取得します。
        /// </summary>
        private Field Field { get; }


        /// <summary>
        /// セルのコレクションを取得します。
        /// </summary>
        public CellViewModel[,] Cells { get; }


        /// <summary>
        /// アクティブ状態かどうかを取得します。
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsActivated => this.Field.IsActivated;


        /// <summary>
        /// 背景色を取得します。
        /// </summary>
        private Color BackgroundColor => Colors.WhiteSmoke;
        #endregion


        #region コンストラクタ
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public FieldViewModel(Field field)
        {
            this.Field = field;

            //--- 描画するセルを準備
            this.Cells = new CellViewModel[Field.RowCount, Field.ColumnCount];
            foreach (var item in this.Cells.WithIndex())
                this.Cells[item.X, item.Y] = new CellViewModel();

            //--- ブロックに関する変更を処理
            this.Field.Tetrimino
                .CombineLatest
                (
                    this.Field.PlacedBlocks,
                    (t, p) => (t == null ? p : p.Concat(t.Blocks))
                            .ToDictionary2(x => x.Position.Row, x => x.Position.Column)
                )
                .Subscribe(x =>
                {
                    foreach (var item in this.Cells.WithIndex())
                    {
                        var color = x.GetValueOrDefault(item.X)
                                    ?.GetValueOrDefault(item.Y)
                                    ?.Color
                                    ?? this.BackgroundColor;
                        item.Element.Color.Value = color;
                    }
                });
        }
        #endregion


        #region 操作
        /// <summary>
        /// 指定された方向にテトリミノを移動させます。
        /// </summary>
        /// <param name="direction">移動方向</param>
        public void MoveTetrimino(MoveDirection direction) => this.Field.MoveTetrimino(direction);


        /// <summary>
        /// 指定された方向にテトリミノを回転させます。
        /// </summary>
        /// <param name="direction">回転方向</param>
        public void RotationTetrimino(RotationDirection direction) => this.Field.RotationTetrimino(direction);


        /// <summary>
        /// テトリミノを強制的に確定させます。
        /// </summary>
        public void ForceFixTetrimino() => this.Field.ForceFixTetrimino();
        #endregion
    }
}