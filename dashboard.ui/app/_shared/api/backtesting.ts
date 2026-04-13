import { apiFetch } from "./client";
import { MovingAverageParams, MovingAveragePoint } from "../interface/backtesting";

const backtestMovingAverageApiUrl = '/api/backtesting/backtestMovingAverageStrategy';

export const backtestingApi = {
    backtestMovingAverageStrategy: (params: MovingAverageParams) =>
        apiFetch<MovingAveragePoint[]>(
            `${backtestMovingAverageApiUrl}?symbol=${encodeURIComponent(params.symbol)}&shortPeriod=${params.shortPeriod}&longPeriod=${params.longPeriod}`,
            { method: "GET" }
        ),
};
