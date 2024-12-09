using System;
using System.Collections.Generic;
using System.Linq;
class RandomGenerators
{
    private const long A = 22695477;
    private const long B = 1;
    private const long M = 4294967296; // 2^32

    // Задание №1: Функция генерации чисел методом мультипликативного метода
    // Задание №2: Генерация чисел в интервале [0, 10]
    public static List<double> MultiplicativeGenerator(long a, long b, long m, long seed, int length, double rangeMin, double rangeMax)
    {
        if (m == 0)
        {
            throw new ArgumentException("Параметр m не может быть равен нулю.");
        }

        List<double> randomNumbers = new List<double>();
        long previous = seed;

        for (int i = 0; i < length; i++)
        {
            long next = (a * previous + b) % m;
            randomNumbers.Add(rangeMin + ((double)next / m) * (rangeMax - rangeMin));
            previous = next;
        }

        return randomNumbers;
    }

    // Задание №3: Вычисление математического ожидания и дисперсии
    public static (double mean, double variance) CalculateStatistics(List<double> numbers)
    {
        double mean = numbers.Average();
        double variance = numbers.Select(n => Math.Pow(n - mean, 2)).Average();
        return (mean, variance);
    }

    // Задание №4: Определение периода последовательности
    public static int CalculatePeriod(List<double> numbers)
    {
        for (int i = 1; i < numbers.Count; i++)
        {
            if (numbers[i] == numbers[0])
            {
                return i;
            }
        }
        return numbers.Count;
    }

    // Задание №5: Вычисление относительных частот
    public static double[] CalculateFrequencies(List<double> numbers, double min, double max, int intervals)
    {
        double intervalSize = (max - min) / intervals;
        double[] frequencies = new double[intervals];

        foreach (var number in numbers)
        {
            int index = (int)((number - min) / intervalSize);
            if (index >= 0 && index < intervals)
                frequencies[index]++;
        }

        for (int i = 0; i < intervals; i++)
        {
            frequencies[i] /= numbers.Count;
        }

        return frequencies;
    }

    public static void PrintTextHistogram(double[] frequencies, int intervals, int maxHeight = 20)
    {
        double maxFrequency = frequencies.Max();

        for (int i = 0; i < intervals; i++)
        {
            int barHeight = (int)((frequencies[i] / maxFrequency) * maxHeight); 

            Console.Write($"Интервал {i + 1}: ");
            for (int j = 0; j < barHeight; j++)
            {
                Console.Write("*");
            }
            Console.WriteLine($" ({frequencies[i]:F4})");
        }
    }

    // Задание №6: Расчет критерия Пирсона
    // Функция для расчета вероятностей попадания в j-й интервал
    static double[] CalculateProbabilities(int intervals, double minValue, double maxValue)
    {
        double[] probabilities = new double[intervals];
        double intervalLength = (maxValue - minValue) / intervals;

        for (int j = 0; j < intervals; j++)
        {
            double lowerBound = minValue + j * intervalLength;
            double upperBound = minValue + (j + 1) * intervalLength;
            probabilities[j] = (upperBound - lowerBound) / (maxValue - minValue);
        }
        return probabilities;
    }

    // Функция для расчета критерия Пирсона
    public static double CalculateChiSquare(int n, double[] observedFrequencies, double[] expectedFrequencies)
    {
        double chiSquare = 0;
        for (int i = 0; i < observedFrequencies.Length; i++)
        {
            if (expectedFrequencies[i] > 0)
            {
                double difference = observedFrequencies[i] - expectedFrequencies[i];
                chiSquare += (difference * difference) / expectedFrequencies[i];
            }
        }
        return chiSquare;
    }



    // Задание №7: Генерации чисел с использованием встроенного генератора случайных чисел
    public static List<double> GenerateRandomNumbers(int length, double min, double max)
    {
        Random random = new Random();
        List<double> numbers = new List<double>();

        for (int i = 0; i < length; i++)
        {
            numbers.Add(random.NextDouble() * (max - min) + min);
        }

        return numbers;
    }

