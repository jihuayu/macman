using System.Threading.Tasks;
using Xunit;

namespace macman.Tests
{
    public class DownloadTests
    {
        [Fact]
        public async Task Test1()
        {
            var list = await Tasks.FindAsync("jei", "1.12.2");
        }
    }
}