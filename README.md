# serial COM test tool for Windows

Using this tool, you can send and receive via serial COM port.
This software is developed as a Windows Form Application using Visual Studio 2015, written in C#.


このツールはArduino IDEのシリアルモニタとほぼ同等の機能を実現するWindows用のソフトウェアです。
Visual Studio2015を用いて開発されたWindowsフォームアプリケーションであり、C#で記述されています。


Arduino IDEには無い機能として、ラジオボタンで受信したデータの表示形式を変更することができます

+ ASCII: 文字列として表示します
+ DEC: 10進数の値として表示します
+ HEX: 16進数の値として表示します

なお、受信した値が１２８以上（１ビット目が１）の場合は、受信データが '?'/63/0x3F になってしまいますが、C#の仕様です。ASCIIの範囲でご利用ください。


