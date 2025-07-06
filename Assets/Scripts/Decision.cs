using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AceConsole
{
    public class Decision
    {
        
        public Decision()
        {
            
        }
        public int Draw(Location l)
        {
            int count = 1;
            if (l.CanMove == true){
                System.Diagnostics.Debug.Write(count + ". Move   ");
                count++;
            }
            if (l.CanExamine == true)
            {
                System.Diagnostics.Debug.Write(count + ". Examine   ");
                count++;
            }
            if (l.CanPresent == true)
            {
                System.Diagnostics.Debug.Write(count + ". Present   ");
                count++;
            }
            if (l.CanTalk == true)
            {
                System.Diagnostics.Debug.Write(count + ". Talk   ");
                count++;
            }
            string? Ans = "y";// System.Diagnostics.Debug.ReadLine();
            int ParsedAns = -1;
            while (!int.TryParse(Ans, out ParsedAns) || ParsedAns > count || ParsedAns <= 0)
            {
                System.Diagnostics.Debug.WriteLine("Invalid Answer.");
                Ans = "y";//System.Diagnostics.Debug.ReadLine();
            }
            return ParsedAns;

        }
    }
}
