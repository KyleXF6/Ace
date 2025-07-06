using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AceConsole
{
    public class ItemDialogue : IStoryPart
    {
        public string ID { get; set; }
        public string NextID { get; set; }

        public bool IsDone { get; set; }

        public IStoryPart NextPart { get; set; }

        public string Line { get; set; }

        public string ItemID { get; set; }
        public Item ReferencedItem { get; set; }
        public ItemDialogue()
        {

        }
        public void Advance()
        {
            ReferencedItem.IsVisible = true;
            IsDone = true;
        }

        public void Draw()
        {
            System.Diagnostics.Debug.WriteLine(Line);
        }

    }
}
