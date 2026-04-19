export interface Note {
  id: string;
  text: string;
  createdAt: number;
}

export interface NotesState {
  notes: Note[];
}
