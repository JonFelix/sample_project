
using System.Threading.Tasks;

namespace d4lilah
{
    public class LevelEditor
    {
        private Game1 _game;
        private bool _editorActive;

        public bool IsActive
        {
            get
            {
                return _editorActive;
            }
            set
            {
                _editorActive = value;
            }
        }

        public LevelEditor(Game1 game)
        {
            _game = game;
        }

        public void Update()
        {

        }
    }
}
