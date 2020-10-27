using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using XShell.Core;
using Xunit;

namespace XShell.Tests
{
    public class CollectionEditorTests
    {
        [Fact]
        public void AddTest()
        {
            var editor = new CollectionEditor<string>();

            var npcQueue = new Queue<PropertyChangedEventArgs>();
            var cmdInvQueue = new Queue<EventArgs>();
            editor.PropertyChanged += (s, p) => npcQueue.Enqueue(p);
            editor.AddCommand.CanExecuteChanged += (s, e) => cmdInvQueue.Enqueue(e);

            editor.AddCommand.Name.Is("Add");
            editor.AddCommand.CheckCanExecute(false);

            editor.Items = new ObservableCollection<string> { "Item1", "Item2", "Item3" };

            editor.AddCommand.CheckCanExecute(false);
            npcQueue.CheckNext(Properties.ItemsPropertyChanged).IsEmpty();
            cmdInvQueue.CheckNext(Assert.NotNull).IsEmpty();

            editor.ItemFactory = () => "New Item";
            editor.AddCommand.CheckCanExecute(true);
            npcQueue.CheckNext(Properties.ItemFactoryPropertyChanged).IsEmpty();
            cmdInvQueue.CheckNext(Assert.NotNull).IsEmpty();

            editor.AllowAdd = false;
            editor.AddCommand.CheckCanExecute(false);
            npcQueue.CheckNext(Properties.AllowAddPropertyChanged).IsEmpty();
            cmdInvQueue.CheckNext(Assert.NotNull).IsEmpty();

            editor.AllowAdd = true;
            editor.AddCommand.CheckCanExecute(true);
            npcQueue.CheckNext(Properties.AllowAddPropertyChanged).IsEmpty();
            cmdInvQueue.CheckNext(Assert.NotNull).IsEmpty();

            editor.AddCommand.Execute(null);
            editor.Items.Count.Is(4);
            editor.Items[3].Is("New Item");
            editor.SelectedIndex.Is(3);
            editor.SelectedItem.Is("New Item");
        }

        [Fact]
        public void RemoveTest()
        {
            var editor = new CollectionEditor<string>();

            var npcQueue = new Queue<PropertyChangedEventArgs>();
            var cmdInvQueue = new Queue<EventArgs>();
            editor.PropertyChanged += (s, p) => npcQueue.Enqueue(p);
            editor.RemoveCommand.CanExecuteChanged += (s, e) => cmdInvQueue.Enqueue(e);

            editor.RemoveCommand.Name.Is("Remove");
            editor.RemoveCommand.CheckCanExecute(false);

            editor.Items = new ObservableCollection<string> { "Item1", "Item2", "Item3" };
            editor.RemoveCommand.CheckCanExecute(false);
            npcQueue.CheckNext(Properties.ItemsPropertyChanged).IsEmpty();
            cmdInvQueue.CheckNext(Assert.NotNull).IsEmpty();

            editor.SelectedIndex = 0;
            editor.RemoveCommand.CheckCanExecute(true);
            npcQueue.CheckNext(Properties.SelectedIndexPropertyChanged).CheckNext(Properties.SelectedItemPropertyChanged).IsEmpty();
            cmdInvQueue.CheckNext(Assert.NotNull).IsEmpty();

            editor.AllowRemove = false;
            editor.RemoveCommand.CheckCanExecute(false);
            npcQueue.CheckNext(Properties.AllowRemovePropertyChanged).IsEmpty();
            cmdInvQueue.CheckNext(Assert.NotNull).IsEmpty();

            editor.AllowRemove = true;
            editor.RemoveCommand.CheckCanExecute(true);
            npcQueue.CheckNext(Properties.AllowRemovePropertyChanged).IsEmpty();
            cmdInvQueue.CheckNext(Assert.NotNull).IsEmpty();


            editor.RemoveCommand.Execute(null);
            editor.Items.Count.Is(2);
            editor.Items[0].Is("Item2");
            editor.Items[1].Is("Item3");

            editor.SelectedIndex.Is(0);
            editor.SelectedItem.Is("Item2");
            npcQueue.CheckNext(Properties.SelectedIndexPropertyChanged).CheckNext(Properties.SelectedItemPropertyChanged).IsEmpty();
        }

