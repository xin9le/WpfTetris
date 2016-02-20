using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Timers;
using Reactive.Bindings;
using WpfTetris.Extensions;
using This = WpfTetris.Models.Field;



namespace WpfTetris.Models
{
    /// <summary>
    /// テトリスの場としての機能を提供します。
    /// </summary>
    public class Field
    {
        #region 定数
        /// <summary>
        /// 行数を表します。この値は定数です。
        /// </summary>
        public const byte RowCount = 24;


        /// <summary>
        /// 列数を表します。この値は定数です。
        /// </summary>
        public const byte ColumnCount = 10;
        #endregion


        #region プロパティ
        /// <summary>
        /// 置かれているブロックのコレクションを取得します。
        /// </summary>
        public IReadOnlyReactiveProperty<IReadOnlyList<Block>> PlacedBlocks => this.placedBlocks;
        private readonly ReactiveProperty<IReadOnlyList<Block>> placedBlocks = new ReactiveProperty<IReadOnlyList<Block>>(Array.Empty<Block>(), ReactivePropertyMode.RaiseLatestValueOnSubscribe);


        /// <summary>
        /// 現在動かしているテトリミノを取得または設定します。
        /// </summary>
        public ReactiveProperty<Tetrimino> Tetrimino { get; } = new ReactiveProperty<Tetrimino>();


        /// <summary>
        /// アクティブ状態かどうかを取得します。
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsActivated => this.isActivated;
        private readonly ReactiveProperty<bool> isActivated = new ReactiveProperty<bool>(mode: ReactivePropertyMode.DistinctUntilChanged);



        /// <summary>
        /// 上限ラインを超えているかどうかを取得します。
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsUpperLimitOvered => this.isUpperLimitOvered;
        private readonly ReactiveProperty<bool> isUpperLimitOvered = new ReactiveProperty<bool>(mode: ReactivePropertyMode.DistinctUntilChanged);


        /// <summary>
        /// 削除した行数を取得します。
        /// </summary>
        public IReadOnlyReactiveProperty<int> LastRemovedRowCount => this.lastRemovedRowCount;
        private readonly ReactiveProperty<int> lastRemovedRowCount = new ReactiveProperty<int>(mode: ReactivePropertyMode.None);


        /// <summary>
        /// タイマーを取得します。
        /// </summary>
        private Timer Timer { get; } = new Timer();
        #endregion


        #region コンストラクタ
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public Field()
        {
            this.Timer.ElapsedAsObservable()
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(x => this.MoveTetrimino(MoveDirection.Down));
        }
        #endregion


        #region 操作
        /// <summary>
        /// アクティブ化します。
        /// </summary>
        /// <param name="kind">最初のテトリミノの種類</param>
        public void Activate(TetriminoKind kind)
        {
            this.isActivated.Value = true;
            this.isUpperLimitOvered.Value = false;
            this.Tetrimino.Value = Models.Tetrimino.Create(kind);
            this.placedBlocks.Value = Array.Empty<Block>();
            this.Timer.Interval = 1000;
            this.Timer.Start();
        }


        /// <summary>
        /// 指定された方向にテトリミノを移動します。
        /// </summary>
        /// <param name="direction">移動方向</param>
        public void MoveTetrimino(MoveDirection direction)
        {
            if (!this.isActivated.Value)
                return;

            //--- 下移動は特別処理
            if (direction == MoveDirection.Down)
            {
                this.Timer.Stop();
                if (this.Tetrimino.Value.Move(direction, this.CheckCollision))
                {
                    this.Tetrimino.ForceNotify();
                }
                else
                {
                    //--- テトリミノを配置済みとして確定し、ブロックが揃ってたら消す
                    var result = this.RemoveAndFixBlock();

                    //--- 揃った行数を通知
                    var removedRowCount = result.Item1;
                    if (removedRowCount > 0)
                        this.lastRemovedRowCount.Value = removedRowCount;

                    //--- ブロックが上限を超えていたらゲームオーバー
                    if (result.Item2.Any(x => x.Position.Row < 0))
                    {
                        this.isActivated.Value = false;
                        this.isUpperLimitOvered.Value = true;
                        return;
                    }

                    //--- 更新
                    this.Tetrimino.Value = null;  //--- 一旦クリア
                    this.placedBlocks.Value = result.Item2;
                }
                this.Timer.Start();
                return;
            }

            //--- 左右移動の場合は移動に成功したら変更通知
            if (this.Tetrimino.Value.Move(direction, this.CheckCollision))
                this.Tetrimino.ForceNotify();
        }


