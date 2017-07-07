# StrCalculator
String型文字列の式を計算するライブラリです。 

## File（ファイル）
StrCalculator：このリポジトリのフォルダ
-ExampleApp：「ExampleApp.exe」と「StrCalculator.dll」のソースコード（C#）
-ExampleApp.exe：このライブラリを使ったコマンドラインプログラム（式を入力すると答えが表示される）
-StrCalculator：メインライブラリ

## How to use（使い方）
1.「StrCalculator.dll」を参照する。
2.プロログラムで「StrCalculator」クラスの変数を作る。
3.「StrCalculator変数.Calc(String型の式);」で計算できる。（結果は「double」型で返す）

## Avalable formula


### 四則演算
.Calc("3+4");
-> 7

.Calc("2-4");
-> -2

.Calc("6*3");
-> 18

.Calc("10/4");
-> 2.5

### 三項以上の式、括弧を含んだ式
.Calc("12/3+1");
-> 5

.Calc("2-(4+1)");
-> -3

.Calc("(4-2)(3+2)");
-> 10

### 指数計算
.Calc("2^3");
-> 8

