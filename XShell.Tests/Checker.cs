using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace XShell.Tests
{
    public static class Checker
    {
        public static void Check(this Data data, int id, string name)
        {
            Assert.AreEqual(id, data.Id);
            Assert.AreEqual(name, data.Name);
        }

        public static void Check(this Data x, Data y)
        {
            x.Check(y.Id, y.Name);
        }

        public static void CheckReference<T>(this T x, T y, bool equals = true)
        {
            Assert.AreEqual(equals, ReferenceEquals(x, y));
        }

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
        {
            Assert.AreEqual(0, collection.Count(), "Count");
        }

        public static T Next<T>(this Queue<T> queue)
        {
            return queue.Dequeue();
        }

        public static T CheckProperty<T, TProperty>(this T data, Func<T, TProperty> getter, TProperty value,
                                                       string message = null)
        {
            Assert.AreEqual(value, getter(data), message);
            return data;
        }
    }

    public class Data
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Data()
        {
            
        }

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
