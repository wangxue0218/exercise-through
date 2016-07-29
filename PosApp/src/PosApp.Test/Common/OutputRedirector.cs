using System;
using System.IO;
using Xunit.Abstractions;

namespace PosApp.Test.Common
{
    class OutputRedirector : IDisposable
    {
        readonly ITestOutputHelper outputHelper;
        readonly StringWriter writer;

        public OutputRedirector(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;
            writer = new StringWriter();
            Console.SetOut(writer);
        }

        public void Dispose()
        {
            Console.Out.Flush();
            outputHelper.WriteLine(writer.ToString());
            writer.Dispose();
        }
    }
}