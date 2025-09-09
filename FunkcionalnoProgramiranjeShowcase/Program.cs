using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace MyBenchmarks
{
    public class SelectEfficiency
    {
        public int N = 10000000;
        private readonly int[] array;
        private readonly Func<int, double> operation = x => x + x;

        public SelectEfficiency()
        {
            array = [.. Enumerable.Range(1, N)];
        }
        [Benchmark]
        public double[] MathForLoop()
        {
            var result = new double[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = operation(i);
            }
            return result;
        }

        [Benchmark]
        public double[] MathForEachLoop()
        {
            var result = new double[array.Length];
            int index = 0;
            foreach (var num in array)
            {
                result[index++] = operation(num);
            }
            return result;
        }

        [Benchmark]
        public double[] MathLinqSelect()
        {
            return [.. array.Select(x => operation(x))];
        }
    }

    public class StringEfficiency
    {
        public int N = 10000000;
        private readonly string[] array;
        private readonly Func<string, string> operation = s => s + s.ToUpper();

        public StringEfficiency()
        {
            array = [.. Enumerable.Range(1, N).Select(x => $"item{x}")];
        }

        [Benchmark]
        public string[] StringForLoop()
        {
            var result = new string[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = operation(array[i]);
            }
            return result;
        }

        [Benchmark]
        public string[] StringForEachLoop()
        {
            var result = new string[array.Length];
            int index = 0;
            foreach (var str in array)
            {
                result[index++] = operation(str);
            }
            return result;
        }

        [Benchmark]
        public string[] StringLinqSelect()
        {
            return [.. array.Select(s => operation(s))];
        }
    }
    public class ObjectEfficiency
    {
        public int N = 10000000;
        private readonly int[] array;

        public ObjectEfficiency()
        {
            array = Enumerable.Range(1, N).ToArray();
        }

        public class Person
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }

        private readonly Func<int, Person> operation =
            x => new Person { Id = x, Name = $"Name{x}" };

        [Benchmark]
        public Person[] ObjectCreateForLoop()
        {
            var result = new Person[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = operation(array[i]);
            }
            return result;
        }

        [Benchmark]
        public Person[] ObjectCreateForEachLoop()
        {
            var result = new Person[array.Length];
            int index = 0;
            foreach (var num in array)
            {
                result[index++] = operation(num);
            }
            return result;
        }

        [Benchmark]
        public Person[] ObjectCreateLinqSelect()
        {
            return [.. array.Select(x => operation(x))];
        }
    }

    public class ObjectUpdateEfficiency
    {
        private int N = 10000000;
        private readonly Person[] people;

        public ObjectUpdateEfficiency()
        {
            people = Enumerable.Range(1, N).Select(x => new Person { Id = x, Name = $"OldName{x}" }).ToArray();
        }

        public class Person
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }

        private readonly Func<Person, Person> updateOperation = p => { p.Name = $"Updated{p.Id}"; return p; };


        [Benchmark]
        public Person[] ForLoopUpdate()
        {
            var result = new Person[people.Length];
            for (int i = 0; i < people.Length; i++)
            {
                result[i] = updateOperation(people[i]);
            }
            return result;
        }

        [Benchmark]
        public Person[] ForEachUpdate()
        {
            var result = new Person[people.Length];
            int index = 0;
            foreach (var person in people)
            {
                result[index++] = updateOperation(person);
            }
            return result;
        }

        [Benchmark]
        public Person[] LinqSelectUpdate()
        {
            return [.. people.Select(updateOperation)];
        }
    }

    public class CheapWhereEfficiency
    {
        private int N = 10000000;
        private readonly int[] array;

        public CheapWhereEfficiency()
        {
            array = Enumerable.Range(1, N).ToArray();
        }

        private readonly Func<int, bool> predicate = x => x % 2 == 0;

        [Benchmark]
        public int[] ForLoopWhere()
        {
            var list = new List<int>();
            for (int i = 0; i < array.Length; i++)
            {
                if (predicate(array[i]))
                    list.Add(array[i]);
            }
            return list.ToArray();
        }

        [Benchmark]
        public int[] ForEachWhere()
        {
            var list = new List<int>();
            foreach (var num in array)
            {
                if (predicate(num))
                    list.Add(num);
            }
            return list.ToArray();
        }

        [Benchmark]
        public int[] LinqWhere()
        {
            return array.Where(predicate).ToArray();
        }
    }

    public class ExpensiveWhereEfficiency
    {
        private int N = 10000000;
        private readonly int[] array;

        public ExpensiveWhereEfficiency()
        {
            array = Enumerable.Range(1, N).ToArray();
        }

        private readonly Func<int, bool> predicate = x => Math.Sin(x) * Math.Log(x + 1) + Math.Sqrt(x) > 1000;

        [Benchmark]
        public int[] ForLoopWhere()
        {
            var list = new List<int>();
            for (int i = 0; i < array.Length; i++)
            {
                if (predicate(array[i]))
                    list.Add(array[i]);
            }
            return list.ToArray();
        }

        [Benchmark]
        public int[] ForEachWhere()
        {
            var list = new List<int>();
            foreach (var num in array)
            {
                if (predicate(num))
                    list.Add(num);
            }
            return list.ToArray();
        }

        [Benchmark]
        public int[] LinqWhere()
        {
            return array.Where(predicate).ToArray();
        }
    }

    public class WhereStringEfficiency
    {
        private int N = 10000000;
        private readonly string[] array;

        public WhereStringEfficiency()
        {
            array = [.. Enumerable.Range(1, N).Select(x => $"item{x}")];
        }

        private readonly Func<string, bool> condition = s => s.StartsWith("item1");

        [Benchmark]
        public string[] ForLoopWhere()
        {
            var list = new List<string>();
            for (int i = 0; i < array.Length; i++)
            {
                if (condition(array[i]))
                {
                    list.Add(array[i]);
                }
            }
            return list.ToArray();
        }

        [Benchmark]
        public string[] ForEachLoopWhere()
        {
            var list = new List<string>();
            foreach (var str in array)
            {
                if (condition(str))
                {
                    list.Add(str);
                }
            }
            return list.ToArray();
        }

        [Benchmark]
        public string[] LinqWhere()
        {
            return array.Where(condition).ToArray();
        }
    }

    public class WhereObjectEfficiency
    {
        private int N = 10000000;
        private readonly Person[] people;

        public WhereObjectEfficiency()
        {
            people = [.. Enumerable.Range(1, N).Select(x => new Person { Id = x, Name = $"Name{x}" })];
        }

        public class Person
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }

        private readonly Func<Person, bool> condition = p => p.Id % 2 == 0 && p.Name.Contains('5');

        [Benchmark]
        public Person[] ForLoopWhere()
        {
            var list = new List<Person>();
            for (int i = 0; i < people.Length; i++)
            {
                if (condition(people[i]))
                {
                    list.Add(people[i]);
                }
            }
            return [.. list];
        }

        [Benchmark]
        public Person[] ForEachLoopWhere()
        {
            var list = new List<Person>();
            foreach (var person in people)
            {
                if (condition(person))
                {
                    list.Add(person);
                }
            }
            return [.. list];
        }

        [Benchmark]
        public Person[] LinqWhere()
        {
            return [.. people.Where(condition)];
        }
    }

    public class AggregateSumEfficiency
    {
        private int N = 10000000;
        private readonly int[] numbers;

        public AggregateSumEfficiency()
        {
            numbers = Enumerable.Range(1, N).ToArray();
        }

        [Benchmark]
        public long ForLoopSum()
        {
            long sum = 0;
            for (int i = 0; i < numbers.Length; i++)
            {
                sum += numbers[i];
            }
            return sum;
        }

        [Benchmark]
        public long ForEachLoopSum()
        {
            long sum = 0;
            foreach (var num in numbers)
            {
                sum += num;
            }
            return sum;
        }

        [Benchmark]
        public long LinqAggregateSum()
        {
            return numbers.Aggregate((a, b) => a + b);
        }
    }

    public class AggregateStringEfficiency
    {
        private int N = 10000000;
        private readonly string[] words;

        public AggregateStringEfficiency()
        {
            words = Enumerable.Range(1, N).Select(x => $"Word{x}").ToArray();
        }

        [Benchmark]
        public string ForLoopConcat()
        {
            string result = "";
            for (int i = 0; i < words.Length; i++)
            {
                result += words[i];
            }
            return result;
        }

        [Benchmark]
        public string ForEachLoopConcat()
        {
            string result = "";
            foreach (var word in words)
            {
                result += word;
            }
            return result;
        }

        [Benchmark]
        public string LinqAggregateConcat()
        {
            return words.Aggregate((a, b) => a + b);
        }
    }

    public class AggregateObjectEfficiency
    {
        private int N = 10000000;
        private readonly int[] numbers;

        public AggregateObjectEfficiency()
        {
            numbers = Enumerable.Range(1, N).ToArray();
        }

        public class Stats
        {
            public int Min { get; set; }
            public int Max { get; set; }
            public long Sum { get; set; }
        }

        [Benchmark]
        public Stats ForLoopStats()
        {
            var stats = new Stats { Min = int.MaxValue, Max = int.MinValue, Sum = 0 };
            for (int i = 0; i < numbers.Length; i++)
            {
                int n = numbers[i];
                if (n < stats.Min) stats.Min = n;
                if (n > stats.Max) stats.Max = n;
                stats.Sum += n;
            }
            return stats;
        }

        [Benchmark]
        public Stats ForEachLoopStats()
        {
            var stats = new Stats { Min = int.MaxValue, Max = int.MinValue, Sum = 0 };
            foreach (var n in numbers)
            {
                if (n < stats.Min) stats.Min = n;
                if (n > stats.Max) stats.Max = n;
                stats.Sum += n;
            }
            return stats;
        }

        [Benchmark]
        public Stats LinqAggregateStats()
        {
            return numbers.Aggregate(
                new Stats { Min = int.MaxValue, Max = int.MinValue, Sum = 0 },
                (acc, n) =>
                {
                    if (n < acc.Min) acc.Min = n;
                    if (n > acc.Max) acc.Max = n;
                    acc.Sum += n;
                    return acc;
                });
        }
    }

    public class GroupByNumberEfficiency
    {
        private int N = 10000000;
        private readonly int[] numbers;

        public GroupByNumberEfficiency()
        {
            numbers = Enumerable.Range(1, N).ToArray();
        }

        [Benchmark]
        public Dictionary<int, List<int>> ForLoopGroupByModulo()
        {
            var groups = new Dictionary<int, List<int>>();
            for (int i = 0; i < numbers.Length; i++)
            {
                int key = numbers[i] % 10;
                if (!groups.ContainsKey(key))
                    groups[key] = new List<int>();
                groups[key].Add(numbers[i]);
            }
            return groups;
        }

        [Benchmark]
        public Dictionary<int, List<int>> ForEachLoopGroupByModulo()
        {
            var groups = new Dictionary<int, List<int>>();
            foreach (var num in numbers)
            {
                int key = num % 10;
                if (!groups.ContainsKey(key))
                    groups[key] = new List<int>();
                groups[key].Add(num);
            }
            return groups;
        }

        [Benchmark]
        public Dictionary<int, List<int>> LinqGroupByModulo()
        {
            return numbers.GroupBy(x => x % 10)
                          .ToDictionary(g => g.Key, g => g.ToList());
        }
    }

    public class GroupByStringEfficiency
    {
        private int N = 10000000;
        private readonly string[] words;

        public GroupByStringEfficiency()
        {
            words = Enumerable.Range(1, N).Select(x => $"Word{x}").ToArray();
        }

        [Benchmark]
        public Dictionary<char, List<string>> ForLoopGroupByFirstLetter()
        {
            var groups = new Dictionary<char, List<string>>();
            for (int i = 0; i < words.Length; i++)
            {
                char key = words[i][0];
                if (!groups.ContainsKey(key))
                    groups[key] = new List<string>();
                groups[key].Add(words[i]);
            }
            return groups;
        }

        [Benchmark]
        public Dictionary<char, List<string>> ForEachLoopGroupByFirstLetter()
        {
            var groups = new Dictionary<char, List<string>>();
            foreach (var word in words)
            {
                char key = word[0];
                if (!groups.ContainsKey(key))
                    groups[key] = new List<string>();
                groups[key].Add(word);
            }
            return groups;
        }

        [Benchmark]
        public Dictionary<char, List<string>> LinqGroupByFirstLetter()
        {
            return words.GroupBy(w => w[0]).ToDictionary(g => g.Key, g => g.ToList());
        }
    }

    public class GroupByObjectEfficiency
    {
        private int N = 10000000;
        private readonly Person[] people;

        public GroupByObjectEfficiency()
        {
            var rnd = new Random(0);
            people = Enumerable.Range(1, N).Select(x => new Person { Id = x, Age = rnd.Next(18, 60) }).ToArray();
        }

        public class Person
        {
            public int Id { get; set; }
            public int Age { get; set; }
        }

        [Benchmark]
        public Dictionary<int, List<Person>> ForLoopGroupByAge()
        {
            var groups = new Dictionary<int, List<Person>>();
            for (int i = 0; i < people.Length; i++)
            {
                int key = people[i].Age;
                if (!groups.ContainsKey(key))
                    groups[key] = new List<Person>();
                groups[key].Add(people[i]);
            }
            return groups;
        }

        [Benchmark]
        public Dictionary<int, List<Person>> ForEachLoopGroupByAge()
        {
            var groups = new Dictionary<int, List<Person>>();
            foreach (var p in people)
            {
                int key = p.Age;
                if (!groups.ContainsKey(key))
                    groups[key] = new List<Person>();
                groups[key].Add(p);
            }
            return groups;
        }

        [Benchmark]
        public Dictionary<int, List<Person>> LinqGroupByAge()
        {
            return people.GroupBy(p => p.Age).ToDictionary(g => g.Key, g => g.ToList());
        }
    }

    public class TakeEfficiency
    {
        private int N = 10000000;
        private int[] array;
        private int takeCount = 500000;

        public TakeEfficiency()
        {
            array = Enumerable.Range(1, N).ToArray();
        }

        [Benchmark]
        public int[] ForLoopTake()
        {
            var result = new int[takeCount];
            for (int i = 0; i < takeCount; i++)
                result[i] = array[i];
            return result;
        }

        [Benchmark]
        public int[] ForeachLoopTake()
        {
            var result = new int[takeCount];
            int index = 0;
            foreach (var item in array)
            {
                if (index >= takeCount) break;
                result[index++] = item;
            }
            return result;
        }

        [Benchmark]
        public int[] LinqTake()
        {
            return array.Take(takeCount).ToArray();
        }
    }

    public class SkipEfficiency
    {
        private int N = 10000000;
        private int[] array;
        private int skipCount = 500000;

        public SkipEfficiency()
        {
            array = Enumerable.Range(1, N).ToArray();
        }

        [Benchmark]
        public int[] ForLoopSkip()
        {
            var result = new int[N - skipCount];
            for (int i = skipCount; i < N; i++)
                result[i - skipCount] = array[i];
            return result;
        }

        [Benchmark]
        public int[] ForeachLoopSkip()
        {
            var result = new int[N - skipCount];
            int index = 0;
            int current = 0;
            foreach (var item in array)
            {
                if (current++ < skipCount) continue;
                result[index++] = item;
            }
            return result;
        }

        [Benchmark]
        public int[] LinqSkip()
        {
            return array.Skip(skipCount).ToArray();
        }
    }

    public class PaginationEfficiency
    {
        private int N = 10000000;
        private int[] array;
        private int pageSize = 1000;
        private int pageIndex = 5000;

        public PaginationEfficiency()
        {
            array = Enumerable.Range(1, N).ToArray();
        }

        [Benchmark]
        public int[] ForLoopPagination()
        {
            var result = new int[pageSize];
            int start = (pageIndex - 1) * pageSize;
            for (int i = 0; i < pageSize; i++)
                result[i] = array[start + i];
            return result;
        }

        [Benchmark]
        public int[] ForeachLoopPagination()
        {
            var result = new int[pageSize];
            int start = (pageIndex - 1) * pageSize;
            int index = 0;
            int current = 0;
            foreach (var item in array)
            {
                if (current++ < start) continue;
                if (index >= pageSize) break;
                result[index++] = item;
            }
            return result;
        }

        [Benchmark]
        public int[] LinqPagination()
        {
            return array.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToArray();
        }
    }

    public class SelectManyEfficiency
    {
        private int N = 100000;
        private int M = 10000;
        private int[][] arrayOfArrays;

        public SelectManyEfficiency()
        {
            arrayOfArrays = new int[N][];
            for (int i = 0; i < N; i++)
            {
                arrayOfArrays[i] = Enumerable.Range(1, M).ToArray();
            }
        }

        [Benchmark]
        public int[] ForLoopSelectMany()
        {
            var list = new List<int>(N * M);
            for (int i = 0; i < arrayOfArrays.Length; i++)
            {
                for (int j = 0; j < arrayOfArrays[i].Length; j++)
                {
                    list.Add(arrayOfArrays[i][j]);
                }
            }
            return list.ToArray();
        }

        [Benchmark]
        public int[] ForeachLoopSelectMany()
        {
            var list = new List<int>(N * M);
            foreach (var inner in arrayOfArrays)
            {
                foreach (var item in inner)
                {
                    list.Add(item);
                }
            }
            return list.ToArray();
        }

        [Benchmark]
        public int[] LinqSelectMany()
        {
            return arrayOfArrays.SelectMany(inner => inner).ToArray();
        }
    }

    public class Program
    {
        public static void BenchmarkTests()
        {
            Console.WriteLine("Unesite broj benchmarka (1-18):");
            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Neispravan unos.");
                return;
            }

            switch (choice)
            {
                case 1:
                    BenchmarkRunner.Run<SelectEfficiency>();
                    break;
                case 2:
                    BenchmarkRunner.Run<StringEfficiency>();
                    break;
                case 3:
                    BenchmarkRunner.Run<ObjectEfficiency>();
                    break;
                case 4:
                    BenchmarkRunner.Run<ObjectUpdateEfficiency>();
                    break;
                case 5:
                    BenchmarkRunner.Run<CheapWhereEfficiency>();
                    break;
                case 6:
                    BenchmarkRunner.Run<ExpensiveWhereEfficiency>();
                    break;
                case 7:
                    BenchmarkRunner.Run<WhereStringEfficiency>();
                    break;
                case 8:
                    BenchmarkRunner.Run<WhereObjectEfficiency>();
                    break;
                case 9:
                    BenchmarkRunner.Run<AggregateSumEfficiency>();
                    break;
                case 10:
                    BenchmarkRunner.Run<AggregateStringEfficiency>();
                    break;
                case 11:
                    BenchmarkRunner.Run<AggregateObjectEfficiency>();
                    break;
                case 12:
                    BenchmarkRunner.Run<GroupByNumberEfficiency>();
                    break;
                case 13:
                    BenchmarkRunner.Run<GroupByStringEfficiency>();
                    break;
                case 14:
                    BenchmarkRunner.Run<GroupByObjectEfficiency>();
                    break;
                case 15:
                    BenchmarkRunner.Run<TakeEfficiency>();
                    break;
                case 16:
                    BenchmarkRunner.Run<SkipEfficiency>();
                    break;
                case 17:
                    BenchmarkRunner.Run<PaginationEfficiency>();
                    break;
                case 18:
                    BenchmarkRunner.Run<SelectManyEfficiency>();
                    break;
                default:
                    Console.WriteLine("Nepoznat broj benchmarka.");
                    break;
            }
        }

        public static void LazyEvaluationExample()
        {
            IEnumerable<int> brojevi = Enumerable.Range(1, 10);

            var filtrirani = brojevi
                .Where(x =>
                {
                    Console.WriteLine($"Proveravam {x}");
                    return x % 2 == 0;
                })
                .Select(x => x * 10);

            Console.WriteLine("Kreiran upit, ali još nije izvršen.");

            foreach (var broj in filtrirani)
            {
                Console.WriteLine($"Rezultat: {broj}");
            }
        }

        public static void NoLazyEvalExample()
        {
            IEnumerable<int> brojevi = Enumerable.Range(1, 10);

            var filtrirani = brojevi
                .Where(x =>
                {
                    Console.WriteLine($"Proveravam {x}");
                    return x % 2 == 0;
                })
                .Select(x => x * 10)
                .ToList();

            Console.WriteLine("Upit je izvršen i rezultati su odmah kreirani.");

            foreach (var broj in filtrirani)
            {
                Console.WriteLine($"Rezultat: {broj}");
            }
        }

        public static void Main(string[] args)
        {
            BenchmarkTests();
        }
    }
}