public class File_service
{
    public string? CurrentFilePath { get; private set; }
    public bool IsModified { get; set; }

    public void NewFile(RichTextBox editor)
    {
        editor.Clear();
        CurrentFilePath = null;
        IsModified = false;
    }

    public void LoadFile(RichTextBox editor, string path)
    {
        editor.Text = File.ReadAllText(path);
        CurrentFilePath = path;
        IsModified = false;
    }

    public void SaveFile(RichTextBox editor, string path)
    {
        File.WriteAllText(path, editor.Text);
        CurrentFilePath = path;
        IsModified = false;
    }
}
