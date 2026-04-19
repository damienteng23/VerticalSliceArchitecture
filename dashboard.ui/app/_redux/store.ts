import { configureStore } from '@reduxjs/toolkit';
import movingAverageReducer from './slices/backtesting/moving-average/slice';
import notesReducer from './slices/backtesting/notes/slice';

export const store = configureStore({
  reducer: {
    movingAverage: movingAverageReducer,
    notes: notesReducer, 
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;