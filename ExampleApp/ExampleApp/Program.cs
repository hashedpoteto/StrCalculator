using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HashedCode;


namespace ExampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            StrCalculator StrCalc = new StrCalculator();
            
            while (true)
            {
                try
                {
                    Console.Write("式:");
                    double reslt = StrCalc.Calc(Console.ReadLine());
                    Console.WriteLine("答え:" + reslt + "\n");
                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message + "\n");
                }
            }
        }
    }
}
