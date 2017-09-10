using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTaskUniversity.ViewModels.TagHelper
{
    public class FilterViewModel
    {
        public FilterViewModel(string searchString = "")
        {
            CurrentFilter = searchString;
        }
        public string CurrentFilter { get; private set; }
    }
}
