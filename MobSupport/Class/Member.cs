using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MobSupport.Class
{
    internal class Member
    {
        public int Id { get; }
        public ReactivePropertySlim<string> Name { get; }

        public string IconNumber { get; }

        public ReactivePropertySlim<ImageSource> IconPath { get; }

        public Member(int id, string name, string iconNum, ImageSource iconPath)
        {
            if (string.IsNullOrEmpty(name)) { throw new ArgumentNullException("nameを空白にすることはできません"); };
            Id = id;
            Name = new(name);
            IconNumber = iconNum;
            IconPath = new(iconPath);
            
        }
    }
}
