using System;

namespace AzAnt
{
    class Program
    {
        public static void Main(string[] args)
        {
            var Args = new AzAntArgs();
            if (!Args.ParseArgs(args))
            {
                if (Args.Error != null)
                {
                    Console.WriteLine(Args.Error);
                }

                Console.WriteLine(AzAntArgs.Usage());
                return;
            }

            try
            {
                new AzAnt(Args).Run();
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Exiting");
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }
    }
}
