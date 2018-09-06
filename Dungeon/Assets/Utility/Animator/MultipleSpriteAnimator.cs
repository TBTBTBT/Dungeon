using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultipleSpriteAnimator : MonoBehaviour
{
    [Header("参照するフォルダ")]
    [SerializeField] private string _folderPath = "Image/";
    [Header("ファイル名のテンプレート")]
    [SerializeField] private string _fileName = "";
    [SerializeField] private SpriteRenderer _target;
    private int _indexBefore = 0;
    [SerializeField]private float _index = 0;
    List<Sprite> _sprites = new List<Sprite>();
    public void Init(string path,string name)
    {
        SetFolderPath(path);
        SetFileName(name);
        LoadSprite();
        Debug.Log(_sprites.Count);
    }
    
    public void SetFolderPath(string path)
    {
        _folderPath = path;
    }

    public void SetFileName(string name)
    {
        _fileName = name;
    }
    void LoadSprite()
    {
        _sprites.Clear();
        var sprites = Resources.LoadAll<Sprite>(_folderPath+_fileName);
        _sprites = sprites.ToList();
        SetSprite(true);
    }

    public void SetSprite(bool immidiate = false)
    {
        int index = (int)_index;
        if (index == _indexBefore && !immidiate)
        {
            return;
        }
        if (!_sprites.CheckIndex(index))
        {
            return;
        }
        _target.sprite = _sprites[index];
        _indexBefore = index;
    }

    void LateUpdate()
    {
        SetSprite();
    }
}
