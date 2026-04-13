import { MovingAveragePoint } from '../../../../_shared/interface/backtesting';

export type MovingAverageState = {
  rawData: MovingAveragePoint[];
  status: 'idle' | 'loading' | 'success' | 'error';
  error: string | null;
};