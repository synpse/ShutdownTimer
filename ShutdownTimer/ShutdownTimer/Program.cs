namespace ShutdownTimer
{
    class Program
    {
        private static void Main(string[] args)
        {
            // Create new instance of Runner
            Runner r = new Runner();

            // Run setup
            r.Setup();

            // Run the actual program
            r.Run();
        }
    }
}
