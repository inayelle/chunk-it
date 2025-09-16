using ChunkIt.Sandbox.Controllers;

// var controller = new AmendSourceFilesController();
var controller = new ChunkingController();

await controller.Run();