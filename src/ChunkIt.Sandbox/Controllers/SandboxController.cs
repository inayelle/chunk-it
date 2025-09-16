namespace ChunkIt.Sandbox.Controllers;

internal sealed class SandboxController : IController
{
    public async Task Run()
    {
        await Task.Yield();
    }
}