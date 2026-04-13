export type MovingAveragePoint = {
    maShortPeriod: number | null;
    maLongPeriod: number | null;
    close: number;
    date: string;
};

export type MovingAverageParams = {
    symbol: string;
    shortPeriod: number;
    longPeriod: number;
};