        [Fact]
        public void CloneTest()
        {
            var editor = new CollectionEditor<string>();

            var npcQueue = new Queue<PropertyChangedEventArgs>();
            var cmdInvQueue = new Queue<EventArgs>();
            editor.PropertyChanged += (s, p) => npcQueue.Enqueue(p);
            editor.CloneCommand.CanExecuteChanged += (s, e) => cmdInvQueue.Enqueue(e);

            editor.CloneCommand.Name.Is("Clone");
            editor.CloneCommand.CheckCanExecute(false);

            editor.Items = new ObservableCollection<string> { "Item1", "Item2", "Item3" };
            editor.CloneCommand.CheckCanExecute(false);
            npcQueue.CheckNext(Properties.ItemsPropertyChanged).IsEmpty();
            cmdInvQueue.CheckNext(Assert.NotNull).IsEmpty();

            editor.ItemCloner = p => "Clone of " + p;
            editor.CloneCommand.CheckCanExecute(false);
            npcQueue.CheckNext(Properties.ItemClonerPropertyChanged).IsEmpty();
            cmdInvQueue.CheckNext(Assert.NotNull).IsEmpty();

            editor.SelectedIndex = 0;
            editor.CloneCommand.CheckCanExecute(true);
            npcQueue.CheckNext(Properties.SelectedIndexPropertyChanged).CheckNext(Properties.SelectedItemPropertyChanged).IsEmpty();
            cmdInvQueue.CheckNext(Assert.NotNull).IsEmpty();

            editor.AllowClone = false;
            editor.CloneCommand.CheckCanExecute(false);
            npcQueue.CheckNext(Properties.AllowClonePropertyChanged).IsEmpty();
            cmdInvQueue.CheckNext(Assert.NotNull).IsEmpty();

            editor.AllowClone = true;
            editor.CloneCommand.CheckCanExecute(true);
            npcQueue.CheckNext(Properties.AllowClonePropertyChanged).IsEmpty();
            cmdInvQueue.CheckNext(Assert.NotNull).IsEmpty();

            editor.CloneCommand.Execute(null);
            editor.Items.Count.Is(4);
            editor.Items[1].Is("Clone of Item1");
            editor.SelectedIndex.Is(1);
            editor.SelectedItem.Is("Clone of Item1");
            npcQueue.CheckNext(Properties.SelectedIndexPropertyChanged).CheckNext(Properties.SelectedItemPropertyChanged).IsEmpty();
            cmdInvQueue.CheckNext(Assert.NotNull).CheckNext(Assert.NotNull).IsEmpty();
        }

        [Fact]
        public void MoveUpTest()
        {
            var editor = new CollectionEditor<string>();

            var npcQueue = new Queue<PropertyChangedEventArgs>();
            var cmdInvQueue = new Queue<EventArgs>();
            editor.PropertyChanged += (s, p) => npcQueue.Enqueue(p);
            editor.MoveUpCommand.CanExecuteChanged += (s, e) => cmdInvQueue.Enqueue(e);

            editor.MoveUpCommand.Name.Is("Move Up");
            editor.MoveUpCommand.CheckCanExecute(false);

            editor.Items = new ObservableCollection<string> { "Item1", "Item2", "Item3" };
            editor.MoveUpCommand.CheckCanExecute(false);
            npcQueue.CheckNext(Properties.ItemsPropertyChanged).IsEmpty();
            cmdInvQueue.CheckNext(Assert.NotNull).IsEmpty();

            editor.SelectedIndex = 1;
            editor.MoveUpCommand.CheckCanExecute(true);
            npcQueue.CheckNext(Properties.SelectedIndexPropertyChanged).CheckNext(Properties.SelectedItemPropertyChanged).IsEmpty();
            cmdInvQueue.CheckNext(Assert.NotNull).IsEmpty();

            editor.SelectedIndex = 0;
            editor.MoveUpCommand.CheckCanExecute(false);
            npcQueue.CheckNext(Properties.SelectedIndexPropertyChanged).CheckNext(Properties.SelectedItemPropertyChanged).IsEmpty();
            cmdInvQueue.CheckNext(Assert.NotNull).IsEmpty();

            editor.SelectedIndex = 1;
            editor.MoveUpCommand.CheckCanExecute(true);
            npcQueue.CheckNext(Properties.SelectedIndexPropertyChanged).CheckNext(Properties.SelectedItemPropertyChanged).IsEmpty();
            cmdInvQueue.CheckNext(Assert.NotNull).IsEmpty();

            editor.AllowMove = false;
            editor.MoveUpCommand.CheckCanExecute(false);
            npcQueue.CheckNext(Properties.AllowMovePropertyChanged).IsEmpty();
            cmdInvQueue.CheckNext(Assert.NotNull).IsEmpty();

            editor.AllowMove = true;
            editor.MoveUpCommand.CheckCanExecute(true);
            npcQueue.CheckNext(Properties.AllowMovePropertyChanged).IsEmpty();
            cmdInvQueue.CheckNext(Assert.NotNull).IsEmpty();

            editor.MoveUpCommand.Execute(null);
            editor.Items.Count.Is(3);
            editor.Items[0].Is("Item2");
            editor.Items[1].Is("Item1");
            editor.Items[2].Is("Item3");
            editor.SelectedIndex.Is(0);
            editor.SelectedItem.Is("Item2");
            npcQueue.CheckNext(Properties.SelectedIndexPropertyChanged).IsEmpty();
            cmdInvQueue.CheckNext(Assert.NotNull).CheckNext(Assert.NotNull).IsEmpty();
            editor.MoveUpCommand.CheckCanExecute(false);
        }

