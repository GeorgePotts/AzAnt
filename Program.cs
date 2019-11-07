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

            new AzAnt(Args).Run();
        }
    }
}