    // Функция для расчета математического ожидания (среднего)
    public static double CalculateMean(List<double> numbers)
    {
        double sum = numbers.Sum();
        return sum / numbers.Count;
    }


    // Функция для расчета дисперсии
    public static double CalculateVariance(List<double> numbers)
    {
        double mean = CalculateMean(numbers);
        double variance = numbers.Select(x => (x - mean) * (x - mean)).Average();
        return variance;
    }

    public static void Main()
    {
        // Задание №2: Генерация чисел
        long seed = 1;
        int[] lengths = { 100, 1000, 10000, 100000 };
        int intervals = 10;

        foreach (var length in lengths)
        {
            Console.WriteLine($"\nДлина последовательности: {length}");
            var numbers = MultiplicativeGenerator(A, B, M, seed, length, 0, 10);

            // Задание №3: Вычисление мат. ожидания и дисперсии
            var (mean, variance) = CalculateStatistics(numbers);
            Console.WriteLine($"Среднее значение: {mean}, Дисперсия: {variance}");

            // Теоретические значения для равномерного распределения [0, 10]
            double theoreticalMean = 5;
            double theoreticalVariance = 100.0 / 12.0;
            Console.WriteLine($"Теоретическое среднее: {theoreticalMean}, Теоретическая дисперсия: {theoreticalVariance}");

            // Сравнение результатов
            Console.WriteLine($"Отклонение от теоретического среднего: {Math.Abs(mean - theoreticalMean)}");
            Console.WriteLine($"Отклонение от теоретической дисперсии: {Math.Abs(variance - theoreticalVariance)}");

            // Задание №4: Определение периода
            int period = CalculatePeriod(numbers);
            Console.WriteLine($"Период последовательности: {period}");

            // Задание №5: Вычисление относительных частот
            double[] frequencies = CalculateFrequencies(numbers, 0, 10, intervals);
            Console.WriteLine("Частоты:");
            foreach (var freq in frequencies)
            {
                Console.WriteLine(freq);
            }

            // Задание №6: Расчет критерия Пирсона
            // Построение гистограммы
            Console.WriteLine("Гистограмма относительных частот:");
            PrintTextHistogram(frequencies, intervals);

            // Расчет вероятностей для каждого интервала
            double[] probabilities = CalculateProbabilities(intervals, 0, 10);

            // Расчет критерия Пирсона
            double chiSquare = CalculateChiSquare(length, frequencies, probabilities);
            Console.WriteLine($"Критерий Пирсона: {chiSquare:F4}");

        }
        Console.WriteLine("\nАнализ с генератором случайных чисел:");
        // Повтор анализа для встроенного генератора
        foreach (var length in lengths)
        {
            Console.WriteLine($"\nДлина последовательности: {length}");

            // Генерация случайных чисел с использованием встроенного генератора
            var numbers = GenerateRandomNumbers(length, 0, 10);

            // Расчет частот
            double[] frequencies = CalculateFrequencies(numbers, 0, 10, intervals);

            // Построение гистограммы
            Console.WriteLine("Гистограмма относительных частот:");
            PrintTextHistogram(frequencies, intervals);

            // Расчет вероятностей для каждого интервала
            double[] probabilities = CalculateProbabilities(intervals, 0, 10);

            // Расчет критерия Пирсона
            double chiSquare = CalculateChiSquare(length, frequencies, probabilities);
            Console.WriteLine($"Критерий Пирсона: {chiSquare:F4}");

            // Расчет математического ожидания и дисперсии
            double mean = CalculateMean(numbers);
            double variance = CalculateVariance(numbers);
            double stdDev = Math.Sqrt(variance);

            Console.WriteLine($"Математическое ожидание (среднее): {mean:F4}");
            Console.WriteLine($"Дисперсия: {variance:F4}");
            Console.WriteLine($"Стандартное отклонение: {stdDev:F4}");
        }
    }
}
