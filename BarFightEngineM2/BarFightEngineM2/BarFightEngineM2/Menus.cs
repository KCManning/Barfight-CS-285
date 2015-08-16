using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarFightEngineM2
{
    public class Menus
    {
        private enum MenuState
        {
            MAIN,
            ROOMSELECTION,
            CHARACTERSELECTION
        }
        private Menu activeMenu;
        private MenuState state;
        private int screenWidth;
        private int screenHeight;
        public Menus()
        {

        }

        public void LoadCharacters(string charactersAddress)
        {

        }
        
    }
}
