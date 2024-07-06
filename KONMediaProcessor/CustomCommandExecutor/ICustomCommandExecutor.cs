namespace KONMediaProcessor.CustomCommandExecutor;

public interface ICustomCommandExecutor
{
    string RunCommand(string arguments, CancellationToken cancellationToken = default);
}
