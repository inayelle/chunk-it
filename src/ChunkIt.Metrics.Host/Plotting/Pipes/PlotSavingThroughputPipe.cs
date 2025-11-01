// using AnyKit.Pipelines;
// using ChunkIt.Common.Plotting;
// using ScottPlot;
//
// namespace ChunkIt.Metrics.Host.Plotting.Pipes;
//
// internal sealed class PlotSavingThroughputPipe : IPlottingPipe
// {
//     private const string FileName = "bsps.png";
//
//     public Task Invoke(
//         PlottingContext context,
//         AsyncPipeline<PlottingContext> next
//     )
//     {
//         var multiplot = AdaptiveMultiplot.WithColumns(
//             columns: 3,
//             totalCount: context.Reports.Count
//         );
//
//         throw new NotImplementedException();
//     }
// }
//
// file sealed class SavingThroughputPlot : Plot
// {
//     public SavingThroughputPlot(IReadOnlyList<ChunkingReport> reports)
//     {
//         reports.
//     }
//
//     private sealed class SavingThroughputBar : Bar
//     {
//
//     }
// }