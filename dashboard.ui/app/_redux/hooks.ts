import { useDispatch, useSelector } from 'react-redux';
import type { RootState, AppDispatch } from './store';

// default
export const useAppDispatch = () => useDispatch<AppDispatch>();
export const useAppSelector = <T>(selector: (state: RootState) => T) =>
  useSelector(selector);

// pre-built selector hook that reads rawData out of the Redux store
// and returns it.
export const useMovingAverageData = () =>
  useAppSelector((state) => state.movingAverage.rawData);

export const useMovingAverageStatus = () =>
  useAppSelector((state) => state.movingAverage.status);

export const useMovingAverageError = () =>
  useAppSelector((state) => state.movingAverage.error);

export const useNotes = () =>
  useAppSelector((state) => state.notes.notes);