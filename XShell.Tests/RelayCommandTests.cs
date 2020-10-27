using XShell.Core;
using Xunit;

namespace XShell.Tests
{
    public class RelayCommandTests
    {
        [Fact]
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

            Assert.Equal(1, executedCount);
            Assert.Equal("Test", executedParameter);
            Assert.Equal(1, canExecuteCount);

            Assert.True(command.CanExecute("Test"));
            Assert.False(command.CanExecute("Wrong parameter"));

            // Do not execute
            command.Execute("Wrong parameter");
            Assert.Equal(1, executedCount);
            
            command.InvalidateCanExecute();
            Assert.Equal(2, canExecuteCount);
        }
    }
}
