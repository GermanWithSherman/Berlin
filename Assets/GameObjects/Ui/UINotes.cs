using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINotes : MonoBehaviour
{
    public Transform NotesContainer;
    public UINote NotePrefab;

    public void hide() => gameObject.SetActive(false);

    public void showNotes(IEnumerable<Note> notes)
    {
        NotesContainer.childrenDestroyAll();
        foreach (Note note in notes)
        {
            UINote uiNote = Instantiate(NotePrefab, NotesContainer);
            uiNote.setNote(note);
        }
        gameObject.SetActive(true);
    }
}