        /// <summary>
        /// 指定された方向にテトリミノを回転させます。
        /// </summary>
        /// <param name="direction">回転方向</param>
        public void RotationTetrimino(RotationDirection direction)
        {
            if (!this.isActivated.Value)
                return;

            if (this.Tetrimino.Value.Rotation(direction, this.CheckCollision))
                this.Tetrimino.ForceNotify();
        }


        /// <summary>
        /// テトリミノが落ちる速度を上げます。
        /// </summary>
        public void SpeedUp()
        {
            const int min = 15;  //--- 最速 15 ミリ秒
            var interval = this.Timer.Interval / 2;  //--- 倍速にしていく
            this.Timer.Interval = Math.Max(interval, min);
        }
        #endregion


        #region 判定 / その他
        /// <summary>
        /// 衝突判定を行います。
        /// </summary>
        /// <param name="block">チェック対象のブロック</param>
        /// <returns>衝突している場合true</returns>
        private bool CheckCollision(Block block)
        {
            if (block == null)
                throw new ArgumentNullException(nameof(block));

            //--- 左側の壁にめり込んでいる
            if (block.Position.Column < 0)
                return true;

            //--- 右側の壁にめり込んでいる
            if (This.ColumnCount <= block.Position.Column)
                return true;

            //--- 床にめり込んでいる
            if (This.RowCount <= block.Position.Row)
                return true;

            //--- すでに配置済みブロックがある
            return this.placedBlocks.Value.Any(x => x.Position == block.Position);
        }


        /// <summary>
        /// ブロックが揃っていたら消し、配置済みブロックを確定します。
        /// </summary>
        /// <returns>確定された配置済みブロック</returns>
        private Tuple<int, Block[]> RemoveAndFixBlock()
        {
            //--- 行ごとにブロックをまとめる
            var rows    = this.placedBlocks.Value
                        .Concat(this.Tetrimino.Value.Blocks)  //--- 配置済みのブロックとテトリミノを合成
                        .GroupBy(x => x.Position.Row)  //--- 行ごとにまとめる
                        .Select(x => new
                        {
                            Row = x.Key,
                            IsFilled = This.ColumnCount <= x.Count(),  //--- 揃っているか
                            Blocks = x,
                        })
                        .ToArray();

            //--- 揃ったブロックを削除して確定
            var blocks  = rows
                        .OrderByDescending(x => x.Row)    //--- 深い方から並び替え
                        .WithIndex(x => x.IsFilled)       //--- 揃っている行が見つかるたびにインクリメント
                        .Where(x => !x.Element.IsFilled)  //--- 揃っている行は消す
                        .SelectMany(x =>
                        {
                            //--- ズラす必要がない行はそのまま処理
                            //--- 処理パフォーマンス向上のため特別処理
                            if (x.Index == 0)
                                return x.Element.Blocks;

                            //--- 消えた行のぶん下に段をズラす
                            return x.Element.Blocks.Select(y =>
                            {
                                var position = new Position(y.Position.Row + x.Index, y.Position.Column);
                                return new Block(y.Color, position);
                            });
                        })
                        .ToArray();

            //--- 削除した行数
            var removedRowCount = rows.Count(x => x.IsFilled);
            return Tuple.Create(removedRowCount, blocks);
        }
        #endregion
    }
}