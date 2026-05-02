import { NextRequest, NextResponse } from "next/server";
import { readFileSync } from "fs";
import { join } from "path";

type MovingAveragePoint = {
  maShortPeriod: number | null;
  maLongPeriod: number | null;
  close: number;
  date: number;
};

function sma(values: number[], period: number): (number | null)[] {
  return values.map((_, i) => {
    if (i < period - 1) return null;
    const sum = values.slice(i - period + 1, i + 1).reduce((a, b) => a + b, 0);
    return sum / period;
  });
}

export async function GET(req: NextRequest) {
  const { searchParams } = req.nextUrl;
  const symbol = (searchParams.get("symbol") ?? "").toUpperCase();
  const shortPeriod = Math.max(1, parseInt(searchParams.get("shortPeriod") ?? "10"));
  const longPeriod = Math.max(1, parseInt(searchParams.get("longPeriod") ?? "50"));

  if (!symbol) {
    return NextResponse.json({ error: "symbol is required" }, { status: 400 });
  }

  const csvPath = join(process.cwd(), "app/_shared/dailyTickerSummary.csv");
  const rows = readFileSync(csvPath, "utf-8")
    .trim()
    .split("\n")
    .map((line) => line.trim().split(","))
    .filter((cols) => cols[0].toUpperCase() === symbol)
    .map((cols) => ({
      date: new Date(cols[1].substring(0, 10)).getTime() / 1000,
      close: parseFloat(cols[5]),
    }))
    .filter((r) => !isNaN(r.date) && !isNaN(r.close))
    .sort((a, b) => a.date - b.date);

  if (rows.length === 0) {
    return NextResponse.json(
      { error: `No data found for symbol: ${symbol}` },
      { status: 404 },
    );
  }

  const closes = rows.map((r) => r.close);
  const shortMAs = sma(closes, shortPeriod);
  const longMAs = sma(closes, longPeriod);

  const result: MovingAveragePoint[] = rows.map((row, i) => ({
    date: row.date,
    close: row.close,
    maShortPeriod: shortMAs[i],
    maLongPeriod: longMAs[i],
  }));

  return NextResponse.json(result);
}
