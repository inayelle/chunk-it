using ChunkIt.Metrics.Deduplication.Controllers;

// var controller = new AmendSourceFilesController();
var controller = new ChunkingController();

await controller.Run();