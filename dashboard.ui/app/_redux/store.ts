import { configureStore } from '@reduxjs/toolkit';
import movingAverageReducer from './slices/backtesting/moving-average/slice';

export const store = configureStore({
  reducer: {
    movingAverage: movingAverageReducer,
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
