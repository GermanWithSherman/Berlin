using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Template : IModable
{
    [JsonProperty("BackgroundColor")]
    private string _backgroundColorHTML = "fff";

    private Color? _backgroundColorCache = null;
    [JsonIgnore]
    public Color BackgroundColor
    {
        get
        {
            if(!_backgroundColorCache.HasValue)
            {
                if(ColorUtility.TryParseHtmlString(_backgroundColorHTML,out Color color))
                {
                    _backgroundColorCache = color;
                }
                else
                {
                    _backgroundColorCache = Color.white;
                    Debug.LogWarning($"Can't parse {_backgroundColorHTML} as color");
                }
            }
            return _backgroundColorCache.GetValueOrDefault();
        }
    }

    [JsonProperty("Color")]
    private string _fontColorHTML = "fff";

    private Color? _fontColorCache=null;
    [JsonIgnore]
    public Color FontColor
    {
        get
        {
            if (!_fontColorCache.HasValue)
            {
                if (!ColorUtility.TryParseHtmlString(_fontColorHTML, out Color color))
                {
                    _fontColorCache = color;
                }
                else
                {
                    _fontColorCache = Color.black;
                    Debug.LogWarning($"Can't parse {_fontColorHTML} as color");
                }
            }
            return _fontColorCache.GetValueOrDefault();
        }
    }

    public IModable copyDeep()
    {
        var result = new Template();

        result._backgroundColorHTML = Modable.copyDeep(_backgroundColorHTML);
        result._fontColorHTML = Modable.copyDeep(_fontColorHTML);

        return result;
    }

    public void mod(IModable modable)
    {
        throw new System.NotImplementedException();
    }
}
