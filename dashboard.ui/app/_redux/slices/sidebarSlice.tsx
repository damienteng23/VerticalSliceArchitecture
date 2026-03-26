import { createSlice } from '@reduxjs/toolkit';

const sidebarSlice = createSlice({
  name: 'calc',
  initialState: {
    inputValue: 0,
    results: null,
  },
  reducers: {
    // This is what the "Calculate" button triggers
    updateValue: (state, action) => {
      state.inputValue = action.payload;
    },
    // This is where we store the API response later
    setApiResults: (state, action) => {
      state.results = action.payload;
    }
  },
});

export const { updateValue, setApiResults } = calculatorSlice.actions;
export default calculatorSlice.reducer;