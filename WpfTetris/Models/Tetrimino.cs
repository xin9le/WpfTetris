using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using WpfTetris.Utilities;
using This = WpfTetris.Models.Tetrimino;



namespace WpfTetris.Models
{
    /// <summary>
    /// テトリミノとしての機能を提供します。
    /// </summary>
    public class Tetrimino
    {
        #region プロパティ
        /// <summary>
        /// 種類を取得します。
        /// </summary>
        public TetriminoKind Kind { get; }


        /// <summary>
        /// 色を取得します。
        /// </summary>
        public Color Color => this.Kind.BlockColor();


        /// <summary>
        /// 基準点 (= 左上座標) を取得します。
        /// </summary>
        public Position Position { get; private set; }


        /// <summary>
        /// 向きを取得します。
        /// </summary>
        public Direction Direction { get; private set; }


        /// <summary>
        /// ブロックを取得します。
        /// </summary>
        public IReadOnlyList<Block> Blocks { get; private set; }
        #endregion


        #region コンストラクタ
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="kind">テトリミノの種類</param>
        private Tetrimino(TetriminoKind kind)
        {
            this.Kind = kind;
            this.Position = kind.InitialPosition();
            this.Blocks = kind.CreateBlock(this.Position);
        }
        #endregion


        #region 生成
        /// <summary>
        /// ランダムにテトリミノの種類を取得します。
        /// </summary>
        /// <returns>テトリミノの種類</returns>
        public static TetriminoKind RandomKind()
        {
            var length = Enum.GetValues(typeof(TetriminoKind)).Length;
            return (TetriminoKind)RandomProvider.ThreadRandom.Next(length);
        }


        /// <summary>
        /// テトリミノを生成します。
        /// </summary>
        /// <returns>インスタンス</returns>
        public static This Create(TetriminoKind? kind = null)
        {
            kind = kind ?? This.RandomKind();
            return new This(kind.Value);
        }
        #endregion


        #region 操作
        /// <summary>
        /// 指定された方向に移動します。
        /// </summary>
        /// <param name="direction">移動方向</param>
        /// <param name="checkCollision">指定のブロックが衝突しているかどうかのデリゲート</param>
        /// <returns>移動したかどうか</returns>
        public bool Move(MoveDirection direction, Func<Block, bool> checkCollision)
        {
            //--- 移動後の位置を決定
            var position = this.Position;
            if (direction == MoveDirection.Down)
            {
                var row = position.Row + 1;
                position = new Position(row, position.Column);
            }
            else
            {
                var delta = (direction == MoveDirection.Right) ? 1 : -1;
                var column = position.Column + delta;
                position = new Position(position.Row, column);
            }

            //--- 位置に合わせたブロックを生成
            var blocks = this.Kind.CreateBlock(position, this.Direction);

            //--- ブロックが衝突していないか
            if (blocks.Any(checkCollision))
                return false;

            //--- 衝突していない場合は移動状態を保存
            this.Position = position;
            this.Blocks = blocks;
            return true;
        }


        /// <summary>
        /// 指定された方向に回転します。
        /// </summary>
        /// <param name="rotationDirection">回転方向</param>
        /// <param name="checkCollision">指定のブロックが衝突しているかどうかのデリゲート</param>
        /// <returns>回転したかどうか</returns>
        public bool Rotation(RotationDirection rotationDirection, Func<Block, bool> checkCollision)
        {
            //--- 回転後の向きを決定
            var count = Enum.GetValues(typeof(Direction)).Length;
            var delta = (rotationDirection == RotationDirection.Right) ? 1 : -1;
            var direction = (int)this.Direction + delta;
            if (direction < 0)      direction += count;
            if (direction >= count) direction %= count;

            //--- 横方向に対するローテーション補正 (Super Rotation)
            var adjustPattern   = this.Kind == TetriminoKind.I
                                ? new [] { 0, 1, -1, 2, -2 }  //--- 形状が I の場合は最大 2 セルの補正
                                : new [] { 0, 1, -1 };  //--- それ以外の場合は 1 セル補正
            foreach (var adjust in adjustPattern)
            {
                //--- 向きに合わせたブロックを生成
                var position = new Position(this.Position.Row, this.Position.Column + adjust);
                var blocks = this.Kind.CreateBlock(position, (Direction)direction);

                //--- 衝突していない場合は回転状態を保存
                if (!blocks.Any(checkCollision))
                {
                    this.Direction = (Direction)direction;
                    this.Position = position;
                    this.Blocks = blocks;
                    return true;
                }
            }

            //--- 回転不能
            return false;
        }
        #endregion
    }
}