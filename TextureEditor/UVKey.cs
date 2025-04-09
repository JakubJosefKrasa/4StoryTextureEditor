using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextureEditor
{
    class UVKey
    {
        public uint Tick { get; set; } = 0;
        public float KeySU { get; set; } = 1.0f;
        public float KeySV { get; set; } = 1.0f;
        public float KeyU { get; set; } = 0.0f;
        public float KeyV { get; set; } = 0.0f;
        public float KeyR { get; set; } = 0.0f;
    }
}
