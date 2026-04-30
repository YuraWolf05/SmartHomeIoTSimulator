namespace SmartHomeIoTSimulator.Infrastructure;

public static class RetryPolicy
{
    public static void Execute(Action action, int attempts = 3)
    {
        int currentAttempt = 0;

        while (true)
        {
            try
            {
                currentAttempt++;
                action();
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}. Спроба {currentAttempt}/{attempts}");

                if (currentAttempt >= attempts)
                    throw;
            }
            finally
            {
                Console.WriteLine("Операцію оброблено.");
            }
        }
    }
}