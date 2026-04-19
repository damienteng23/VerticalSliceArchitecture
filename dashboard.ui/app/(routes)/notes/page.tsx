"use client";

import { useState } from "react";
import { Button, TextField, IconButton } from "@mui/material";
import { useAppDispatch } from "../../_redux/hooks";
import { useNotes } from "../../_redux/hooks";
import {
  addNote,
  deleteNote,
} from "@/app/_redux/slices/backtesting/notes/slice";
import { MdDelete } from "react-icons/md";

export default function NotesPage() {
  const dispatch = useAppDispatch();
  const notes = useNotes();
  const [draft, setDraft] = useState("");

  const handleAdd = () => {
    if (!draft.trim()) return;
    dispatch(addNote(draft));
    setDraft("");
  };

  return (
    <div style={{ padding: 24, maxWidth: 600 }}>
      <h1>Notes</h1>

      <div style={{ display: "flex", gap: 8, marginBottom: 16 }}>
        <TextField
          label="New note"
          value={draft}
          onChange={(e) => setDraft(e.target.value)}
          onKeyDown={(e) => e.key === "Enter" && handleAdd()}
          fullWidth
          size="small"
        />
        <Button variant="contained" onClick={handleAdd}>
          Add
        </Button>
      </div>

      {notes.map((note) => (
        <div
          key={note.id}
          style={{
            display: "flex",
            justifyContent: "space-between",
            padding: "8px 0",
            borderBottom: "1px solid #333",
          }}
        >
          <span>{note.text}</span>
          <IconButton
            size="small"
            onClick={() => dispatch(deleteNote(note.id))}
          >
            <MdDelete />
          </IconButton>
        </div>
      ))}

      {notes.length === 0 && <p style={{ color: "#888" }}>No notes yet.</p>}
    </div>
  );
}
