using System.Collections.Generic;


namespace GioBot.Resources.DataType
{
    public class Settings
    {
        public string token { get; set; }
        public ulong owner { get; set; }
        public List<ulong> log { get; set; }
        public string version { get; set; }        
    }
}
