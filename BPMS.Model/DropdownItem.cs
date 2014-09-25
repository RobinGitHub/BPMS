using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BPMS.Model
{
    /// <summary>
    /// 下拉项对象
    /// </summary>
    public class DropdownItem
    {
        public int ID { get; set; }
        public string Text { get; set; }

        public static DropdownItem GetPlease
        {
            get
            {
                DropdownItem item = new DropdownItem();
                item.ID = 0;
                item.Text = "--请选择--";
                return item;
            }
        }
    }
}
