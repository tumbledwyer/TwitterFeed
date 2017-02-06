using System.IO;
using NUnit.Framework;

namespace TwitterFeed.Tests.TestUtils
{
    public class FileUtils
    {
        public static string GetTestFile(string testFile)
        {
            var testDirectory = TestContext.CurrentContext.TestDirectory;
            return Path.Combine(testDirectory, "TestData", testFile);
        }
    }
}
