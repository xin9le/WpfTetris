# WpfTetris

MVVM based Tetris application sample using ReactiveProperty on WPF. You can study following through this sample.

* How to use ReactiveProperty
* Simple MVVM (Model - View - ViewModel) architecture
* Programmable data binding

I made this for [Hokuriku ComCamp 2016 powerd by MVPs](http://hokurikucomcamp.connpass.com/event/23628/).
This event is a part of [Japan ComCamp 2016 powered by MVPs](https://technet.microsoft.com/ja-jp/mt637807).

![ScreenShot](https://raw.githubusercontent.com/xin9le/WpfTetris/master/screenshot.png)




## Feature

* Automatic fall down by timer
* Move / Rotation / Fall down
* Fix tetrimino immediately
* Display next tetrimino
* Display deleted rows information
* Speed-up gradually (when tetrimino is deleted)
* [Super rotation](https://ja.wikipedia.org/wiki/%E3%83%86%E3%83%88%E3%83%AA%E3%82%B9)



## How to use

| キー | 動作 |
|---|---|
| ↑ | Rotation right |
| ↓ | Fall down |
| ← | Move left |
| → | Move right |
| X | Rotation right |
| Z | Rotation left |
| Space | Fix tetrimino immediately |
| Esc | Restart |



## Explanation document

[Tetris Algorithm](https://www.slideshare.net/xin9le/tetris-algorithm)



## Installation

This game is provided as ClickOnce application. 
You can download and install it from following URL.

* [WpfTetris](http://clickonceget.azurewebsites.net/app/WpfTetris)




## License

This game is provided under MIT License.




## Author

Takaaki Suzuki (a.k.a [@xin9le](https://twitter.com/xin9le)) is software developer in Japan who awarded Microsoft MVP for Developer Technologies (C#) since July 2012.
