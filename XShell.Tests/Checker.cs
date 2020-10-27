using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xunit;

namespace XShell.Tests
{
    public static class Checker
    {
        public static void Check(this Data data, int id, string name)
        {
            Assert.Equal(id, data.Id);
            Assert.Equal(name, data.Name);
        }

        public static void Check(this Data x, Data y) 
            => x.Check(y.Id, y.Name);

        public static void CheckReference<T>(this T x, T y, bool equals = true) 
            => Assert.Equal(equals, ReferenceEquals(x, y));

        public static Queue<T> CheckNext<T>(this Queue<T> queue, Action<T> check)
        {
            check(queue.Dequeue());
            return queue;
        }

        public static Queue<T> CheckNext<T>(this Queue<T> queue, T next)
        {
            queue.Dequeue().CheckReference(next);
            return queue;
        }

        public static void IsEmpty<T>(this IEnumerable<T> collection) 
            => Assert.Equal(0, collection.Count());

        public static T Next<T>(this Queue<T> queue) 
            => queue.Dequeue();

        public static T CheckProperty<T, TProperty>(this T data, Func<T, TProperty> getter, TProperty value)
        {
            Assert.Equal(value, getter(data));
            return data;
        }

        public static T CheckCanExecute<T>(this T command, bool canExecute, object parameter = null) where T : ICommand
        {
            Assert.Equal(canExecute, command.CanExecute(parameter));
            return command;
        }

        public static void Is<T>(this T value, T expected) 
            => Assert.Equal(expected, value);
    }

    public class Data
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Data() { }

        public Data(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public Data Clone()
        {
            return (Data) MemberwiseClone();
        }
    }
}
