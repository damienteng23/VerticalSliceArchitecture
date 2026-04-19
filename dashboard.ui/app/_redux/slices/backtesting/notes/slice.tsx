import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { NotesState } from './types';

const initialState: NotesState = { notes: [] };

const notesSlice = createSlice({
  name: 'notes',
  initialState,
  reducers: {
    addNote: (state, action: PayloadAction<string>) => {
      state.notes.push({
        id: crypto.randomUUID(),
        text: action.payload,
        createdAt: Date.now(),
      });
    },
    deleteNote: (state, action: PayloadAction<string>) => {
      state.notes = state.notes.filter((n) => n.id !== action.payload);
    },
  },
});

export const { addNote, deleteNote } = notesSlice.actions;
export default notesSlice.reducer;