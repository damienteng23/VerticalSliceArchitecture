import { NextResponse } from "next/server";
import { readFileSync } from "fs";
import { join } from "path";

export async function GET() {
  const csvPath = join(process.cwd(), "app/_shared/dailyTickerSummary.csv");
  const tickers = [
    ...new Set(
      readFileSync(csvPath, "utf-8")
        .trim()
        .split("\n")
        .map((line) => line.trim().split(",")[0].toUpperCase())
        .filter(Boolean),
    ),
  ].sort();

  return NextResponse.json(tickers);
}
