using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using WpfTetris.Extensions;
using WpfTetris.Models;
using WpfTetris.ViewModels;
using This = WpfTetris.Views.MainWindow;



namespace WpfTetris.Views
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        #region プロパティ
        /// <summary>
        /// ゲームを取得または設定します。
        /// </summary>
        private GameViewModel Game
        {
            get { return this.DataContext as GameViewModel; }
            set { this.DataContext = value; }
        }
        #endregion


        #region コンストラクタ
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public MainWindow()
        {
            this.Game = new GameViewModel();
            this.InitializeComponent();
            This.SetupField(this.field, this.Game.Field.Cells, 30);
            This.SetupField(this.nextField, this.Game.NextField.Cells, 18);
            this.AttachEvents();
            this.Game.Play();
        }
        #endregion


        #region 初期化
        /// <summary>
        /// フィールド準備を行います。
        /// </summary>
        /// <param name="field">フィールドとなる Grid</param>
        /// <param name="cells">セルの描画用モデル</param>
        /// <param name="blockSize">ブロックサイズ</param>
        private static void SetupField(Grid field, CellViewModel[,] cells, byte blockSize)
        {
            //--- 行/列の定義
            for (int r = 0; r < cells.GetLength(0); r++)
                field.RowDefinitions.Add(new RowDefinition { Height = new GridLength(blockSize, GridUnitType.Pixel) });

            for (int c = 0; c < cells.GetLength(1); c++)
                field.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(blockSize, GridUnitType.Pixel) });

            //--- セル設定
            foreach (var item in cells.WithIndex())
            {
                //--- 背景色をバインド
                var brush = new SolidColorBrush();
                var control = new TextBlock
                {
                    DataContext = item.Element,
                    Background = brush,
                    Margin = new Thickness(1),
                };
                BindingOperations.SetBinding(brush, SolidColorBrush.ColorProperty, new Binding("Color.Value"));

                //--- 位置を決めて子要素として追加
                Grid.SetRow(control, item.X);
                Grid.SetColumn(control, item.Y);
                field.Children.Add(control);
            }
        }


        /// <summary>
        /// イベントの関連付けを行います。
        /// </summary>
        private void AttachEvents()
        {
            this.KeyDown += (s, e) =>
            {
                switch (e.Key)
                {
                    case Key.Z:     this.Game.Field.RotationTetrimino(RotationDirection.Left);   break;
                    case Key.X:     this.Game.Field.RotationTetrimino(RotationDirection.Right);  break;
                    case Key.Up:    this.Game.Field.RotationTetrimino(RotationDirection.Right);  break;
                    case Key.Right: this.Game.Field.MoveTetrimino(MoveDirection.Right);  break;
                    case Key.Down:  this.Game.Field.MoveTetrimino(MoveDirection.Down);   break;
                    case Key.Left:  this.Game.Field.MoveTetrimino(MoveDirection.Left);   break;
                    case Key.Space: this.Game.Play();  break;
                }
            };
        }
        #endregion
    }
}