using ChunkIt.Metrics.Host.Gathering;
using ChunkIt.Metrics.Host.Plotting;

var gatheringPipeline = new GatheringPipeline();

var reports = await gatheringPipeline.Invoke(new GatheringContext());

var plottingPipeline = new PlottingPipeline();

await plottingPipeline.Invoke(new PlottingContext(reports));