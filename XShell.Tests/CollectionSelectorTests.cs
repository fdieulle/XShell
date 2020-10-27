using System.Collections.Generic;
using System.ComponentModel;
using XShell.Core;
using Xunit;

namespace XShell.Tests
{
    public class CollectionSelectorTests
    {
        [Fact]
        public void NominalTest()
        {
            var pcQueue = new Queue<PropertyChangedEventArgs>();

            var selector = new CollectionSelector<string>(new [] { "Item 1", "Item 2", "Item 3"});
            selector.PropertyChanged += (s, p) => pcQueue.Enqueue(p);

            Check(selector, -1, null);
            pcQueue.IsEmpty();

            selector.SelectedIndex = 2;
            Check(selector, 2, "Item 3");
            pcQueue.CheckNext(Properties.SelectedIndexPropertyChanged)
                .CheckNext(Properties.SelectedItemPropertyChanged);

            selector.SelectedIndex = -1;
            Check(selector, -1, null);
            pcQueue.CheckNext(Properties.SelectedIndexPropertyChanged)
                .CheckNext(Properties.SelectedItemPropertyChanged);

            selector.SelectedItem = "Item 2";
            Check(selector, 1, "Item 2");
            pcQueue.CheckNext(Properties.SelectedItemPropertyChanged)
                .CheckNext(Properties.SelectedIndexPropertyChanged);

            selector.SelectedItem = "Item 2";
            Check(selector, 1, "Item 2");
            pcQueue.IsEmpty();

            selector.SelectedIndex = 1;
            Check(selector, 1, "Item 2");
            pcQueue.IsEmpty();
        }

        private static void Check(CollectionSelector<string> selector, int selectedIndex, string selectedItem)
        {
            Assert.Equal(selectedIndex, selector.SelectedIndex);
            Assert.Equal(selectedItem, selector.SelectedItem);
        }
    }
}
