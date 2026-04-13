import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import { backtestingApi } from '../../../../_shared/api/backtesting';
import { MovingAverageParams } from '../../../../_shared/interface/backtesting';
import { MovingAverageState } from './types';

const initialState: MovingAverageState = {
  rawData: [],
  status: 'idle',
  error: null,
};

export const runBacktest = createAsyncThunk(
  'backtesting/movingAverage',
  async (params: MovingAverageParams) => {
    return await backtestingApi.backtestMovingAverageStrategy(params);
  }
);

const movingAverageSlice = createSlice({
  name: 'backtesting',
  initialState,
  reducers: {
    clearResults: (state) => {
      state.rawData = [];
      state.status = 'idle';
      state.error = null;
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(runBacktest.pending, (state) => {
        state.status = 'loading';
        state.error = null;
      })
      .addCase(runBacktest.fulfilled, (state, action) => {
        state.status = 'success';
        state.rawData = action.payload;
      })
      .addCase(runBacktest.rejected, (state, action) => {
        state.status = 'error';
        state.error = action.error.message ?? 'Unknown error';
      });
  },
});

export const { clearResults } = movingAverageSlice.actions;
export default movingAverageSlice.reducer;