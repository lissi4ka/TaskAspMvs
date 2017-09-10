using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TestTaskUniversity.ViewModels.TagHelper
{
    public class PageViewModel
    {
        public int PageNumber { get; private set; }
        public int TotalPages { get; private set; }

        public SelectList PagesList { get; private set; }
        public int? SelectedCountPage { get; private set; }

        public PageViewModel(int count, int pageNumber, int pageSize)
        {
            var pageList = new List<Page>();

            pageList.Add(new Page {Value = 10, Text = "Show 10 per page" });
            pageList.Add(new Page {Value = 25, Text = "Show 25 per page" });
            pageList.Add(new Page {Value = 50, Text = "Show 50 per page" });
            pageList.Add(new Page {Value = 100, Text = "Show 100 per page" });

            PagesList = new SelectList(pageList, "Value", "Text", pageSize);
            SelectedCountPage = pageSize;

            PageNumber = pageNumber;
            TotalPages = (int) Math.Ceiling(count / (double) pageSize);
        }

        public bool HasPreviousPage
        {
            get { return (PageNumber > 1); }
        }

        public bool HasNextPage
        {
            get { return (PageNumber < TotalPages); }
        }
    }

    public class Page
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }

}
