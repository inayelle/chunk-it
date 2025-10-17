using ScottPlot.TickGenerators;

namespace ChunkIt.Common.Plotting;

public sealed class NumericWithUnitTickGenerator : NumericAutomatic
{
    private readonly string _unit;

    public NumericWithUnitTickGenerator(string unit)
    {
        _unit = unit;

        LabelFormatter = Formatter;
    }

    private string Formatter(double value)
    {
        return LabelFormatters.Numeric(value) + $" {_unit}";
    }
}