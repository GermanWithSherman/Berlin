using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectImageDialog : Dialog<SelectImageDialogSetting>
{
    public Transform ImageContainer;
    public SelectImageDialogImage SelectImagePrefab;

    /*void Start()
    {
        SelectImageDialogSetting s = new SelectImageDialogSetting();
        s.Path = "media/npc/random_f";
        setSettings(s);
    }*/

    public override void setSettings(DialogSetting settings)
    {
        _settings = (SelectImageDialogSetting)settings;

        loadFromFolder(_settings.Path);
    }

    public void imageSelect(string path)
    {

        path = path.Replace('\\','/');

        Debug.Log(path);

        _data["PATH"] = path;
        submit();
    }

    private void loadFromFolder(string path)
    {
        ImageContainer.childrenDestroyAll();

        string completePath = GameManager.Instance.path(path);

        Debug.Log($"Loading images from {completePath}");

        string[] fileNames = System.IO.Directory.GetFiles(completePath, "*.jpg");

        foreach(string fileName in fileNames)
        {
            string relativeFileName = path.Substring("media/".Length) + fileName.Substring(completePath.Length); //TODO: very bad... obviously
            SelectImageDialogImage uiImage = Instantiate(SelectImagePrefab, ImageContainer);
            uiImage.RawImage.texture = GameManager.Instance.TextureCache[relativeFileName];
            uiImage.ImagePath = relativeFileName;
            uiImage.Button.onClick.AddListener(delegate { imageSelect(uiImage.ImagePath); });
        }
    }

}

public class SelectImageDialogSetting : DialogSetting
{
    public string Path;
}
