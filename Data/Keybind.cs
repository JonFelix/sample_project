using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;


namespace d4lilah.Data
{
    public class Keybind
    {
        Keys? mKey = null;
        string mMouseKey;
        string mName;
        string mPayload = "";
        bool mKeyPressed = false;
        bool mKeyDown = false;
        bool mKeyReleased = false;     

        public string Name
        {
            get
            {
                return mName;
            }
        }

        public string Key
        {
            get
            {
                if(mKey != null)
                {
                    return mKey.ToString();
                }
                else
                {
                    return mMouseKey;
                }
            }
        }

        [JsonIgnoreAttribute]
        public Keys? XnaKey
        {
            get
            {
                return mKey;
            }
        }
        [JsonIgnoreAttribute]
        public string MouseButton
        {
            get
            {
                return mMouseKey;
            }
        }
        [JsonIgnoreAttribute]
        public bool Pressed
        {
            get
            {
                return mKeyPressed;
            }
        }
        [JsonIgnoreAttribute]
        public bool Down
        {
            get
            {
                return mKeyDown;
            }
        }
        [JsonIgnoreAttribute]
        public bool Released
        {
            get
            {
                return mKeyReleased;
            }
        }
        [JsonIgnoreAttribute]
        public string Payload
        {
            get
            {
                return mPayload;
            }
            set
            {
                mPayload = value;
            }
        }
                                
        public Keybind(string key, string name)
        {
            if(KeyTranslation.Translate(key) != null)
            {
                mKey = KeyTranslation.Translate(key);
            }
            else
            {
                mMouseKey = key;
            }
            
            mName = name;
        }

                               
        public void SetState(bool down)
        {
            mKeyReleased = (mKeyDown && !down);
            mKeyPressed = (!mKeyDown && down);
            mKeyDown = down;
        }

        public void Bind(Keys key)
        {
            mKey = key;
        }

        public string[] GetBind()
        {
            string[] res = new string[2];
            res[0] = mName;
            if(mKey == null)
            {
                res[1] = mMouseKey;
            }
            else
            {
                res[1] = mKey.ToString();
            }    
            return res;
        }
    }
}