        [Fact]
        public void MoveDownTest()
        {
            var editor = new CollectionEditor<string>();

            var npcQueue = new Queue<PropertyChangedEventArgs>();
            var cmdInvQueue = new Queue<EventArgs>();
            editor.PropertyChanged += (s, p) => npcQueue.Enqueue(p);
            editor.MoveDownCommand.CanExecuteChanged += (s, e) => cmdInvQueue.Enqueue(e);

            editor.MoveDownCommand.Name.Is("Move Down");
            editor.MoveDownCommand.CheckCanExecute(false);

            editor.Items = new ObservableCollection<string> { "Item1", "Item2", "Item3" };
            editor.MoveDownCommand.CheckCanExecute(false);
            npcQueue.CheckNext(Properties.ItemsPropertyChanged).IsEmpty();
            cmdInvQueue.CheckNext(Assert.NotNull).IsEmpty();

            editor.SelectedIndex = 1;
            editor.MoveDownCommand.CheckCanExecute(true);
            npcQueue.CheckNext(Properties.SelectedIndexPropertyChanged).CheckNext(Properties.SelectedItemPropertyChanged).IsEmpty();
            cmdInvQueue.CheckNext(Assert.NotNull).IsEmpty();

            editor.SelectedIndex = 2;
            editor.MoveDownCommand.CheckCanExecute(false);
            npcQueue.CheckNext(Properties.SelectedIndexPropertyChanged).CheckNext(Properties.SelectedItemPropertyChanged).IsEmpty();
            cmdInvQueue.CheckNext(Assert.NotNull).IsEmpty();

            editor.SelectedIndex = 1;
            editor.MoveDownCommand.CheckCanExecute(true);
            npcQueue.CheckNext(Properties.SelectedIndexPropertyChanged).CheckNext(Properties.SelectedItemPropertyChanged).IsEmpty();
            cmdInvQueue.CheckNext(Assert.NotNull).IsEmpty();

            editor.AllowMove = false;
            editor.MoveDownCommand.CheckCanExecute(false);
            npcQueue.CheckNext(Properties.AllowMovePropertyChanged).IsEmpty();
            cmdInvQueue.CheckNext(Assert.NotNull).IsEmpty();

            editor.AllowMove = true;
            editor.MoveDownCommand.CheckCanExecute(true);
            npcQueue.CheckNext(Properties.AllowMovePropertyChanged).IsEmpty();
            cmdInvQueue.CheckNext(Assert.NotNull).IsEmpty();

            editor.MoveDownCommand.Execute(null);
            editor.Items.Count.Is(3);
            editor.Items[0].Is("Item1");
            editor.Items[1].Is("Item3");
            editor.Items[2].Is("Item2");
            editor.SelectedIndex.Is(2);
            editor.SelectedItem.Is("Item2");
            npcQueue.CheckNext(Properties.SelectedIndexPropertyChanged).IsEmpty();
            cmdInvQueue.CheckNext(Assert.NotNull).CheckNext(Assert.NotNull).IsEmpty();
            editor.MoveDownCommand.CheckCanExecute(false);
        }

