using Microsoft.Xna.Framework.Audio;

namespace d4lilah.Data
{
    public class SoundItem
    {
        public SoundEffectInstance Instance;
        public int ID;
        public ShellVector Position;
        public ShellSound Shell;
    }

    public class ShellSound
    {
        public int ID;
        public string Sound;
        public ShellVector Position;
    }
}
