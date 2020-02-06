using System.Threading.Tasks;
using Macman.Io;
using Xunit;

namespace Macman.Tests
{
    public class DownloadTests
    {
        [Fact]
        public async Task Test1()
        {
            var list = await ApiManager.FindAsync("jei", "1.12.2");
        }
    }
}