        [Fact]
        public void ClearTest()
        {
            var editor = new CollectionEditor<string> {Items = null};

            var npcQueue = new Queue<PropertyChangedEventArgs>();
            var cmdInvQueue = new Queue<EventArgs>();
            editor.PropertyChanged += (s, p) => npcQueue.Enqueue(p);
            editor.ClearCommand.CanExecuteChanged += (s, e) => cmdInvQueue.Enqueue(e);

            editor.ClearCommand.Name.Is("Clear");
            editor.ClearCommand.CheckCanExecute(false);

            editor.Items = new ObservableCollection<string> { "Item1", "Item2", "Item3" };
            editor.ClearCommand.CheckCanExecute(true);
            npcQueue.CheckNext(Properties.ItemsPropertyChanged).IsEmpty();
            cmdInvQueue.CheckNext(Assert.NotNull).IsEmpty();

            editor.AllowClear = false;
            editor.ClearCommand.CheckCanExecute(false);
            npcQueue.CheckNext(Properties.AllowClearPropertyChanged).IsEmpty();
            cmdInvQueue.CheckNext(Assert.NotNull).IsEmpty();

            editor.AllowClear = true;
            npcQueue.CheckNext(Properties.AllowClearPropertyChanged).IsEmpty();
            cmdInvQueue.CheckNext(Assert.NotNull).IsEmpty();

            editor.ClearCommand.Execute(null);
            editor.Items.IsEmpty();
            editor.SelectedIndex.Is(-1);
            editor.SelectedItem.Is(null);
        }

        [Fact]
        public void AddCommandsValidityTest()
        {
            var editor = new CollectionEditor<string>();

            Assert.False(editor.AddCommand.CanExecute(null));
            Assert.False(editor.RemoveCommand.CanExecute(null));
            Assert.False(editor.CloneCommand.CanExecute(null));
            Assert.False(editor.MoveUpCommand.CanExecute(null));
            Assert.False(editor.MoveDownCommand.CanExecute(null));
            Assert.True(editor.ClearCommand.CanExecute(null));

            editor.ItemFactory = () => "New Item";
            editor.ItemCloner = p => "Cloned from " + p;
            editor.Items = new ObservableCollection<string> { "Item1", "Item2", "Item3" };

            Assert.True(editor.AddCommand.CanExecute(null));
            Assert.False(editor.RemoveCommand.CanExecute(null));
            Assert.False(editor.CloneCommand.CanExecute(null));
            Assert.False(editor.MoveUpCommand.CanExecute(null));
            Assert.False(editor.MoveDownCommand.CanExecute(null));
            Assert.True(editor.ClearCommand.CanExecute(null));

            editor.SelectedIndex = 1;

            Assert.True(editor.AddCommand.CanExecute(null));
            Assert.True(editor.RemoveCommand.CanExecute(null));
            Assert.True(editor.CloneCommand.CanExecute(null));
            Assert.True(editor.MoveUpCommand.CanExecute(null));
            Assert.True(editor.MoveDownCommand.CanExecute(null));
            Assert.True(editor.ClearCommand.CanExecute(null));

            editor.SelectedIndex = 0;

            Assert.True(editor.AddCommand.CanExecute(null));
            Assert.True(editor.RemoveCommand.CanExecute(null));
            Assert.True(editor.CloneCommand.CanExecute(null));
            Assert.False(editor.MoveUpCommand.CanExecute(null));
            Assert.True(editor.MoveDownCommand.CanExecute(null));
            Assert.True(editor.ClearCommand.CanExecute(null));

            editor.SelectedIndex = 2;

            Assert.True(editor.AddCommand.CanExecute(null));
            Assert.True(editor.RemoveCommand.CanExecute(null));
            Assert.True(editor.CloneCommand.CanExecute(null));
            Assert.True(editor.MoveUpCommand.CanExecute(null));
            Assert.False(editor.MoveDownCommand.CanExecute(null));
            Assert.True(editor.ClearCommand.CanExecute(null));
        }
    }
}
