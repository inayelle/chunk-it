using ChunkIt.Sandbox.Controllers;

// var controller = new SandboxController();
var controller = new ChunkingController();

await controller.Run();