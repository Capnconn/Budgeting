using Microsoft.Extensions.Options;
using System.ComponentModel;
using System.Diagnostics;

namespace BudgetingApp.Server.Ngrok;

// Local-dev convenience: spins up an ngrok tunnel bound to this app's reserved domain as a child
// process, so Plaid can reach the webhook endpoint without anyone manually running `ngrok` in a
// separate terminal. Requires the ngrok CLI to already be installed/authenticated on the machine -
// this only manages its lifecycle, it doesn't provision an account or domain.
public class NgrokTunnelHostedService(IOptions<NgrokOptions> options, ILogger<NgrokTunnelHostedService> logger) : IHostedService, IDisposable
{
    private readonly ChildProcessJobObject _jobObject = new();
    private Process? _process;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var ngrok = options.Value;

        if (!ngrok.Enabled)
        {
            return Task.CompletedTask;
        }

        if (string.IsNullOrWhiteSpace(ngrok.Domain) || ngrok.Port <= 0)
        {
            logger.LogWarning("Ngrok is enabled but Domain/Port are not configured - skipping tunnel startup.");
            return Task.CompletedTask;
        }

        var startInfo = new ProcessStartInfo
        {
            FileName = ngrok.ExecutablePath,
            Arguments = $"http --url={ngrok.Domain} {ngrok.Port}",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
        };

        try
        {
            _process = Process.Start(startInfo);
        }
        catch (Win32Exception ex)
        {
            logger.LogWarning(ex, "Could not start ngrok ('{ExecutablePath}'). Is it installed and on PATH? Continuing without a tunnel.", ngrok.ExecutablePath);
            return Task.CompletedTask;
        }

        if (_process is not null)
        {
            _jobObject.Assign(_process);

            _process.OutputDataReceived += (_, e) => { if (e.Data is not null) logger.LogInformation("[ngrok] {Line}", e.Data); };
            _process.ErrorDataReceived += (_, e) => { if (e.Data is not null) logger.LogInformation("[ngrok] {Line}", e.Data); };
            _process.BeginOutputReadLine();
            _process.BeginErrorReadLine();

            logger.LogInformation("Started ngrok tunnel: https://{Domain} -> http://localhost:{Port}", ngrok.Domain, ngrok.Port);
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        if (_process is { HasExited: false })
        {
            try
            {
                _process.Kill(entireProcessTree: true);
            }
            catch
            {
                // Best effort - if it's already gone, there's nothing to clean up.
            }
        }

        return Task.CompletedTask;
    }

    public void Dispose() => _jobObject.Dispose();
}
