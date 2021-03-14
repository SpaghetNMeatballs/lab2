using System;
using System.Reflection;
using System.Diagnostics;
using System.IO;

namespace lab2
{
    // Структура для хранения интервалов температур и концентраций раствора
    struct Interval
    {
        public double left;
        public double right;
        public double step;
        public int amount;
        public Interval(double left, double right, double step)
        {
            this.left = left;
            this.right = right;
            this.step = step;
            // Размер массива для хранения рассчитанных вязкостей сахарозы
            this.amount = (int)((right - left) / step) + 1;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            // Первое задание, расчёт данных вязкостей, сравнение с экспериментальными данными и вычисление отклонений
            double calc1 = Math.Round(calc(40, 0), 2), calc2 = Math.Round(calc(50, 50), 2), calc3 = Math.Round(calc(30, 30), 2);
            double delta1 = Math.Round(Math.Abs(calc1 - 0.65), 2), delta2 = Math.Round(Math.Abs(calc2 - 4.94), 2), delta3 = Math.Round(Math.Abs(calc3 - 2.5), 2);
            Console.WriteLine(String.Format("t=40C\tCB=0%\tmu={0}\tdelta={1}\nt=50C\tCB=50%\tmu={2}\tdelta={3}\nt=30C\tCB=30%\tmu={4}\tdelta={5}", calc1, delta1, calc2, delta2, calc3, delta3));

            // Второе задание, вывод массива
            double[][] midarr;

            double leftt = 10, rightt = 50, stept = 10, leftcb = 0, rightcb = 50, stepcb = 2.5;

            Interval t = new Interval(leftt, rightt, stept), cb = new Interval(leftcb, rightcb, stepcb);
            midarr = genArr(t, cb);
            printArr(t, cb, midarr);
            printVersAndDate();
        }

        // Функция расчёта вязкости сахарозы по формуле
        private static double calc(double t, double cb)
        {
            double x = cb / (1900 - 18 * cb);
            double tempmu = 22.46 * x - 0.114 + ((30 - t)/ (91 + t))*(1.1+43.1*Math.Pow(x, 1.25));
            return Math.Pow(10, tempmu);
        }

        // Генерация массива вязкостей на основе данных интервалов
        private static double[][] genArr(Interval t, Interval cb)
        {
            double[][] toReturn = new double[t.amount][];
            for (int i = 0; i < t.amount; i++)
            {
                toReturn[i] = new double[cb.amount];
                for (int j = 0; j < cb.amount; j++)
                {
                    toReturn[i][j] = Math.Round(calc(t.left + i * t.step, cb.left + j * cb.step), 2);
                }
            }
            return toReturn;
        }

        // Вывод массива вязкостей с внешними строкой и колонкой ответственными за интервалы температуры и вязкости
        private static void printArr(Interval t, Interval cb, double[][] inp)
        {
            // Генерация первой строки
            Console.WriteLine("\n\t");
            String towrite = "\t";
            for (int i = 0; i < cb.amount; i++)
            {
                towrite+=(cb.left+i*cb.step).ToString()+'\t';
            }
            Console.WriteLine(towrite);

            // Вывод температур и вязкостей
            for (int i = 0; i < inp.Length; i++)
            {
                Console.WriteLine("{0}\t{1}", t.left + i * t.step, string.Join("\t", inp[i]));
            }
            Console.SetWindowSize(cb.amount * 9, Console.WindowHeight);
        }

        private static void printVersAndDate()
        {
            // Константы времени изменения программы и даты изменения
            DateTime changeDate = new DateTime(2021, 3, 14, 14, 19, 0);
            String version = "1";

            Console.WriteLine(String.Format("\nДата выполнения программы:\t{0}\nВерсия выполненной программы:\t{1}", changeDate, version));
        }

    }
}
