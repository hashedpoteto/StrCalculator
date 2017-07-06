using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashedCode
{
    public class StrCalculator
    {
        //計算用スタック
        Stack<double?> DimeNumStk;
        //計算演算子スタック
        Stack<Marker> DimeMkStk;


        int strLong;
        int Dime = 0;//現在かかっいる括弧

        public StrCalculator()
        {
            DimeNumStk = new Stack<double?>();
            DimeMkStk = new Stack<Marker>();
        }

        //計算場所
        public double Calc(String calcStr)
        {

            DimeNumStk.Clear();//スタックを初期化する
            DimeMkStk.Clear();
            Dime = 0;
            strLong = calcStr.Length;

            int i = 0;//ループ用変数
            bool SW = false;//演算子を要求するかどうか

            while (i < strLong)
            //計算式を読み込んで計算する。
            {


                if (char.IsNumber(calcStr[i]))
                //数字なら
                {
                    if (SW)
                        //もし、演算子が来るべきなら
                        //例外を返す
                        throw new Exception((i + 1) + "番目の" + calcStr[i] + "が無効な文字");

                    int DeciRank = 0;//小数第何位かどうか
                    double? value = 0;//項を格納する

                    //項を読み取る
                    while (true)
                    {
                        //文字列の長さを超えたら抜ける
                        if (i >= strLong) break;

                        if (char.IsNumber(calcStr[i]))
                        //数字だったら
                        {
                            //数字なら
                            if (DeciRank == 0)
                            //整数部分なら
                            {
                                value *= 10;
                                value += calcStr[i] - '0';
                            }
                            else
                            //小数部分なら
                            {

                                value += (calcStr[i] - '0') / Math.Pow(10.0, DeciRank);
                                DeciRank++;
                            }
                            //次へ
                            i++;
                        }
                        else
                        //数字でないなら
                        {
                            //スペースを削除
                            IsSpace(calcStr, ref i);

                            if (calcStr[i] != '.' || DeciRank > 0)
                                //.文字でないなら
                                break;
                            //次へ行き、小数モードへ
                            i++;
                            DeciRank++;
                        }
                    }
                    //値をプッシュする
                    DimeNumStk.Push(value);
                    SW = true;
                }
                else
                //数字でないなら
                {
                    if (IsSpace(calcStr, ref i))
                        continue;


                    Marker marker;
                    double? b_value;

                    //マーカーを読み込む
                    switch (calcStr[i])
                    {
                        case '(':
                            //右向き括弧

                            if (SW)
                            //記号が要求されているなら
                            {
                                DimeMkStk.Push(new Marker('*', Dime));
                                //次の文字は数字を要求する
                                SW = false;
                            }
                            //深くする
                            Dime++;


                            break;
                        case ')':
                            //左向き括弧
                            while (DimeMkStk.Count > 0 && DimeMkStk.Peek().dime == Dime)
                            {
                                b_value = DimeNumStk.Pop();
                                DimeNumStk.Push(DimeMkStk.Pop().Calc(DimeNumStk.Pop(), b_value));
                            }

                            //浅くする
                            Dime--;

                            if (Dime < 0)
                                //もし対応する括弧がなくなったら
                                //例外を返す
                                throw new Exception((i + 1) + "番目の ) に対応した ( がない");

                            //次の文字は演算子を要求
                            SW = true;
                            break;
                        default:
                            //その他演算子
                            marker = new Marker(calcStr[i], Dime);
                            //マーカー読み取り
                            if (!marker.IsActive)
                                //演算子が有効でないなら
                                throw new Exception((i + 1) + "番目の" + calcStr[i] + "が無効な文字");

                            while (DimeMkStk.Count > 0 && DimeMkStk.Peek() >= marker)
                            //演算子優先度が後よりも高い場合
                            {
                                b_value = DimeNumStk.Pop();
                                DimeNumStk.Push(DimeMkStk.Pop().Calc(DimeNumStk.Pop(), b_value));
                            }

                            DimeMkStk.Push(marker);
                            if (!SW)
                                //もし、数字が来るべきなら
                                //例外を返す
                                throw new Exception((i + 1) + "番目の" + calcStr[i] + "が無効な文字");
                            //次の文字は数字を要求
                            SW = false;
                            break;
                    }

                    i++;
                }

            }
            if (Dime != 0)
                //例外を返す
                throw new ArrayTypeMismatchException();

            while (DimeMkStk.Count > 0)
            {
                double? b_value = DimeNumStk.Pop();
                DimeNumStk.Push(DimeMkStk.Pop().Calc(DimeNumStk.Pop(), b_value));
            }

            if (DimeNumStk.Count == 0)
                return 0;

            if (DimeNumStk.Count != 1)
                //例外を返す
                throw new ArrayTypeMismatchException();

            return (double)(DimeNumStk.Pop());
        }



        //スペース
        //その文字がスペースかどうか（連なっている場合はすべて移動する）
        public bool IsSpace(String str, ref int i)
        {
            //
            if (str[i] == ' ')
            //スぺースがあったら
            {

                do
                {
                    //スペースがなくなるか終点まで繰り返す
                    i++;
                } while (i < strLong && str[i] == ' ');

                return true;
            }
            //スペースがなかったら
            return false;
        }
    }

    //演算子クラス
    internal class Marker
    {
        public int mark { get; }//演算子の種類
        public int rank { get; }//演算子の優先順位
        public int dime { get; }//括弧の深さ

        static int dimension;
        //このクラスがアクティブかどうか
        public bool IsActive
        {
            get
            {
                if (this.rank != 0) return true;
                return false;
            }
        }

        //ディメンションアップダウン
        void UpDime() { dimension++; }
        void DownDime() { dimension++; }

        //ディメンションをクリア
        void Clear() { dimension = 0; }

        //コンストラクタ
        public Marker(char Mark, int Dime)
        {
            //ディメンション
            dime = Dime;
            //マークによって
            switch (Mark)
            {
                case '+':
                    //足し算
                    mark = 1;
                    rank = 1;
                    break;
                case '-':
                    //引き算
                    mark = 2;
                    rank = 1;
                    break;
                case '*':
                    //掛け算
                    mark = 3;
                    rank = 2;
                    break;
                case '/':
                    //割り算
                    mark = 4;
                    rank = 2;
                    break;
                case '^':
                    //指数計算
                    mark = 5;
                    rank = 3;
                    break;
                default:

                    break;
            }
        }

        //演算子による計算
        public double? Calc(double? a, double? b)
        {
            switch (mark)
            {
                case 1:
                    return a + b;
                case 2:
                    return a - b;
                case 3:
                    return a * b;
                case 4:
                    return a / b;
                case 5:
                    return Math.Pow((double)a, (double)b);
            }
            return null;
        }

        //オペレータ


        //比較演算子
        public static bool operator >=(Marker a, Marker b)
        {
            if (a.dime == b.dime)
            {
                if (a.rank >= b.rank)
                    return true;
                else
                    return false;
            }
            else if (a.dime > b.dime)
                return true;
            return false;
        }
        public static bool operator <=(Marker a, Marker b)
        {
            return false;
        }
    }
}
