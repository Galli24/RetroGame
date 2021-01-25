using RenderEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroGame.Services
{
     class FontManager
    {

        #region Singleton

        private static FontManager _instance;
        public static FontManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new FontManager();
                return _instance;
            }
        }

        #endregion


        private Dictionary<Tuple<string, int>, Font> _fonts = new Dictionary<Tuple<string, int>, Font>();

        public Font this[string name, int size = 32]
        { 
            get
            {
                var fontPath = $"./Resources/Fonts/{name}.ttf";
                if (_fonts.TryGetValue(new Tuple<string, int>(fontPath, size), out var font))
                    return font;
                font = new Font(fontPath, size);
                _fonts.Add(new Tuple<string, int>(fontPath, size), font);
                return font;
            }
        }

    }
}
