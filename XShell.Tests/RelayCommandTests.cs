using NUnit.Framework;
using XShell.Core;

namespace XShell.Tests
{
    [TestFixture]
    public class RelayCommandTests
    {
        [Test]
        public void TestExecute()
        {
            var executedCount = 0;
            string executedParameter = null;
            var command = new RelayCommand<string>(p =>
                {
                    executedCount++;
                    executedParameter = p;
                }, p => p == "Test");

            var canExecuteCount = 0;
            command.CanExecuteChanged += (sender, args) => { canExecuteCount++; };

            command.Execute("Test");

            Assert.AreEqual(1, executedCount);
            Assert.AreEqual("Test", executedParameter);
            Assert.AreEqual(1, canExecuteCount);

            Assert.IsTrue(command.CanExecute("Test"));
            Assert.IsFalse(command.CanExecute("Wrong parameter"));

            // Do not execute
            command.Execute("Wrong parameter");
            Assert.AreEqual(1, executedCount);
            
            command.InvalidateCanExecute();
            Assert.AreEqual(2, canExecuteCount);
        }
    }
}
