"use client";

import { useEffect, useRef, useState } from "react";
import { SciChartSurface, NumericAxis, DateTimeNumericAxis, XyDataSeries, SplineLineRenderableSeries, LegendModifier, EAutoRange, MouseWheelZoomModifier, ZoomPanModifier, XAxisDragModifier, YAxisDragModifier, NumberRange } from "scichart";
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
            sciChartSurface.yAxes.add(new NumericAxis(wasmContext, { autoRange: EAutoRange.Never, growBy: new NumberRange(0.1, 0.1) }));

            const xValues = rawData.map((p) => new Date(p.date).getTime() / 1000);
            const closeValues = rawData.map((p) => p.close);
            const shortMaValues = rawData.map((p) => p.maShortPeriod ?? NaN);
            const longMaValues = rawData.map((p) => p.maLongPeriod ?? NaN);

            sciChartSurface.renderableSeries.add(
                new SplineLineRenderableSeries(wasmContext, {
                    dataSeries: new XyDataSeries(wasmContext, { dataSeriesName: "Close", xValues, yValues: closeValues, containsNaN: false, isSorted: true }),
                    stroke: "#ffffff",
                    strokeThickness: 2,
                })
            );

            sciChartSurface.renderableSeries.add(
                new SplineLineRenderableSeries(wasmContext, {
                    dataSeries: new XyDataSeries(wasmContext, { dataSeriesName: "Short MA", xValues, yValues: shortMaValues, containsNaN: true, isSorted: true }),
                    stroke: "#4fc3f7",
                    strokeThickness: 2,
                })
            );

            sciChartSurface.renderableSeries.add(
                new SplineLineRenderableSeries(wasmContext, {
                    dataSeries: new XyDataSeries(wasmContext, { dataSeriesName: "Long MA", xValues, yValues: longMaValues, containsNaN: true, isSorted: true }),
                    stroke: "#f48fb1",
                    strokeThickness: 2,
                })
            );

            sciChartSurface.chartModifiers.add(new LegendModifier());
            sciChartSurface.chartModifiers.add(new MouseWheelZoomModifier());
            sciChartSurface.chartModifiers.add(new ZoomPanModifier());
            sciChartSurface.chartModifiers.add(new XAxisDragModifier());
            sciChartSurface.chartModifiers.add(new YAxisDragModifier());

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
            <div style={{ display: "flex", gap: "12px", alignItems: "flex-end", marginBottom: "16px" }}>
                <label>
                    Symbol
                    <input
                        type="text"
                        value={symbol}
                        onChange={(e) => setSymbol(e.target.value.toUpperCase())}
                        placeholder="e.g. AAPL"
                        style={{ display: "block", padding: "6px 8px", marginTop: "4px" }}
                    />
                </label>
                <label>
                    Short Period
                    <input
                        type="number"
                        value={shortPeriod}
                        onChange={(e) => setShortPeriod(Number(e.target.value))}
                        min={1}
                        style={{ display: "block", padding: "6px 8px", marginTop: "4px", width: "80px" }}
                    />
                </label>
                <label>
                    Long Period
                    <input
                        type="number"
                        value={longPeriod}
                        onChange={(e) => setLongPeriod(Number(e.target.value))}
                        min={1}
                        style={{ display: "block", padding: "6px 8px", marginTop: "4px", width: "80px" }}
                    />
                </label>
                <button onClick={handleRun} disabled={status === 'loading'} style={{ padding: "6px 16px" }}>
                    {status === 'loading' ? 'Running...' : 'Run'}
                </button>
            </div>
            <div ref={chartRef} style={{ width: "100%", height: 400 }} />
        </div>
    );
}