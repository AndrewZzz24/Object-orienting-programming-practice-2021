// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System.Collections.Generic;

namespace Banks.Tools
{
    public class MainMenuCommands
    {
        private static List<string> _menuCommands = new List<string>
        {
            "Create Central Bank",
            "Create Bank",
            "Get Banks List",
            "Add client",
            "Create Account",
            "Add money",
            "Withdraw money",
            "Exit",
        };

        public static List<string> MenuCommands => _menuCommands;
    }
}