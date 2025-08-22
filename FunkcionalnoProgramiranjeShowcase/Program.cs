using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace MyBenchmarks
{
    public class SelectEfficiency
    {
        private const int N = 1000000000;
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

    [SimpleJob]
    [MarkdownExporterAttribute.GitHub]
    public class StringEfficiency
    {
        private const int N = 100000000;
        private readonly string[] array;
        private readonly Func<string, string> operation = s => s.ToUpper();

        public StringEfficiency()
        {
            array = Enumerable.Range(1, N).Select(x => $"item{x}").ToArray();
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
        private const int N = 100000000;
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
        private const int N = 100000000;
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
        private const int N = 1000000000;
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
        private const int N = 1000000000;
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
        private const int N = 100000000;
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
        private const int N = 100000000;
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

    public class Program
    {
        public static void Main(string[] args)
        {
            var summary1 = BenchmarkRunner.Run<SelectEfficiency>();
            //var summary2 = BenchmarkRunner.Run<StringEfficiency>();
            //var summary3 = BenchmarkRunner.Run<ObjectEfficiency>();
            //var summary4 = BenchmarkRunner.Run<ObjectUpdateEfficiency>();
            //var summary5 = BenchmarkRunner.Run<CheapWhereEfficiency>();
            //var summary6 = BenchmarkRunner.Run<ExpensiveWhereEfficiency>();
            //var summary7 = BenchmarkRunner.Run<WhereStringEfficiency>();
            //var summary8 = BenchmarkRunner.Run<WhereObjectEfficiency>();
        }
    }
}