"use client";

import { useEffect, useRef, useState } from "react";
import { Button, TextField } from "@mui/material";
import { SciChartSurface, NumericAxis, DateTimeNumericAxis, XyDataSeries, FastLineRenderableSeries, LegendModifier, EAutoRange, MouseWheelZoomModifier, ZoomPanModifier, XAxisDragModifier, YAxisDragModifier, NumberRange, CursorModifier, EXyDirection, RolloverModifier, SplineLineRenderableSeries } from "scichart";
import { useMovingAverageData, useMovingAverageStatus } from "../../../_redux/hooks";
import { useAppDispatch } from "../../../_redux/hooks";
import { runBacktest } from "../../../_redux/slices/backtesting/moving-average/slice";

SciChartSurface.configure({ wasmUrl: "/scichart2d.wasm" });

export default function MovingAveragePage() {

    // SciChart requires chartRef
    const chartRef = useRef<HTMLDivElement>(null);
    const dispatch = useAppDispatch();

    const rawData = useMovingAverageData();
    const status = useMovingAverageStatus();

    const [symbol, setSymbol] = useState('');
    const [shortPeriod, setShortPeriod] = useState<number>(10);
    const [longPeriod, setLongPeriod] = useState<number>(50);

    const handleRun = () => {
        if (!symbol) return;
        dispatch(runBacktest({ symbol, shortPeriod, longPeriod }));
    };

    useEffect(() => {
        console.log('movingAverage rawData:', rawData);
    }, [rawData]);

    useEffect(() => {
        if (status !== 'success' || rawData.length === 0) return;

        let surface: SciChartSurface;

        const init = async () => {
            if (!chartRef.current) return;

            const { sciChartSurface, wasmContext } = await SciChartSurface.create(chartRef.current);
            surface = sciChartSurface;

            const xAxis = new DateTimeNumericAxis(wasmContext, { autoRange: EAutoRange.Never, growBy: new NumberRange(0.1, 0.1) });
            xAxis.labelProvider.formatLabel = (dataValue: number) => {
                const date = new Date(dataValue * 1000);
                const dd = String(date.getDate()).padStart(2, '0');
                const mm = String(date.getMonth() + 1).padStart(2, '0');
                const yyyy = date.getFullYear();
                return `${dd}-${mm}-${yyyy}`;
            };
            sciChartSurface.xAxes.add(xAxis);
            sciChartSurface.yAxes.add(new NumericAxis(wasmContext, { autoRange: EAutoRange.Once, growBy: new NumberRange(0.1, 0.1) }));

            const xValues = rawData.map((p) => new Date(p.date).getTime() / 1000).sort((a, b) => a - b);
            const closeValues = rawData.map((p) => p.close);

            const shortMaPoints = rawData.filter((p) => p.maShortPeriod != null);
            const shortMaXValues = shortMaPoints.map((p) => new Date(p.date).getTime() / 1000).sort((a, b) => a - b);
            const shortMaYValues = shortMaPoints.map((p) => p.maShortPeriod!);

            const longMaPoints = rawData.filter((p) => p.maLongPeriod != null);
            const longMaXValues = longMaPoints.map((p) => new Date(p.date).getTime() / 1000).sort((a, b) => a - b);
            const longMaYValues = longMaPoints.map((p) => p.maLongPeriod!);

            sciChartSurface.renderableSeries.add(
                new SplineLineRenderableSeries(wasmContext, {
                    dataSeries: new XyDataSeries(wasmContext, { 
                        dataSeriesName: "Close",
                        xValues: xValues,
                        yValues: closeValues,
                        containsNaN: false,
                        isSorted: true 
                    }),
                    stroke: "#ffffff",
                    strokeThickness: 2,
                })
            );7

            sciChartSurface.renderableSeries.add(
                new SplineLineRenderableSeries(wasmContext, {
                    dataSeries: new XyDataSeries(wasmContext, { dataSeriesName: "Short MA", xValues: shortMaXValues, yValues: shortMaYValues, containsNaN: false, isSorted: true }),
                    stroke: "#4fc3f7",
                    strokeThickness: 2,
                })
            );

            sciChartSurface.renderableSeries.add(
                new SplineLineRenderableSeries(wasmContext, {
                    dataSeries: new XyDataSeries(wasmContext, { dataSeriesName: "Long MA", xValues: longMaXValues, yValues: longMaYValues, containsNaN: false, isSorted: true }),
                    stroke: "#f48fb1",
                    strokeThickness: 2,
                })
            );

            // sciChartSurface.chartModifiers.add(new LegendModifier());
            // sciChartSurface.chartModifiers.add(new MouseWheelZoomModifier());
            // sciChartSurface.chartModifiers.add(new ZoomPanModifier());
            // sciChartSurface.chartModifiers.add(new XAxisDragModifier());
            // sciChartSurface.chartModifiers.add(new YAxisDragModifier());
            sciChartSurface.chartModifiers.add(new RolloverModifier({ snapToDataPoint: true }));

            requestAnimationFrame(() => sciChartSurface.zoomExtents());
        };

        init();

        return () => {
            surface?.delete();
        };
    }, [status, rawData]);

    return (
        <div>
            <h1>Backtesting</h1>
            <div style={{ display: "flex", gap: "12px", alignItems: "center", marginBottom: "16px" }}>
                <TextField
                    label="Symbol"
                    value={symbol}
                    onChange={(e) => setSymbol(e.target.value.toUpperCase())}
                    placeholder="e.g. AAPL"
                    size="small"
                />
                <TextField
                    label="Short Period"
                    type="number"
                    value={shortPeriod}
                    onChange={(e) => setShortPeriod(Number(e.target.value))}
                    slotProps={{ htmlInput: { min: 1 } }}
                    size="small"
                    sx={{ width: 120 }}
                />
                <TextField
                    label="Long Period"
                    type="number"
                    value={longPeriod}
                    onChange={(e) => setLongPeriod(Number(e.target.value))}
                    slotProps={{ htmlInput: { min: 1 } }}
                    size="small"
                    sx={{ width: 120 }}
                />
                <Button
                    variant="contained"
                    onClick={handleRun}
                    disabled={status === 'loading'}
                >
                    {status === 'loading' ? 'Running...' : 'Run'}
                </Button>
            </div>
            <div ref={chartRef} style={{ width: "100%", position: "relative" }} />
        </div>
    );
}