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
        //[Params(1000000)]
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
        //[Params(1000000)]
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

        // Uslov: string počinje sa "item1"
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

        // Uslov: osoba ima paran Id i ime koje sadrži "5"
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
        private int N = 1000000;
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
        private int N = 100000;
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
        private int N = 1000000;
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

    public class Program
    {
        public static void Main(string[] args)
        {
            //var summary1 = BenchmarkRunner.Run<SelectEfficiency>();
            //var summary2 = BenchmarkRunner.Run<StringEfficiency>();
            //var summary3 = BenchmarkRunner.Run<ObjectEfficiency>();
            //var summary4 = BenchmarkRunner.Run<ObjectUpdateEfficiency>();
            //var summary5 = BenchmarkRunner.Run<CheapWhereEfficiency>();
            //var summary6 = BenchmarkRunner.Run<ExpensiveWhereEfficiency>();
            //var summary7 = BenchmarkRunner.Run<WhereStringEfficiency>();
            //var summary8 = BenchmarkRunner.Run<WhereObjectEfficiency>();
            //var summary9 = BenchmarkRunner.Run<AggregateSumEfficiency>();
            //var summary10 = BenchmarkRunner.Run<AggregateStringEfficiency>();
            var summary11 = BenchmarkRunner.Run<AggregateObjectEfficiency>();
        }
    }
}