using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer
{
    class ListViewItemComparer : System.Collections.IComparer
    {
        private int col;
        public SortOrder sort = SortOrder.None;
        public ListViewItemComparer()
        {
            col = 0;
        }

        /// <summary>
        /// 컬럼과 정렬 기준(asc, desc)을 사용하여 정렬 함.
        /// </summary>
        /// <param name="column">몇 번째 컬럼인지를 나타냄.</param>
        /// <param name="sort">정렬 방법을 나타냄. Ex) Ascending, Descending</param>

        public ListViewItemComparer(int column, SortOrder sort)
        {
            if(column==1) // 크기칼럼은 다른 패턴으로 구성되어 별도의 방식이 필요.(보류)
            {
                col = 100;
            }
            else
            {
                col = column;
            }
            this.sort = sort;
        }

        public int Compare(object x, object y)
        {
            if (col == 100)
            {
                return 0;
            }
            if (sort == SortOrder.Ascending)
                try {
                    return String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
                }
                catch
                {
                    return 0;
                }
                else
                try
                {
                    return String.Compare(((ListViewItem)y).SubItems[col].Text, ((ListViewItem)x).SubItems[col].Text);
                }
                catch
                {
                    return 0;
                }
        }
    }
}
