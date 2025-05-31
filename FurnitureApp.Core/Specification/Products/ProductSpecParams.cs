using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureApp.Core.Specification.Products
{
    public class ProductSpecParams
    {
        public string? sort { get; set; }
        public int? CategoryId { get; set; }
        public int? pageSize { get; set; } = 4;
        public int? pageIndex { get; set; } = 1;

        private string? search;

        public string? Search
        {
            get { return search; }
            set { search = value?.ToLower(); }
        }
    }
}
