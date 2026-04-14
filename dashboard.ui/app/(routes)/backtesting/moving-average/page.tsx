"use client";

import { useEffect, useRef, useState } from "react";
import { Button, TextField } from "@mui/material";
import {
  SciChartSurface,
  NumericAxis,
  DateTimeNumericAxis,
  XyDataSeries,
  FastLineRenderableSeries,
  LegendModifier,
  EAutoRange,
  MouseWheelZoomModifier,
  ZoomPanModifier,
  XAxisDragModifier,
  YAxisDragModifier,
  NumberRange,
  CursorModifier,
  EXyDirection,
  RolloverModifier,
  SplineLineRenderableSeries,
} from "scichart";
import {
  useMovingAverageData,
  useMovingAverageStatus,
} from "../../../_redux/hooks";
import { useAppDispatch } from "../../../_redux/hooks";
import { runBacktest } from "../../../_redux/slices/backtesting/moving-average/slice";

SciChartSurface.configure({ wasmUrl: "/scichart2d.wasm" });

export default function MovingAveragePage() {
  // SciChart requires chartRef
  const chartRef = useRef<HTMLDivElement>(null);
  const dispatch = useAppDispatch();

  const rawData = useMovingAverageData();
  const status = useMovingAverageStatus();

  const [symbol, setSymbol] = useState("");
  const [shortPeriod, setShortPeriod] = useState<number>(10);
  const [longPeriod, setLongPeriod] = useState<number>(50);

  const handleRun = () => {
    if (!symbol) return;
    dispatch(runBacktest({ symbol, shortPeriod, longPeriod }));
  };

  useEffect(() => {
    console.log("movingAverage rawData:", rawData);
  }, [rawData]);

  useEffect(() => {
    if (status !== "success" || rawData.length === 0) return;

    let surface: SciChartSurface;

    const init = async () => {
      if (!chartRef.current) return;

      const { sciChartSurface, wasmContext } = await SciChartSurface.create(
        chartRef.current,
      );
      surface = sciChartSurface;

      const xAxis = new DateTimeNumericAxis(wasmContext, {
        autoRange: EAutoRange.Never,
        growBy: new NumberRange(0.1, 0.1),
      });
      
      sciChartSurface.xAxes.add(xAxis);
      sciChartSurface.yAxes.add(
        new NumericAxis(wasmContext, {
          autoRange: EAutoRange.Once,
          growBy: new NumberRange(0.1, 0.1),
        }),
      );

      const sorted = [...rawData]
        .sort((a, b) => a.date - b.date);

      const xValuesClose = sorted.map((p) => p.date);
      const yValuesClose = sorted.map((p) => p.close);

      const yValuesShortMa = sorted
        .filter((p) => p.maShortPeriod != null)
        .map((p) => p.maShortPeriod!);
      const xValuesShortMa = sorted
        .filter((p) => p.maShortPeriod != null)
        .map((p) => p.date!);

      const yValuesLongMa = sorted
        .filter((p) => p.maLongPeriod != null)
        .map((p) => p.maLongPeriod!);
      const xValuesLongMa = sorted
        .filter((p) => p.maLongPeriod != null)
        .map((p) => p.date!);


      sciChartSurface.renderableSeries.add(
        new SplineLineRenderableSeries(wasmContext, {
          dataSeries: new XyDataSeries(wasmContext, {
            dataSeriesName: "Close",
            xValues: xValuesClose,
            yValues: yValuesClose,
            containsNaN: false,
            isSorted: true,
          }),
          stroke: "#ffffff",
          strokeThickness: 2,
        }),
      );
      7;

      sciChartSurface.renderableSeries.add(
        new SplineLineRenderableSeries(wasmContext, {
          dataSeries: new XyDataSeries(wasmContext, {
            dataSeriesName: "Short MA",
            xValues: xValuesShortMa,
            yValues: yValuesShortMa,
            containsNaN: false,
            isSorted: true,
          }),
          stroke: "#4fc3f7",
          strokeThickness: 2,
        }),
      );

      sciChartSurface.renderableSeries.add(
        new SplineLineRenderableSeries(wasmContext, {
          dataSeries: new XyDataSeries(wasmContext, {
            dataSeriesName: "Long MA",
            xValues: xValuesLongMa,
            yValues: yValuesLongMa,
            containsNaN: false,
            isSorted: true,
          }),
          stroke: "#f48fb1",
          strokeThickness: 2,
        }),
      );

      sciChartSurface.chartModifiers.add(new LegendModifier());
      sciChartSurface.chartModifiers.add(new MouseWheelZoomModifier());
      sciChartSurface.chartModifiers.add(new ZoomPanModifier());
      sciChartSurface.chartModifiers.add(new XAxisDragModifier());
      sciChartSurface.chartModifiers.add(new YAxisDragModifier());
      sciChartSurface.chartModifiers.add(
        new RolloverModifier({ snapToDataPoint: true }),
      );

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
      <div
        style={{
          display: "flex",
          gap: "12px",
          alignItems: "center",
          marginBottom: "16px",
        }}
      >
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
          disabled={status === "loading"}
        >
          {status === "loading" ? "Running..." : "Run"}
        </Button>
      </div>
      <div ref={chartRef} style={{ width: "100%", position: "relative" }} />
    </div>
  );
}
