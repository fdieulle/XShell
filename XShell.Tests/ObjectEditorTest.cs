using System.Collections.Generic;
using XShell.Core;
using Xunit;

namespace XShell.Tests
{
    public class ObjectEditorTest
    {
        [Fact]
        public void Test()
        {
            var editor = new ObjectEditor<Data>(p => p?.Clone());
            
            var properties = new Queue<string>();
            var applyQueue = new Queue<T>();
            var cancelQueue = new Queue<T>();

            editor.PropertyChanged += (sender, args) => properties.Enqueue(args.PropertyName);
            editor.ApplyExecuted += (oldV, newV) => applyQueue.Enqueue(new T(oldV, newV));
            editor.CancelExecuted += (oldV, newV) => cancelQueue.Enqueue(new T(oldV, newV));

            var data = new Data(1, "Test");
            
            editor.Object = data;
            
            properties.CheckNext(p => Assert.Equal("Object", p));
            properties.CheckNext(p => Assert.Equal("Editable", p));
            properties.IsEmpty();

            editor.Object.CheckReference(data);
            editor.Editable.CheckReference(data, false);
            editor.Editable.Check(1, "Test");

            editor.Editable.Name = "Name changed";
            editor.Editable.Check(1, "Name changed");
            var editable = editor.Editable;

            editor.CancelCommand.Execute(null);

            editor.Object.CheckReference(data);
            editor.Editable.CheckReference(data, false);
            editor.Editable.Check(1, "Test");

            var editable1 = editable;
            cancelQueue.CheckNext(p => { p.OldValue.CheckReference(editable1); p.NewValue.Check(data); });
            cancelQueue.IsEmpty();
            properties.CheckNext(p => Assert.Equal("Editable", p));
            properties.IsEmpty();
            
            editable = editor.Editable;
            editor.Editable.Name = "Name changed";
            editor.Editable.Check(1, "Name changed");

            editor.ApplyCommand.Execute(null);

            editor.Object.CheckReference(editable);
            editor.Editable.CheckReference(editable, false);
            editor.Editable.Check(1, "Name changed");

            applyQueue.CheckNext(p => { p.OldValue.CheckReference(data); p.NewValue.CheckReference(editable); });
            applyQueue.IsEmpty();
            properties.CheckNext(p => Assert.Equal("Object", p));
            properties.CheckNext(p => Assert.Equal("Editable", p));
            properties.IsEmpty();


            cancelQueue.IsEmpty();
        }

        private class T
        {
            public Data OldValue { get; private set; }
            public Data NewValue { get; private set; }

            public T(Data oldValue, Data newValue)
            {
                OldValue = oldValue;
                NewValue = newValue;
            }
        }
    }
}
