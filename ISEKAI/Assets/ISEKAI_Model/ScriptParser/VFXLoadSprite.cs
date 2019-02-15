using System;

namespace ISEKAI_Model
{
    public class VFXLoadSprite : Command
    {
        public override int commandNumber {get {return 10;}}

        public string filePath {get; private set;}
        public int width {get; private set;}
        public int height {get; private set;}
        public bool isGIF { get; private set; }

        public VFXLoadSprite(string filePath, int width, int height, bool isGIF)
        {
            this.width = width;
            this.height = height;
            this.filePath = filePath;
            this.isGIF = isGIF;
        }
    }
}