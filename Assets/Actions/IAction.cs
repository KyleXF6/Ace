using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Actions
{
    public interface IAction
    {
        string ID { get;  }
        string Name { get; }
        bool IsVisible { get; }
        bool CanExecute(Game game);
        void Execute(Game game);


    }
}
