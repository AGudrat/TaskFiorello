using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fiorello.ViewModel.Product
{
    public class Paginate<T>
    {
        public Paginate()
        {
        }
        public Paginate(List<T> models,int currentPage,int pageCount)
        {
            int startPage = currentPage - 1;
            int endPage = currentPage + 2;

            if (startPage <= 0)
            {
                endPage = endPage - (startPage - 1);
                startPage = 1;
            }
            if (endPage > pageCount)
            {
                endPage = pageCount;
                if (endPage>5)
                {
                    startPage = endPage - 2;
                }
            }
            Items = models;
            CurrentPage = currentPage;
            PageCount = pageCount;
            StartPage = startPage;
            EndPage = endPage;
        }
        public List<T> Items { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }
    }
}
